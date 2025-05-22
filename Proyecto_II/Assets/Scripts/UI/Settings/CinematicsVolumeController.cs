using UnityEngine;
using UnityEngine.Video;


/*NOMBRE CLASE: CinematicsVolumeController
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 22/05/2025
 * DESCRIPCI�N: Script que gestiona el volumen de las cinem�ticas.
 * VERSI�N: 1.0 El volumen de las cinem�ticas se ajusta en funci�n del volumen general y el volumen de las cinem�ticas.
 */

public class CinematicsVolumeController : MonoBehaviour
{
    public VideoPlayer cinematicVideoPlayer;
    public AudioSource audioSource;

    private void Awake()
    {
        // Configuraci�n inicial del VideoPlayer
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