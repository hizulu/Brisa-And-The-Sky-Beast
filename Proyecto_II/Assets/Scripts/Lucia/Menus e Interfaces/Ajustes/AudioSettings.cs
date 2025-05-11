using TMPro;
using UnityEngine;

/* NOMBRE CLASE: AudioSettings
 * AUTOR: Lucía García López
 * FECHA: 23/04/2025
 * DESCRIPCIÓN: Script que gestiona los ajustes de audio del juego. Se puede modificar el volumen general, de música y de efectos de sonido.
 * VERSIÓN: 1.0
 * 1.1 Inicialmente se había pensado en añadir un AudioSource para diálogos, pero se ha decidido no implementarlo por el momento.
 */

public class AudioSettings : MonoBehaviour
{
    [Range(0f, 1f)] public float generalVolume = 1f;
    [Range(0f, 1)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    // [Range(0f, 1f)] public float dialogueVolume = 1f; // Volumen de diálogos

    // [SerializeField] private AudioSource dialogueSource; // AudioSource para diálogos

    private AudioManager audioManager;

    [SerializeField] private TextMeshProUGUI generalVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    private void Start()
    {
        audioManager = AudioManager.Instance;

        // if (dialogueSource == null)
        //     Debug.LogWarning("No se asignó AudioSource para diálogos.");

        SetGeneralText(generalVolume);
        SetMusicText(musicVolume);
        SetSFXText(sfxVolume);

        UpdateAllVolumes();
    }

    public void SetGeneralVolumeFromSlider(float value)
    {
        generalVolume = value;
        SetGeneralText(value);
        UpdateAllVolumes();
    }

    public void SetMusicVolumeFromSlider(float value)
    {
        musicVolume = value;
        SetMusicText(value);
        UpdateAllVolumes();
    }

    public void SetSFXVolumeFromSlider(float value)
    {
        sfxVolume = value;
        SetSFXText(value);
        UpdateAllVolumes();
    }

    // public void SetDialogueVolumeFromSlider(float value)
    // {
    //     dialogueVolume = value;
    //     UpdateAllVolumes();
    // }

    private void UpdateAllVolumes()
    {
        if (audioManager == null) return;

        audioManager.musicSource.volume = generalVolume * musicVolume;
        audioManager.SFXSource.volume = generalVolume * sfxVolume;

        // if (dialogueSource != null)
        //     dialogueSource.volume = Mathf.Pow(generalVolume * dialogueVolume, 1.5f);
    }

    // public void PlayDialogue(AudioClip clip)
    // {
    //     if (dialogueSource == null || clip == null) return;

    //     dialogueSource.clip = clip;
    //     dialogueSource.loop = false;
    //     dialogueSource.Play();
    // }

    // public void StopDialogue()
    // {
    //     if (dialogueSource != null)
    //         dialogueSource.Stop();
    // }

    public void SetGeneralText(float value)
    {
        if (generalVolumeText != null)
            generalVolumeText.text = value.ToString("0.0");
    }

    public void SetMusicText(float value)
    {
        if (musicVolumeText != null)
            musicVolumeText.text = value.ToString("0.0");
    }

    void SetSFXText(float value)
    {
        if (sfxVolumeText != null)
            sfxVolumeText.text = value.ToString("0.0");
    }

    void SetDialogueText(float value)
    {
        // if (dialogueVolumeText != null)
        //     dialogueVolumeText.text = value.ToString("0.0");
    }
}
