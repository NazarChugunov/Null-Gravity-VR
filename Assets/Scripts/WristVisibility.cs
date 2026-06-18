using UnityEngine;

public class WristVisibility : MonoBehaviour
{
    [Header("ќб'Їкти")]
    public Transform headCamera; // ѕерет€гни сюди Main Camera
    public Transform wristUI;    // ѕерет€гни сюди сам WristComputer
    public GameObject uiContent; // ѕерет€гни пустий об'Їкт, всередин≥ €кого лежить фон ≥ смужки

    [Header("Ќалаштуванн€ чутливост≥")]
    [Range(10f, 90f)]
    public float visibilityAngle = 45f; //  ут, при €кому вмикаЇтьс€ екран

    void Update()
    {
        if (headCamera == null || wristUI == null || uiContent == null) return;

        // ¬ектор в≥д зап'€ст€ до голови гравц€
        Vector3 directionToHead = headCamera.position - wristUI.position;

        // ¬ираховуЇмо кут (€кщо екран дивитьс€ в ≥нший б≥к, зм≥ни -wristUI.forward на wristUI.up)
        float angle = Vector3.Angle(-wristUI.forward, directionToHead);

        // якщо кут менший за 45 градус≥в - екран вмикаЇтьс€
        bool isLookingAtWatch = angle < visibilityAngle;

        if (uiContent.activeSelf != isLookingAtWatch)
        {
            uiContent.SetActive(isLookingAtWatch);
        }
    }
}