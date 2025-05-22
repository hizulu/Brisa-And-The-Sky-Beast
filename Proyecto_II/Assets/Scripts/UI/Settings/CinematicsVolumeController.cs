using UnityEngine;
using UnityEngine.Video;

public class CinematicsVolumeController : MonoBehaviour
{
    public VideoPlayer cinematicVideoPlayer;
    public AudioSource audioSource;

    #region Singleton
    public static CinematicsVolumeController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Configuraci�n inicial del VideoPlayer
            if (cinematicVideoPlayer != null)
            {
                cinematicVideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
                cinematicVideoPlayer.SetTargetAudioSource(0, audioSource);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    void Update()
    {
        if (audioSource != null)
        {
            audioSource.volume = AudioSettings.GeneralVolumeMultiplier *
                                AudioSettings.CinematicVolumeMultiplier;
        }
    }

    // M�todo para reproducir el sonido de la cinem�tica
    public void PlayCinematicAudio()
    {
        audioSource.Play();
    }

    // M�todo para detener el sonido de la cinem�tica
    public void StopCinematicAudio()
    {
        audioSource.Stop();
    }
}