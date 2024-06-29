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

        // Load saved settings
        bgmToggle.isOn = PlayerPrefs.GetInt("��������", 1) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt("��Ч", 1) == 1;
        sfxEnabled = sfxToggle.isOn;

        // Apply saved settings
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
        PlayerPrefs.SetInt("��������", isOn ? 1 : 0);
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
        PlayerPrefs.SetInt("��Ч", isOn ? 1 : 0);
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
