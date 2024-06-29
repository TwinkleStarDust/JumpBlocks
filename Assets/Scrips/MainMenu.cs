using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton; // 开始按钮
    public Button settingsButton; // 设置按钮
    public Button exitButton; // 退出按钮
    public Button returnButton; // 设置面板中的返回按钮
    public GameObject settingsPanel; // 设置面板引用
    public GameObject mainMenuPanel; // 主菜单面板引用
    public CameraController cameraController; // 相机控制器引用

    private BGMManager bgmManager; // BGM 管理器引用

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);
        returnButton.onClick.AddListener(CloseSettings);

        settingsPanel.SetActive(false); // 初始隐藏设置面板
        mainMenuPanel.SetActive(true);  // 确保主菜单面板是激活的

        bgmManager = FindObjectOfType<BGMManager>();
        if (bgmManager != null)
        {
            bgmManager.PlayBGM(); // 播放背景音乐
        }
        else
        {
            Debug.LogError("在场景中未找到 BGMManager。");
        }

        // 检查必要引用是否已分配
        if (mainMenuPanel == null)
        {
            Debug.LogError("MainMenuPanel 未分配。");
        }
        else
        {
            Debug.Log("MainMenuPanel 已分配。");
        }

        if (settingsPanel == null)
        {
            Debug.LogError("SettingsPanel 未分配。");
        }
        else
        {
            Debug.Log("SettingsPanel 已分配。");
        }

        if (cameraController == null)
        {
            Debug.LogError("CameraController 未分配。");
        }
        else
        {
            Debug.Log("CameraController 已分配。");
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("LevelSelection"); // 加载关卡选择场景
    }

    void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false); // 隐藏主菜单面板
        if (cameraController != null)
        {
            cameraController.MoveToSettings(); // 移动相机到设置位置
        }
        else
        {
            Debug.LogError("在 OpenSettings 中 CameraController 为 null。");
        }
    }

    void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true); // 显示主菜单面板
        if (cameraController != null)
        {
            cameraController.MoveToMainMenu(); // 移动相机到主菜单位置
        }
        else
        {
            Debug.LogError("在 CloseSettings 中 CameraController 为 null。");
        }
    }

    void ExitGame()
    {
        Application.Quit(); // 退出游戏
    }
}
