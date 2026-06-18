using UnityEngine;

// Аварійне освітлення: слабке світло, що горить на старті,
// і вимикається після ремонту вказаного модуля (першого — Світла).
// Стежить за публічним полем integrity у RepairableModule, не змінюючи його код.
public class EmergencyLighting : MonoBehaviour
{
    [Tooltip("Модуль, ремонт якого вимикає аварійне світло (модуль Світла)")]
    public RepairableModule triggerModule;

    [Tooltip("Аварійні джерела світла, що згаснуть після ремонту")]
    public Light[] emergencyLights;

    [Tooltip("Поріг цілісності, після якого аварійне світло вимикається")]
    public float repairThreshold = 100f;

    private bool turnedOff = false;

    void Update()
    {
        if (turnedOff || triggerModule == null) return;

        if (triggerModule.integrity >= repairThreshold)
        {
            foreach (var l in emergencyLights)
                if (l != null) l.enabled = false;

            turnedOff = true;
            enabled = false; // більше не потрібен
        }
    }
}
