using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton; // 开始按钮
    public Button settingsButton; // 设置按钮
    public Button exitButton; // 退出按钮
    public Button returnButton; // 设置面板中的返回按钮
    public GameObject settingsPanel; // 设置面板
    public GameObject mainMenuPanel; // 主菜单面板
    public CameraController cameraController; // 摄像机控制器引用

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);
        returnButton.onClick.AddListener(CloseSettings);

        settingsPanel.SetActive(false); // 初始化时隐藏设置面板
        mainMenuPanel.SetActive(true);  // 确保主菜单面板处于激活状态
    }

    void StartGame()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(true);
        cameraController.MoveToSettings();
    }

    void CloseSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(true);
        cameraController.MoveToMainMenu();
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
