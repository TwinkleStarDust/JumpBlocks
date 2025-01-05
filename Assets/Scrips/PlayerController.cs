using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    int inputKeyIndex;

    public float jumpForce = 2.5f;
    Vector2 step;
    bool animIsPlaying;
    bool hasFinished = false; // ͨ��״̬��־
    bool hasDied = false; // ����״̬��־
    bool gameStarted = false; // ��Ϸ�Ƿ��ѿ�ʼ��־
    float startTime; // ��Ϸ��ʼʱ��
    float fallTime = 0f;
    float fallThreshold = 0.2f; // �����ж�ʱ����ֵ

    private Vector2 initialPosition;
    private WinManager winManager;
    private SettingsMenu settingsMenu;

    public AudioClip jumpSound;
    public AudioClip WinSound;
    public AudioClip LoseSound;

    private AudioSource audioSource;

    void Awake()
    {
        // ��Awake�л�ȡWinManager����
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
        initialPosition = transform.position; // ��¼��ʼλ��
        settingsMenu = FindObjectOfType<SettingsMenu>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.3f; // ��������
    }

    void Update()
    {
        if (hasFinished) // ֻ��ʤ��ʱֹͣ�����⣬����ʱ��Ȼ���Լ���
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
            // ����Ƿ񵽴��յ�
            if (!hasFinished && !hasDied && Physics2D.CircleCast(transform.position, 0.2f, Vector2.zero, 0f, LayerMask.GetMask("Goal")))
            {
                float completionTime = Time.timeSinceLevelLoad - startTime;
                print("���ͨ��ʱ���ǣ�" + completionTime);
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

            // ����Ƿ����
            if (!hasFinished && !Physics2D.CircleCast(transform.position, 0.2f, Vector2.down, 0.1f))
            {
                fallTime += Time.deltaTime;
                if (!hasDied && fallTime >= fallThreshold)
                {
                    print("������");
                    hasDied = true;

                    if (settingsMenu != null && settingsMenu.IsSFXEnabled())
                    {
                        audioSource.PlayOneShot(LoseSound);
                    }

                    FindObjectOfType<GameManager>().ResetPlayerPosition();
                    ResetGame(); // ������Ϸ״̬�����������Ϸ
                }
            }
            else
            {
                fallTime = 0f;
            }
        }
    }

    // �����������״̬
    public void SetHasDied(bool value)
    {
        hasDied = value;
    }

    // ������Ϸ״̬
    public void ResetGame()
    {
        hasFinished = false;
        hasDied = false;
        gameStarted = false;
        fallTime = 0f;
        startTime = Time.timeSinceLevelLoad;
    }
}
