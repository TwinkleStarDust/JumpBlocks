using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // ���� TextMeshPro �����ռ�

public class WinManager : MonoBehaviour
{
    public GameObject winPanel;
    public TextMeshProUGUI winText; // �޸�Ϊ TextMeshProUGUI
    public Button retryButton;
    public Button levelSelectButton;

    private void Start()
    {
        // �������
        winPanel.SetActive(true);

        // Ϊ��ť����¼�������
        retryButton.onClick.AddListener(RetryLevel);
        levelSelectButton.onClick.AddListener(LevelSelect);
    }

    // ��ʾʤ������ͨ��ʱ��
    public void ShowWinPanel(float time)
    {
        winPanel.SetActive(true);
        winText.text = "ţ�ƣ����ͨ��ʱ���ǣ�" + time.ToString("F3") + "��";
    }

    // ������ս��ǰ�ؿ�
    void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ���عؿ�ѡ��
    void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
