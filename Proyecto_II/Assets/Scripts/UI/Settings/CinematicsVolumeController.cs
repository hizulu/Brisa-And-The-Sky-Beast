using UnityEngine;
using UnityEngine.Video;

public class CinematicsVolumeController : MonoBehaviour
{
    public VideoPlayer cinematicVideoPlayer;
    public AudioSource audioSource;

    private void Awake()
    {
        // Configuración inicial del VideoPlayer
        if (cinematicVideoPlayer != null)
        {
            cinematicVideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            cinematicVideoPlayer.SetTargetAudioSource(0, audioSource);
        }

    }

    void Update()
    {
        if (audioSource != null)
        {
            audioSource.volume = AudioSettings.GeneralVolumeMultiplier *
                                AudioSettings.CinematicVolumeMultiplier;
        }
    }
}