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
        // 获取Canvas引用
        winPanelCanvas = GetComponentInParent<Canvas>();
        if (winPanelCanvas == null)
        {
            Debug.LogError("Cannot find Canvas component in parent!");
            return;
        }

        // 检查组件引用
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

        // 初始化时隐藏Canvas和Panel
        winPanelCanvas.enabled = false;
        winPanel.SetActive(false);
        Debug.Log("WinManager Awake: Canvas and Panel hidden");

        // 为按钮添加事件监听器
        retryButton.onClick.AddListener(RetryLevel);
        levelSelectButton.onClick.AddListener(LevelSelect);

        isInitialized = true;
    }

    // 显示胜利面板和通关时间
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
        // 先启用Canvas，再显示Panel
        winPanelCanvas.enabled = true;
        winPanel.SetActive(true);
        winText.text = "牛逼，你的通关时间是：" + time.ToString("F3") + "秒";
        Debug.Log("Win panel shown successfully");
    }

    // 重新挑战当前关卡
    void RetryLevel()
    {
        hasWon = false;
        // 隐藏Canvas和Panel
        if (winPanelCanvas != null) winPanelCanvas.enabled = false;
        if (winPanel != null) winPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 返回关卡选择
    void LevelSelect()
    {
        hasWon = false;
        // 隐藏Canvas和Panel
        if (winPanelCanvas != null) winPanelCanvas.enabled = false;
        if (winPanel != null) winPanel.SetActive(false);
        SceneManager.LoadScene("LevelSelection");
    }
}
