using UnityEngine;
using UnityEngine.Video;


/*NOMBRE CLASE: CinematicsVolumeController
 * AUTOR: Lucía García López
 * FECHA: 22/05/2025
 * DESCRIPCIÓN: Script que gestiona el volumen de las cinemáticas.
 * VERSIÓN: 1.0 El volumen de las cinemáticas se ajusta en función del volumen general y el volumen de las cinemáticas.
 */

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