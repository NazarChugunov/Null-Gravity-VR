using UnityEngine;

public class PlayerBelt : MonoBehaviour
{
    [Header("Прив'язка до гравця")]
    public Transform headCamera; // Перетягни сюди Main Camera (очі гравця)

    [Header("Налаштування висоти")]
    [Tooltip("На скільки метрів вниз від очей знаходиться пояс")]
    public float heightOffset = 0.55f;

    void LateUpdate()
    {
        if (headCamera == null) return;

        // 1. Позиція: пояс завжди під головою по осях X та Z
        transform.position = new Vector3(headCamera.position.x, headCamera.position.y - heightOffset, headCamera.position.z);

        // 2. Обертання: беремо кут повороту голови, але обнуляємо нахили (осі X та Z)
        Vector3 targetEulerAngles = headCamera.eulerAngles;
        targetEulerAngles.x = 0; // Щоб пояс не задирався, коли дивишся вгору
        targetEulerAngles.z = 0; // Щоб пояс не хилився при нахилі голови

        transform.eulerAngles = targetEulerAngles;
    }
}