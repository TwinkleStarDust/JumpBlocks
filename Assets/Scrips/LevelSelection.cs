using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;
    public Button level5Button;
    public Button level6Button;
    public Button level7Button;
    public Button level8Button;
    public Button level9Button;
    public Button level10Button;
    public Button backButton;

    private void Start()
    {
        // 为所有按钮添加点击事件监听器
        if (level1Button != null)
            level1Button.onClick.AddListener(() => LoadLevel(1));

        if (level2Button != null)
            level2Button.onClick.AddListener(() => LoadLevel(2));

        if (level3Button != null)
            level3Button.onClick.AddListener(() => LoadLevel(3));

        if (level4Button != null)
            level4Button.onClick.AddListener(() => LoadLevel(4));

        if (level5Button != null)
            level5Button.onClick.AddListener(() => LoadLevel(5));

        if (level6Button != null)
            level6Button.onClick.AddListener(() => LoadLevel(6));

        if (level7Button != null)
            level7Button.onClick.AddListener(() => LoadLevel(7));

        if (level8Button != null)
            level8Button.onClick.AddListener(() => LoadLevel(8));

        if (level9Button != null)
            level9Button.onClick.AddListener(() => LoadLevel(9));

        if (level10Button != null)
            level10Button.onClick.AddListener(() => LoadLevel(10));

        if (backButton != null)
            backButton.onClick.AddListener(ReturnToMainMenu);
    }

    void LoadLevel(int levelNumber)
    {
        string sceneName = $"Level-{levelNumber}";
        Debug.Log($"正在加载场景：{sceneName}");
        SceneTransitionManager.Instance.LoadSceneWithTransition(sceneName);
    }

    void ReturnToMainMenu()
    {
        Debug.Log("正在返回主菜单");
        SceneTransitionManager.Instance.LoadSceneWithTransition("MainMenu");
    }
}
