using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    [Range(0f, 1f)] public float generalVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    // [Range(0f, 1f)] public float dialogueVolume = 1f; // Volumen de diálogos

    // [SerializeField] private AudioSource dialogueSource; // AudioSource para diálogos

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;

        // if (dialogueSource == null)
        //     Debug.LogWarning("No se asignó AudioSource para diálogos.");

        UpdateAllVolumes();
    }

    public void SetGeneralVolumeFromSlider(float value)
    {
        generalVolume = value;
        UpdateAllVolumes();
    }

    public void SetMusicVolumeFromSlider(float value)
    {
        musicVolume = value;
        UpdateAllVolumes();
    }

    public void SetSFXVolumeFromSlider(float value)
    {
        sfxVolume = value;
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
        //     dialogueSource.volume = generalVolume * dialogueVolume;
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
}
