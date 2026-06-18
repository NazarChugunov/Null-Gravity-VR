using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ������ ������ ��������� � ����-��� ����� ��� (Singleton)
    public static GameManager Instance;

    // ��������� ��� (���/��������) ��� ������� ����� �������� ������
    public static bool LastResultWin = false;

    [Header("ѳ��� �������� ������")]
    public string endSceneName = "EndScreen_VR";

    [Header("UI ������ (����� � �������)")]
    public GameObject endGameCanvas;
    public TMP_Text statusText;

    [Header("����")]
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip loseSound;

    [Header("������������ ��������")]
    public int totalModulesToRepair = 3;
    private int repairedModulesCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (endGameCanvas != null) endGameCanvas.SetActive(false);
    }

    // ����������� � �������� ��� �������� ������
    public void ModuleRepaired()
    {
        repairedModulesCount++;
        Debug.Log($"³������������ ������: {repairedModulesCount}/{totalModulesToRepair}");

        if (repairedModulesCount >= totalModulesToRepair)
        {
            TriggerWin();
        }
    }

    public void TriggerWin()
    {
        LoadEndScene(true);
    }

    // ����������� � ������� ������'� ������
    public void TriggerGameOver()
    {
        LoadEndScene(false);
    }

    // ����������� ������ �������� ������ �� ������ ����� �����
    private void LoadEndScene(bool win)
    {
        LastResultWin = win;
        Time.timeScale = 1f; // ��������� ��� ����� ������������ �����
        SceneManager.LoadScene(endSceneName);
    }
}