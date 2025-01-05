using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SoundManager");
                    instance = go.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    public AudioClip clickSound;
    private AudioSource audioSource;
    private SettingsMenu settingsMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();

            // 添加场景加载事件监听
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Debug.Log("销毁重复的SoundManager实例");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // 移除场景加载事件监听
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 场景加载完成时的回调
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"场景 {scene.name} 已加载，正在初始化音效系统");

        // 获取新场景中的SettingsMenu
        settingsMenu = FindObjectOfType<SettingsMenu>();

        // 为新场景中的所有按钮添加点击音效
        AddClickSoundToAllButtons();
    }

    private void Start()
    {
        // 获取初始场景的SettingsMenu
        settingsMenu = FindObjectOfType<SettingsMenu>();

        // 为初始场景的按钮添加点击音效
        AddClickSoundToAllButtons();
    }

    private void InitializeAudio()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;

        // 尝试加载点击音效
        clickSound = Resources.Load<AudioClip>("Sound/click");

        // 如果加载失败，尝试不同的路径
        if (clickSound == null)
        {
            // 尝试直接从Sound文件夹加载
            clickSound = Resources.Load<AudioClip>("click");

            if (clickSound == null)
            {
                Debug.LogError("Cannot find click sound effect! Please ensure the audio file is placed in: Assets/Resources/Sound/click");
                Debug.LogError("或直接放在 Assets/Resources/click");

                // 列出Resources文件夹中的所有音频文件
                Object[] audioFiles = Resources.LoadAll("", typeof(AudioClip));
                if (audioFiles.Length > 0)
                {
                    Debug.Log("在Resources文件夹中找到的音频文件：");
                    foreach (Object audioFile in audioFiles)
                    {
                        Debug.Log("- " + audioFile.name);
                    }
                }
                else
                {
                    Debug.Log("Resources文件夹中没有找到任何音频文件");
                }
            }
        }
    }

    public void PlayClickSound()
    {
        // 更新SettingsMenu引用（以防它在场景切换时丢失）
        if (settingsMenu == null)
        {
            settingsMenu = FindObjectOfType<SettingsMenu>();
        }

        if (settingsMenu != null && !settingsMenu.IsSFXEnabled())
        {
            return;
        }

        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
            Debug.Log("播放点击音效");
        }
        else
        {
            Debug.LogWarning("尝试播放音效，但音效文件未加载");
        }
    }

    public void AddClickSoundToAllButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        Debug.Log($"找到 {buttons.Length} 个按钮，正在添加点击音效");

        foreach (Button button in buttons)
        {
            // 移除之前可能存在的点击音效监听器，避免重复
            button.onClick.RemoveListener(PlayClickSound);
            // 添加点击音效监听器
            button.onClick.AddListener(PlayClickSound);
            Debug.Log($"已为按钮 '{button.name}' 添加点击音效");
        }
    }

    public void AddClickSoundToButton(Button button)
    {
        if (button != null)
        {
            button.onClick.RemoveListener(PlayClickSound);
            button.onClick.AddListener(PlayClickSound);
            Debug.Log($"已为按钮 '{button.name}' 添加点击音效");
        }
    }
}