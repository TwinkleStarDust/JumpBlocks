using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button level1Button; // 第一关按钮
    public Button level2Button; // 第二关按钮
    public Button level3Button; // 第三关按钮
    public Button backButton;   // 返回主菜单按钮

    private void Start()
    {
        level1Button.onClick.AddListener(() => LoadLevel("Level-1"));
        level2Button.onClick.AddListener(() => LoadLevel("Level-2"));
        level3Button.onClick.AddListener(() => LoadLevel("Level-3"));
        backButton.onClick.AddListener(ReturnToMainMenu); // 返回主菜单
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
