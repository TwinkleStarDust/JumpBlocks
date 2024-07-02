using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button level1Button; // ��һ�ذ�ť
    public Button level2Button; // �ڶ��ذ�ť
    public Button level3Button; // �����ذ�ť
    public Button backButton;   // �������˵���ť

    private void Start()
    {
        level1Button.onClick.AddListener(() => LoadLevel("Level-1"));
        level2Button.onClick.AddListener(() => LoadLevel("Level-2"));
        level3Button.onClick.AddListener(() => LoadLevel("Level-3"));
        backButton.onClick.AddListener(ReturnToMainMenu); // �������˵�
    }

    void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
