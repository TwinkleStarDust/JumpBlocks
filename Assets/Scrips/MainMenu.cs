using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton; // ��ʼ��ť
    public Button settingsButton; // ���ð�ť
    public Button exitButton; // �˳���ť
    public Button returnButton; // ��������еķ��ذ�ť
    public GameObject settingsPanel; // �������
    public GameObject mainMenuPanel; // ���˵����
    public CameraController cameraController; // ���������������

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);
        returnButton.onClick.AddListener(CloseSettings);

        settingsPanel.SetActive(false); // ��ʼ��ʱ�����������
        mainMenuPanel.SetActive(true);  // ȷ�����˵���崦�ڼ���״̬
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
