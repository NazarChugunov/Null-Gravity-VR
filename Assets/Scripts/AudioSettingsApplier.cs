using UnityEngine;

// Застосовує збережену гучність на старті будь-якої сцени (щоб гра поважала налаштування з меню).
public class AudioSettingsApplier : MonoBehaviour
{
    void Awake()
    {
        bool muted = PlayerPrefs.GetInt("AudioMuted", 0) == 1;
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = muted ? 0f : master;
    }
}
