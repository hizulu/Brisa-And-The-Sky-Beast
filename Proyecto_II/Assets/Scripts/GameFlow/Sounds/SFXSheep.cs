using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum de los tipos de efectos de sonido existentes para las ovejas.
/// </summary>
public enum SheepSFXType
{
    Idle,
    Jump,
}

/*
 * NOMBRE CLASE: SFXSheep
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 22/05/2025
 * DESCRIPCIÓN: Clase que gestiona la lógica de los efectos de sonido de las ovejas.
 * VERSIÓN: 1.0.
 */

public class SFXSheep : MonoBehaviour
{
    [SerializeField] private AudioSource sheepAudioSource;

    [Header("Tipos de Audios Ovejas")]
    [SerializeField] private AudioClip[] idle;
    [SerializeField] private AudioClip[] jump;

    private Dictionary<SheepSFXType, AudioClip[]> sfxNPCClips;

    private void Awake()
    {
        sfxNPCClips = new Dictionary<SheepSFXType, AudioClip[]>
        {
            { SheepSFXType.Idle, idle },
            { SheepSFXType.Jump, jump }
        };
    }

    /// <summary>
    /// Método que reproduce aleatoriamente un sonido asociado al tipo específico (recibe como parámetro).
    /// Si el AudioSource ya está reproduciendo algo, no hace nada.
    /// </summary>
    /// <param name="_soundType">Tipo de sonido que se quiere reproducir.</param>
    /// <param name="_volume">Nivel del volumen que se quiere reproducir por defecto (esto luego es modificable al llamar a la función).</param>
    public void PlayRandomSFX(SheepSFXType _soundType, float _volume = 0.3f)
    {
        if (sheepAudioSource.isPlaying) return;

        if (sfxNPCClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            sheepAudioSource.clip = selectedClip;
            sheepAudioSource.volume = _volume * AudioSettings.GeneralVolumeMultiplier * AudioSettings.SFXVolumeMultiplier; ;
            sheepAudioSource.loop = false;
            sheepAudioSource.Play();
        }
    }

    /// <summary>
    /// Método que detiene el sonido indicado.
    /// </summary>
    /// <param name="_soundType">Tipo de sonido que se quiere detener.</param>
    public void StopSound(SheepSFXType _soundType)
    {
        if (sfxNPCClips.TryGetValue(_soundType, out AudioClip[] clips) && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            sheepAudioSource.clip = selectedClip;
            sheepAudioSource.Stop();
        }
    }
}
