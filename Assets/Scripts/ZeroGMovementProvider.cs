using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ZeroGMovementProvider : MonoBehaviour
{
    [Header("������������ �����")]
    public float thrustForce = 5f;

    [Header("��������� (������)")]
    public InputActionProperty thrustInput;

    [Header("������ �������")]
    public Transform controllerTransform;

    [Header("������ ��������� ����� (��'��/�����)")]
    [Tooltip("AudioSource (loop) ����� ��������� ������")]
    public AudioSource thrustAudio;
    [Tooltip("������-������� ������ ������ �������")]
    public ParticleSystem thrustEffect;

    private Rigidbody rb;
    private PushPullLocomotion pushPull;
    private PlayerResources playerResources; // ������ ��������� �� ������� �������
    private bool isThrusting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pushPull = GetComponent<PushPullLocomotion>();
        playerResources = GetComponent<PlayerResources>(); // ��������� ������ ������'�/������ �� ����� � ��'���

        rb.useGravity = false;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

        if (thrustInput.action != null) thrustInput.action.Enable();
    }

    void FixedUpdate()
    {
        ApplyThrust();
    }

    private void ApplyThrust()
    {
        bool thrustingNow = false;

        // ���� ������ ��� �������� � ���� � ���� ���� ��������� �� ���� � ������� �������
        if (pushPull == null || !pushPull.IsGrabbing)
        {
            float thrustValue = thrustInput.action != null ? thrustInput.action.ReadValue<float>() : 0f;

            // ���� ������� ������� ������ �������
            if (thrustValue > 0.1f)
            {
                // Гравець тисне тягу — тримаємо таймер відновлення скинутим (навіть якщо пальне скінчилось)
                if (playerResources != null) playerResources.NotifyThrustUsed();

                // ���������� �� PlayerResources. ���� ����� TryConsumeFuel() ������� true (������ �) - ������!
                if (playerResources != null && playerResources.TryConsumeFuel())
                {
                    Vector3 thrustVector = controllerTransform.forward * thrustValue * thrustForce;
                    rb.AddForce(thrustVector, ForceMode.Acceleration);
                    thrustingNow = true;
                }
            }
        }

        UpdateThrustEffects(thrustingNow);
    }

    // ��������/��������� ��'�� �� ������ ��� ��� ��������� ������
    private void UpdateThrustEffects(bool on)
    {
        if (on == isThrusting) return;
        isThrusting = on;

        if (on)
        {
            if (thrustAudio != null && !thrustAudio.isPlaying) thrustAudio.Play();
            if (thrustEffect != null && !thrustEffect.isEmitting) thrustEffect.Play();
        }
        else
        {
            if (thrustAudio != null && thrustAudio.isPlaying) thrustAudio.Stop();
            if (thrustEffect != null && thrustEffect.isEmitting)
                thrustEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}