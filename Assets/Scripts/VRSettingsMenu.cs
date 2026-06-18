using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Проста VR-панель налаштувань: гучність гри, музика, без звуку.
// Зберігає у PlayerPrefs і застосовує глобально через AudioListener.volume.
public class VRSettingsMenu : MonoBehaviour
{
    [Header("Панелі")]
    public GameObject settingsPanel;   // власне вікно налаштувань (ховається/показується)
    public GameObject mainButtons;     // група головних кнопок (ховаємо під час налаштувань)
    [Tooltip("Додаткові обʼєкти (заголовок, версія, підказки), які ховати під час налаштувань")]
    public GameObject[] extraToHide;

    [Header("Повзунки")]
    public Slider masterSlider;
    public Slider musicSlider;
    public TMP_Text masterValue;
    public TMP_Text musicValue;

    [Header("Без звуку")]
    public TMP_Text muteLabel;

    [Header("Джерело музики (меню)")]
    public AudioSource musicSource;

    public const string MASTER = "MasterVolume";
    public const string MUSIC = "MusicVolume";
    public const string MUTE = "AudioMuted";

    private float savedMaster = 1f;
    private float savedMusic = 0.6f;
    private bool muted = false;

    void Awake()
    {
        savedMaster = PlayerPrefs.GetFloat(MASTER, 1f);
        savedMusic = PlayerPrefs.GetFloat(MUSIC, 0.6f);
        muted = PlayerPrefs.GetInt(MUTE, 0) == 1;
        ApplyAll();
    }

    void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);

        if (masterSlider != null)
        {
            masterSlider.minValue = 0f; masterSlider.maxValue = 1f;
            masterSlider.SetValueWithoutNotify(savedMaster);
            masterSlider.onValueChanged.AddListener(OnMasterChanged);
        }
        if (musicSlider != null)
        {
            musicSlider.minValue = 0f; musicSlider.maxValue = 1f;
            musicSlider.SetValueWithoutNotify(savedMusic);
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }
        UpdateLabels();
    }

    public void Open()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (mainButtons != null) mainButtons.SetActive(false);
        if (extraToHide != null)
            foreach (var go in extraToHide) if (go != null) go.SetActive(false);
    }

    public void Close()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainButtons != null) mainButtons.SetActive(true);
        if (extraToHide != null)
            foreach (var go in extraToHide) if (go != null) go.SetActive(true);
        PlayerPrefs.Save();
    }

    private void OnMasterChanged(float v)
    {
        savedMaster = v;
        PlayerPrefs.SetFloat(MASTER, v);
        ApplyAll();
        UpdateLabels();
    }

    private void OnMusicChanged(float v)
    {
        savedMusic = v;
        PlayerPrefs.SetFloat(MUSIC, v);
        ApplyAll();
        UpdateLabels();
    }

    public void ToggleMute()
    {
        muted = !muted;
        PlayerPrefs.SetInt(MUTE, muted ? 1 : 0);
        ApplyAll();
        UpdateLabels();
    }

    private void ApplyAll()
    {
        AudioListener.volume = muted ? 0f : savedMaster;
        if (musicSource != null)
            musicSource.volume = (muted ? 0f : 1f) * savedMusic;
    }

    private void UpdateLabels()
    {
        if (masterValue != null) masterValue.text = Mathf.RoundToInt(savedMaster * 100f) + "%";
        if (musicValue != null) musicValue.text = Mathf.RoundToInt(savedMusic * 100f) + "%";
        if (muteLabel != null) muteLabel.text = muted ? "ЗВУК: ВИМКНЕНО" : "ЗВУК: УВІМКНЕНО";
    }
}
