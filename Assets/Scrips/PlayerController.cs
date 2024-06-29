using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    int inputKeyIndex;

    public float jumpForce = 2.5f;
    Vector2 step;
    bool animIsPlaying;
    bool hasFinished = false; // 通关状态标志
    bool hasDied = false; // 死亡状态标志
    bool gameStarted = false; // 游戏是否已开始标志
    float startTime; // 游戏开始时间
    float fallTime = 0f;
    float fallThreshold = 0.2f; // 掉落判定时间阈值

    private Vector2 initialPosition;
    private WinManager winManager;
    private SettingsMenu settingsMenu; // 引用SettingsMenu

    // 新增：音效
    public AudioClip jumpSound; // 确保这个字段是public
    public AudioClip WinSound; // 成功音效
    public AudioClip LoseSound; // 失败音效

    private AudioSource audioSource; // 新增：音频源

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        step = FindObjectOfType<GameManager>().step;
        initialPosition = transform.position; // 记录初始位置
        winManager = FindObjectOfType<WinManager>(); // 获取 WinManager 引用
        settingsMenu = FindObjectOfType<SettingsMenu>(); // 获取 SettingsMenu 引用

        if (winManager == null)
        {
            Debug.LogError("WinManager not found in the scene.");
        }

        if (settingsMenu == null)
        {
            Debug.LogError("SettingsMenu not found in the scene.");
        }
        else
        {
            Debug.Log("SettingsMenu successfully found.");
        }

        audioSource = gameObject.AddComponent<AudioSource>(); // 添加音频源组件
        audioSource.volume = 0.3f; // 设置音量
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFinished || hasDied) // 如果游戏结束或玩家死亡，停止输入检测
            return;

        if (!gameStarted && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            gameStarted = true;
            startTime = Time.timeSinceLevelLoad; // 记录游戏开始时间
        }

        if (animIsPlaying)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            inputKeyIndex = 1;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            inputKeyIndex = -1;
    }

    void FixedUpdate()
    {
        if (inputKeyIndex != 0 && !animIsPlaying)
        {
            if (settingsMenu != null && settingsMenu.IsSFXEnabled())
            {
                // 播放跳跃音效
                audioSource.PlayOneShot(jumpSound);
            }

            var jupAnim = rb.DOJump(rb.position + new Vector2(step.x * inputKeyIndex, step.y), jumpForce, 1, 0.15f).SetEase(Ease.OutCubic).OnComplete(() => { animIsPlaying = false; });
            inputKeyIndex = 0;
            animIsPlaying = jupAnim.IsPlaying();
        }

        if (gameStarted && !animIsPlaying)
        {
            if (!hasFinished && Physics2D.CircleCast(transform.position, 0.2f, Vector2.zero, 0f, LayerMask.GetMask("Goal")))
            {
                print("牛逼，你的通关时间是：" + (Time.timeSinceLevelLoad - startTime));
                hasFinished = true; // 标记为已通关

                if (settingsMenu != null && settingsMenu.IsSFXEnabled())
                {
                    // 播放成功音效
                    audioSource.PlayOneShot(WinSound);
                }

                if (winManager != null)
                {
                    winManager.ShowWinPanel(Time.timeSinceLevelLoad - startTime); // 显示胜利面板
                }
                else
                {
                    Debug.LogError("WinManager reference is null.");
                }
            }

            if (!hasFinished && !Physics2D.CircleCast(transform.position, 0.2f, Vector2.down, 0.1f))
            {
                fallTime += Time.deltaTime;
                if (!hasDied && fallTime >= fallThreshold)
                {
                    print("你死了，你的存活时间是：" + (Time.timeSinceLevelLoad - startTime));
                    hasDied = true; // 标记为已死亡
                    FindObjectOfType<GameManager>().ResetPlayerPosition(); // 重置玩家位置

                    if (settingsMenu != null && settingsMenu.IsSFXEnabled())
                    {
                        // 播放失败音效
                        audioSource.PlayOneShot(LoseSound);
                    }
                }
            }
            else
            {
                fallTime = 0f; // 如果玩家在地面上，重置掉落计时器
            }
        }
    }

    // 公共方法，用于设置玩家死亡状态
    public void SetHasDied(bool value)
    {
        hasDied = value;
    }

    // 公共方法，用于重置游戏状态
    public void ResetGame()
    {
        hasFinished = false;
        hasDied = false;
        gameStarted = false;
    }
}
