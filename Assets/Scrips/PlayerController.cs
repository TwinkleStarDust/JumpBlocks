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
    private SettingsMenu settingsMenu; // ����SettingsMenu

    // ��������Ч
    public AudioClip jumpSound; // ȷ������ֶ���public
    public AudioClip WinSound; // �ɹ���Ч
    public AudioClip LoseSound; // ʧ����Ч

    private AudioSource audioSource; // ��������ƵԴ

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        step = FindObjectOfType<GameManager>().step;
        initialPosition = transform.position; // ��¼��ʼλ��
        winManager = FindObjectOfType<WinManager>(); // ��ȡ WinManager ����
        settingsMenu = FindObjectOfType<SettingsMenu>(); // ��ȡ SettingsMenu ����

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

        audioSource = gameObject.AddComponent<AudioSource>(); // �����ƵԴ���
        audioSource.volume = 0.3f; // ��������
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFinished || hasDied) // �����Ϸ���������������ֹͣ������
            return;

        if (!gameStarted && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            gameStarted = true;
            startTime = Time.timeSinceLevelLoad; // ��¼��Ϸ��ʼʱ��
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
                // ������Ծ��Ч
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
                print("ţ�ƣ����ͨ��ʱ���ǣ�" + (Time.timeSinceLevelLoad - startTime));
                hasFinished = true; // ���Ϊ��ͨ��

                if (settingsMenu != null && settingsMenu.IsSFXEnabled())
                {
                    // ���ųɹ���Ч
                    audioSource.PlayOneShot(WinSound);
                }

                if (winManager != null)
                {
                    winManager.ShowWinPanel(Time.timeSinceLevelLoad - startTime); // ��ʾʤ�����
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
                    print("�����ˣ���Ĵ��ʱ���ǣ�" + (Time.timeSinceLevelLoad - startTime));
                    hasDied = true; // ���Ϊ������
                    FindObjectOfType<GameManager>().ResetPlayerPosition(); // �������λ��

                    if (settingsMenu != null && settingsMenu.IsSFXEnabled())
                    {
                        // ����ʧ����Ч
                        audioSource.PlayOneShot(LoseSound);
                    }
                }
            }
            else
            {
                fallTime = 0f; // �������ڵ����ϣ����õ����ʱ��
            }
        }
    }

    // �������������������������״̬
    public void SetHasDied(bool value)
    {
        hasDied = value;
    }

    // ��������������������Ϸ״̬
    public void ResetGame()
    {
        hasFinished = false;
        hasDied = false;
        gameStarted = false;
    }
}
