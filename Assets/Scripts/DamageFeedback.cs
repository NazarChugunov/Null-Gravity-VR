using UnityEngine;
using UnityEngine.XR; // ������������� ������ ������ �� "�����" ������

public class DamageFeedback : MonoBehaviour
{
    [Header("����")]
    public AudioSource audioSource;
    public AudioClip damageSound;

    [Header("³������ (Haptics)")]
    [Range(0f, 1f)] public float amplitude = 1.0f; // ����������� ����
    public float duration = 0.5f; // ϳ��������

    private float lastHealth = 100f;

    // ϳ�����: ��������� 2 ��������� (currentHealth, maxHealth) ��� ��������� �'������� � OnHealthChanged
    public void OnHealthUpdate(float currentHealth, float maxHealth)
    {
        // ����������, �� ������'� ���� ����������
        if (currentHealth < lastHealth)
        {
            TriggerEffects();
        }
        lastHealth = currentHealth;
    }

    private void TriggerEffects()
    {
        // 1. ³��������� ����
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        // 2. ����������� �����: ��������� ������ ������� � ���� � ������ ����������
        VibrateHardware(XRNode.LeftHand);
        VibrateHardware(XRNode.RightHand);
    }

    private void VibrateHardware(XRNode handNode)
    {
        // �������� ������� ������� (���������), ���� ����� ����������
        InputDevice device = InputDevices.GetDeviceAtXRNode(handNode);

        if (device.isValid)
        {
            uint channel = 0; // ����������� ����� �������
            device.SendHapticImpulse(channel, amplitude, duration);
        }
    }
}