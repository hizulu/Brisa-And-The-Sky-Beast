using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum de los tipos de efectos de sonido existentes para las ovejas.
/// </summary>
public enum SheepSFXType
{
    Idle,
    Walk,
    Graze,
    Jump,
    Bee
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
    [SerializeField] private AudioClip[] walk;
    [SerializeField] private AudioClip[] graze;
    [SerializeField] private AudioClip[] jump;
    [SerializeField] private AudioClip[] bee;

    private Dictionary<SheepSFXType, AudioClip[]> sfxNPCClips;

    private void Awake()
    {
        sfxNPCClips = new Dictionary<SheepSFXType, AudioClip[]>
        {
            { SheepSFXType.Idle, idle },
            { SheepSFXType.Walk, walk },
            { SheepSFXType.Graze, graze },
            { SheepSFXType.Jump, jump },
            { SheepSFXType.Bee, bee },
        };
    }

    public void PlayRandomSFX(SheepSFXType _soundType, float _volume = 0.3f)
    {
        if (sheepAudioSource.isPlaying) return;

        if (sfxNPCClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            sheepAudioSource.clip = selectedClip;
            sheepAudioSource.volume = _volume;
            sheepAudioSource.loop = false;
            sheepAudioSource.Play();
        }
    }

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
