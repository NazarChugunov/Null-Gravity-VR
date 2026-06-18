using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Контролер окремої сцени фінального екрану.
// Читає результат із GameManager.LastResultWin і показує відповідний текст.
// Кнопки викликають RestartGame / ReturnToMenu.
public class EndScreenController : MonoBehaviour
{
    [Header("Текст результату")]
    public TMP_Text titleText;

    [Header("Кнопки")]
    [Tooltip("Кнопка 'Заново' — ховається на екрані перемоги")]
    public GameObject restartButton;
    [Tooltip("RectTransform кнопки 'Головне меню' — центрується на екрані перемоги")]
    public RectTransform menuButtonRect;
    [Tooltip("Позиція Y кнопки меню, коли вона лишається єдиною (перемога)")]
    public float menuSoloY = -90f;
    [Tooltip("Позиція Y кнопки меню, коли показані обидві кнопки (поразка)")]
    public float menuPairedY = -270f;

    [Header("Назви сцен")]
    public string gameSceneName = "TestGround";
    public string mainMenuSceneName = "MainMenu_VR";

    [Header("Кольори")]
    public Color winColor = new Color(0.3f, 1f, 0.4f);
    public Color loseColor = new Color(1f, 0.3f, 0.25f);

    [Header("Звук")]
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip loseSound;

    [Header("Тексти (UK)")]
    [TextArea] public string winText = "МІСІЮ ВИКОНАНО\nСИСТЕМИ СТАНЦІЇ ВІДНОВЛЕНО";
    [TextArea] public string loseText = "ВАС ЗНИЩЕНО\nСТАНЦІЮ НЕ ВРЯТОВАНО";

    void Start()
    {
        Time.timeScale = 1f; // підстраховка

        if (titleText != null)
        {
            if (GameManager.LastResultWin)
            {
                titleText.text = winText;
                titleText.color = winColor;
            }
            else
            {
                titleText.text = loseText;
                titleText.color = loseColor;
            }
        }

        // Звук результату
        if (audioSource != null)
        {
            AudioClip clip = GameManager.LastResultWin ? winSound : loseSound;
            if (clip != null) audioSource.PlayOneShot(clip);
        }

        // На екрані перемоги ховаємо кнопку "Заново" — лишається тільки "Головне меню".
        // На екрані поразки кнопка "Заново" доступна.
        bool win = GameManager.LastResultWin;
        if (restartButton != null)
            restartButton.SetActive(!win);

        // Центруємо кнопку меню, коли вона єдина (перемога)
        if (menuButtonRect != null)
        {
            Vector2 p = menuButtonRect.anchoredPosition;
            p.y = win ? menuSoloY : menuPairedY;
            menuButtonRect.anchoredPosition = p;
        }
    }

    // Кнопка "Заново" — перезапуск ігрової сцени
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    // Кнопка "Головне меню"
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
