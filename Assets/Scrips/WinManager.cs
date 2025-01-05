using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WinManager : MonoBehaviour
{
    public GameObject winPanel;
    public TextMeshProUGUI winText;
    public Button retryButton;
    public Button levelSelectButton;
    private bool hasWon = false;
    private bool isInitialized = false;
    private Canvas winPanelCanvas;

    private void Awake()
    {
        // ��ȡCanvas����
        winPanelCanvas = GetComponentInParent<Canvas>();
        if (winPanelCanvas == null)
        {
            Debug.LogError("Cannot find Canvas component in parent!");
            return;
        }

        // ����������
        if (winPanel == null)
        {
            Debug.LogError("WinPanel is not assigned in WinManager!");
            return;
        }
        if (winText == null)
        {
            Debug.LogError("WinText is not assigned in WinManager!");
            return;
        }
        if (retryButton == null)
        {
            Debug.LogError("RetryButton is not assigned in WinManager!");
            return;
        }
        if (levelSelectButton == null)
        {
            Debug.LogError("LevelSelectButton is not assigned in WinManager!");
            return;
        }

        // ��ʼ��ʱ����Canvas��Panel
        winPanelCanvas.enabled = false;
        winPanel.SetActive(false);
        Debug.Log("WinManager Awake: Canvas and Panel hidden");

        // Ϊ��ť����¼�������
        retryButton.onClick.AddListener(RetryLevel);
        levelSelectButton.onClick.AddListener(LevelSelect);

        isInitialized = true;
    }

    // ��ʾʤ������ͨ��ʱ��
    public void ShowWinPanel(float time)
    {
        Debug.Log("ShowWinPanel called with time: " + time);
        if (!isInitialized)
        {
            Debug.LogError("WinManager not initialized yet!");
            return;
        }

        if (winPanel == null || winText == null || winPanelCanvas == null)
        {
            Debug.LogError("Required components are null in WinManager!");
            return;
        }

        hasWon = true;
        // ������Canvas������ʾPanel
        winPanelCanvas.enabled = true;
        winPanel.SetActive(true);
        winText.text = "ţ�ƣ����ͨ��ʱ���ǣ�" + time.ToString("F3") + "��";
        Debug.Log("Win panel shown successfully");
    }

    // ������ս��ǰ�ؿ�
    void RetryLevel()
    {
        hasWon = false;
        // ����Canvas��Panel
        if (winPanelCanvas != null) winPanelCanvas.enabled = false;
        if (winPanel != null) winPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ���عؿ�ѡ��
    void LevelSelect()
    {
        hasWon = false;
        // ����Canvas��Panel
        if (winPanelCanvas != null) winPanelCanvas.enabled = false;
        if (winPanel != null) winPanel.SetActive(false);
        SceneManager.LoadScene("LevelSelection");
    }
}
