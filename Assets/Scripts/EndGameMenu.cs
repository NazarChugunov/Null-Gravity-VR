using UnityEngine;
using UnityEngine.SceneManagement;

// Невеликий допоміжний скрипт для кнопок екрану завершення гри.
// Скидає Time.timeScale (бо GameManager ставить його в 0) і завантажує сцену.
public class EndGameMenu : MonoBehaviour
{
    [Tooltip("Назва сцени головного меню у Build Settings")]
    public string mainMenuSceneName = "MainMenu_VR";

    [Tooltip("Відстань перед гравцем, на якій з'являється екран (метри)")]
    public float distanceInFront = 2.5f;

    // Коли канвас активується (GameManager робить SetActive(true)) —
    // ставимо його перед камерою гравця, бо подія може статись будь-де на станції.
    private void OnEnable()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Беремо лише напрямок погляду по горизонталі, щоб екран не нахилявся
        Vector3 flatForward = cam.transform.forward;
        flatForward.y = 0f;
        if (flatForward.sqrMagnitude < 0.001f) flatForward = Vector3.forward;
        flatForward.Normalize();

        transform.position = cam.transform.position + flatForward * distanceInFront;
        // Передня сторона канваса дивиться на гравця
        transform.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
    }

    // Перезапустити поточну ігрову сцену
    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    // Повернутися в головне меню
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
