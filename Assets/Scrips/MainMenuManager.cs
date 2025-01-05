using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button settingsButton;
    public Button exitButton;
    public GameObject settingsPanel;
    private bool isTransitioning = false;

    private void Start()
    {
        // 确保SoundManager实例存在
        SoundManager soundManager = SoundManager.Instance;

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClick);
        }

        // 为所有按钮添加点击音效
        soundManager.AddClickSoundToAllButtons();
    }

    private void OnStartButtonClick()
    {
        if (isTransitioning) return;

        isTransitioning = true;
        DisableAllButtons();
        SceneTransitionManager.Instance.LoadSceneWithTransition("LevelSelection", 1f);
    }

    private void OnSettingsButtonClick()
    {
        if (isTransitioning) return;

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    private void OnExitButtonClick()
    {
        if (isTransitioning) return;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void DisableAllButtons()
    {
        if (startButton != null) startButton.interactable = false;
        if (settingsButton != null) settingsButton.interactable = false;
        if (exitButton != null) exitButton.interactable = false;
    }
}