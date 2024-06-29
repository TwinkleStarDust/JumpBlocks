using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton; // ��ʼ��ť
    public Button settingsButton; // ���ð�ť
    public Button exitButton; // �˳���ť
    public Button returnButton; // ��������еķ��ذ�ť
    public GameObject settingsPanel; // �����������
    public GameObject mainMenuPanel; // ���˵��������
    public CameraController cameraController; // �������������

    private BGMManager bgmManager; // BGM ����������

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);
        returnButton.onClick.AddListener(CloseSettings);

        settingsPanel.SetActive(false); // ��ʼ�����������
        mainMenuPanel.SetActive(true);  // ȷ�����˵�����Ǽ����

        bgmManager = FindObjectOfType<BGMManager>();
        if (bgmManager != null)
        {
            bgmManager.PlayBGM(); // ���ű�������
        }
        else
        {
            Debug.LogError("�ڳ�����δ�ҵ� BGMManager��");
        }

        // ����Ҫ�����Ƿ��ѷ���
        if (mainMenuPanel == null)
        {
            Debug.LogError("MainMenuPanel δ���䡣");
        }
        else
        {
            Debug.Log("MainMenuPanel �ѷ��䡣");
        }

        if (settingsPanel == null)
        {
            Debug.LogError("SettingsPanel δ���䡣");
        }
        else
        {
            Debug.Log("SettingsPanel �ѷ��䡣");
        }

        if (cameraController == null)
        {
            Debug.LogError("CameraController δ���䡣");
        }
        else
        {
            Debug.Log("CameraController �ѷ��䡣");
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("LevelSelection"); // ���عؿ�ѡ�񳡾�
    }

    void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false); // �������˵����
        if (cameraController != null)
        {
            cameraController.MoveToSettings(); // �ƶ����������λ��
        }
        else
        {
            Debug.LogError("�� OpenSettings �� CameraController Ϊ null��");
        }
    }

    void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true); // ��ʾ���˵����
        if (cameraController != null)
        {
            cameraController.MoveToMainMenu(); // �ƶ���������˵�λ��
        }
        else
        {
            Debug.LogError("�� CloseSettings �� CameraController Ϊ null��");
        }
    }

    void ExitGame()
    {
        Application.Quit(); // �˳���Ϸ
    }
}
