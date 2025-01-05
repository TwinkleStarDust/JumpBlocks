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
    private SettingsMenu settingsMenu;

    public AudioClip jumpSound;
    public AudioClip WinSound;
    public AudioClip LoseSound;

    private AudioSource audioSource;

    void Awake()
    {
        // 在Awake中获取WinManager引用
        winManager = FindObjectOfType<WinManager>();
        if (winManager == null)
        {
            Debug.LogError("Cannot find WinManager in the scene!");
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        step = FindObjectOfType<GameManager>().step;
        initialPosition = transform.position; // 记录初始位置
        settingsMenu = FindObjectOfType<SettingsMenu>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.3f; // 设置音量
    }

    void Update()
    {
        if (hasFinished) // 只在胜利时停止输入检测，死亡时仍然可以继续
            return;

        if (!gameStarted && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            gameStarted = true;
            startTime = Time.timeSinceLevelLoad;
        }

        if (animIsPlaying)
            return;

        if (Input.GetKeyDown(KeyCode.D))
            inputKeyIndex = 1;
        else if (Input.GetKeyDown(KeyCode.A))
            inputKeyIndex = -1;
    }

    void FixedUpdate()
    {
        if (inputKeyIndex != 0 && !animIsPlaying)
        {
            if (settingsMenu != null && settingsMenu.IsSFXEnabled())
            {
                audioSource.PlayOneShot(jumpSound);
            }

            var jupAnim = rb.DOJump(rb.position + new Vector2(step.x * inputKeyIndex, step.y), jumpForce, 1, 0.15f).SetEase(Ease.OutCubic).OnComplete(() => { animIsPlaying = false; });
            inputKeyIndex = 0;
            animIsPlaying = jupAnim.IsPlaying();
        }

        if (gameStarted && !animIsPlaying)
        {
            // 检查是否到达终点
            if (!hasFinished && !hasDied && Physics2D.CircleCast(transform.position, 0.2f, Vector2.zero, 0f, LayerMask.GetMask("Goal")))
            {
                float completionTime = Time.timeSinceLevelLoad - startTime;
                print("你的通关时间是：" + completionTime);
                hasFinished = true;

                if (settingsMenu != null && settingsMenu.IsSFXEnabled())
                {
                    audioSource.PlayOneShot(WinSound);
                }

                if (winManager != null)
                {
                    winManager.ShowWinPanel(completionTime);
                }
                else
                {
                    Debug.LogError("WinManager reference is null.");
                }
            }

            // 检查是否掉落
            if (!hasFinished && !Physics2D.CircleCast(transform.position, 0.2f, Vector2.down, 0.1f))
            {
                fallTime += Time.deltaTime;
                if (!hasDied && fallTime >= fallThreshold)
                {
                    print("你死了");
                    hasDied = true;

                    if (settingsMenu != null && settingsMenu.IsSFXEnabled())
                    {
                        audioSource.PlayOneShot(LoseSound);
                    }

                    FindObjectOfType<GameManager>().ResetPlayerPosition();
                    ResetGame(); // 重置游戏状态，允许继续游戏
                }
            }
            else
            {
                fallTime = 0f;
            }
        }
    }

    // 设置玩家死亡状态
    public void SetHasDied(bool value)
    {
        hasDied = value;
    }

    // 重置游戏状态
    public void ResetGame()
    {
        hasFinished = false;
        hasDied = false;
        gameStarted = false;
        fallTime = 0f;
        startTime = Time.timeSinceLevelLoad;
    }
}
