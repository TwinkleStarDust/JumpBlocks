using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // 引用 TextMeshPro 命名空间

public class WinManager : MonoBehaviour
{
    public GameObject winPanel;
    public TextMeshProUGUI winText; // 修改为 TextMeshProUGUI
    public Button retryButton;
    public Button levelSelectButton;

    private void Start()
    {
        // 结算面板
        winPanel.SetActive(true);

        // 为按钮添加事件监听器
        retryButton.onClick.AddListener(RetryLevel);
        levelSelectButton.onClick.AddListener(LevelSelect);
    }

    // 显示胜利面板和通关时间
    public void ShowWinPanel(float time)
    {
        winPanel.SetActive(true);
        winText.text = "牛逼，你的通关时间是：" + time.ToString("F3") + "秒";
    }

    // 重新挑战当前关卡
    void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 返回关卡选择
    void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
