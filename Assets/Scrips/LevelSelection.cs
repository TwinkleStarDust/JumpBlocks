using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    // Add more buttons as needed for other levels
    public Button returnButton; // ·µ»Ø°´Å¥

    private void Start()
    {
        level1Button.onClick.AddListener(() => LoadLevel("Level-1"));
        level2Button.onClick.AddListener(() => LoadLevel("Level-2"));
        level3Button.onClick.AddListener(() => LoadLevel("Level-3"));
        // Add more listeners as needed for other levels

        returnButton.onClick.AddListener(ReturnToMainMenu); // ·µ»Ø°´Å¥¼àÌıÆ÷
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
