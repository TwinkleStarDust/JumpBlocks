using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Toggle bgmToggle;
    public Toggle sfxToggle;
    public Button backButton;
    public GameObject settingsPanel;

    private BGMManager bgmManager;
    private bool sfxEnabled;

    private void Start()
    {
        bgmToggle.onValueChanged.AddListener(SetBGM);
        sfxToggle.onValueChanged.AddListener(SetSFX);
        backButton.onClick.AddListener(CloseSettings);

        // 加载保存设置
        bgmToggle.isOn = PlayerPrefs.GetInt("背景音乐", 1) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt("音效", 1) == 1;
        sfxEnabled = sfxToggle.isOn;

        // 应用保存的设置
        SetBGM(bgmToggle.isOn);
        SetSFX(sfxToggle.isOn);

        bgmManager = FindObjectOfType<BGMManager>();
        if (bgmManager == null)
        {
            Debug.LogError("BGMManager not found in the scene.");
        }
    }

    void SetBGM(bool isOn)
    {
        PlayerPrefs.SetInt("背景音乐", isOn ? 1 : 0);
        if (bgmManager != null)
        {
            if (isOn)
            {
                bgmManager.PlayBGM();
            }
            else
            {
                bgmManager.StopBGM();
            }
        }
    }

    void SetSFX(bool isOn)
    {
        PlayerPrefs.SetInt("音效", isOn ? 1 : 0);
        sfxEnabled = isOn;
    }

    void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public bool IsSFXEnabled()
    {
        return sfxEnabled;
    }
}
