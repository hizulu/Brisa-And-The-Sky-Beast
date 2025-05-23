using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum de los tipos de efectos de sonido existentes para los NPCs.
/// </summary>
public enum NPCSFXType
{
    Idle,
    Talk
}

/*
 * NOMBRE CLASE: SFXNPCs
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 22/05/2025
 * DESCRIPCI�N: Clase que gestiona la l�gica de los efectos de sonido de los NPCs.
 * VERSI�N: 1.0.
 */

public class SFXNPCs : MonoBehaviour
{
    [SerializeField] private AudioSource npcAudioSource;

    [Header("Tipos de Audios NPCs")]
    [SerializeField] private AudioClip[] idle;
    [SerializeField] private AudioClip[] talk;

    private Dictionary<NPCSFXType, AudioClip[]> sfxNPCClips;

    private void Awake()
    {
        sfxNPCClips = new Dictionary<NPCSFXType, AudioClip[]>
        {
            { NPCSFXType.Idle, idle },
            { NPCSFXType.Talk, talk },
        };
    }

    /// <summary>
    /// M�todo que reproduce aleatoriamente un sonido asociado al tipo espec�fico (recibe como par�metro).
    /// Si el AudioSource ya est� reproduciendo algo, no hace nada.
    /// </summary>
    /// <param name="_soundType">Tipo de sonido que se quiere reproducir.</param>
    /// <param name="_volume">Nivel del volumen que se quiere reproducir por defecto (esto luego es modificable al llamar a la funci�n).</param>
    public void PlayRandomSFX(NPCSFXType _soundType, float _volume = 0.5f)
    {
        if (npcAudioSource.isPlaying) return;

        if (sfxNPCClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            npcAudioSource.clip = selectedClip;
            npcAudioSource.volume = _volume * AudioSettings.GeneralVolumeMultiplier * AudioSettings.SFXVolumeMultiplier; ;
            npcAudioSource.loop = false;
            npcAudioSource.Play();
        }
    }

    /// <summary>
    /// M�todo que detiene el sonido indicado.
    /// </summary>
    /// <param name="_soundType">Tipo de sonido que se quiere detener.</param>
    public void StopSound(NPCSFXType _soundType)
    {
        if (sfxNPCClips.TryGetValue(_soundType, out AudioClip[] clips) && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            npcAudioSource.clip = selectedClip;
            npcAudioSource.Stop();
        }
    }
}
