using TMPro;
using UnityEngine;

/* NOMBRE CLASE: AudioSettings
 * AUTOR: Lucía García López
 * FECHA: 23/04/2025
 * DESCRIPCIÓN: Script que gestiona los ajustes de audio del juego. Se puede modificar el volumen general, de música y de efectos de sonido.
 * VERSIÓN: 1.0
 * 1.1 Inicialmente se había pensado en añadir un AudioSource para diálogos, pero se ha decidido no implementarlo por el momento.
 * 1.2 Sonido de 0 a 100, no decimal.
 * 1.3 Funcionamiento con soundPlayerManager.
 */

public class AudioSettings : MonoBehaviour
{
    [Range(0f, 100)] public int generalVolume = 50;
    [Range(0f, 100)] public int musicVolume = 50;
    [Range(0f, 100)] public int sfxVolume = 50;
    // [Range(0f, 1f)] public float dialogueVolume = 1f; // Volumen de diálogos

    // [SerializeField] private AudioSource dialogueSource; // AudioSource para diálogos

    private AudioManager audioManager;
    private SFXPlayer sfxPlayer;
    private SFXBeast sfxBeast;
    private SFXEnemy sfxEnemy;
    private SFXNPCs sfxNPCs;
    private SFXSheep sfxSheep;

    [SerializeField] private TextMeshProUGUI generalVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    // Multiplicadores que se aplicarán a los volúmenes base
    public static float GeneralVolumeMultiplier { get; private set; } = 0.5f;
    public static float MusicVolumeMultiplier { get; private set; } = 0.5f;
    public static float SFXVolumeMultiplier { get; private set; } = 0.5f;

    #region Singleton
    public static AudioSettings Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        sfxPlayer = FindObjectOfType<SFXPlayer>();
        sfxBeast = FindObjectOfType<SFXBeast>();
        sfxEnemy = FindObjectOfType<SFXEnemy>();
        sfxNPCs = FindObjectOfType<SFXNPCs>();
        sfxSheep = FindObjectOfType<SFXSheep>();
        
        // if (dialogueSource == null)
        //     Debug.LogWarning("No se asignó AudioSource para diálogos.");

        SetGeneralText(generalVolume);
        SetMusicText(musicVolume);
        SetSFXText(sfxVolume);

        UpdateAllVolumes();
    }

    public void SetGeneralVolumeFromSlider(float value)
    {
        generalVolume = Mathf.RoundToInt(value);
        GeneralVolumeMultiplier = generalVolume / 100f;
        SetGeneralText(generalVolume);
        UpdateAllVolumes();
    }

    public void SetMusicVolumeFromSlider(float value)
    {
        musicVolume = Mathf.RoundToInt(value);
        MusicVolumeMultiplier = musicVolume / 100f;
        SetMusicText(musicVolume);
        UpdateAllVolumes();
    }

    public void SetSFXVolumeFromSlider(float value)
    {
        sfxVolume = Mathf.RoundToInt(value);
        SFXVolumeMultiplier = sfxVolume / 100f;
        SetSFXText(sfxVolume);
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

        // Actualizar música (solo afectada por volumen general y música)
        if (audioManager != null && audioManager.music != null)
        {
            audioManager.music.volume = GeneralVolumeMultiplier * MusicVolumeMultiplier;
        }

        // Actualizar sonidos ambientales (tratados como SFX)
        if (audioManager != null)
        {
            if (audioManager.environmentSounds != null)
            {
                audioManager.environmentSounds.volume = GeneralVolumeMultiplier * SFXVolumeMultiplier;
            }

            if (audioManager.smallSounds != null)
            {
                audioManager.smallSounds.volume = GeneralVolumeMultiplier * SFXVolumeMultiplier;
            }
        }
        // if (dialogueSource != null)
        //     dialogueSource.volume = Mathf.Pow(generalVolume * dialogueVolume, 1.5f);
    }

    // public void PlayDialogue(AudioClip clip)
    // {
    //     if (dialogueSource == null || clip == null) return;
    //
    //     dialogueSource.clip = clip;
    //     dialogueSource.loop = false;
    //     dialogueSource.Play();
    // }
    //
    // public void StopDialogue()
    // {
    //     if (dialogueSource != null)
    //         dialogueSource.Stop();
    // }

    public void SetGeneralText(float value)
    {
        if (generalVolumeText != null)
            generalVolumeText.text = value.ToString("0");
    }

    public void SetMusicText(float value)
    {
        if (musicVolumeText != null)
            musicVolumeText.text = value.ToString("0");
    }

    void SetSFXText(float value)
    {
        if (sfxVolumeText != null)
            sfxVolumeText.text = value.ToString("0");
    }

    //void SetDialogueText(float value)
    //{
    //    // if (dialogueVolumeText != null)
    //    //     dialogueVolumeText.text = value.ToString("0");
    //}
}