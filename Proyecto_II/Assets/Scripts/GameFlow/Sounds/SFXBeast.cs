using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum de los tipos de efectos de sonido existentes para la Bestia.
/// </summary>
public enum BeastSFXType
{
    Idle,
    Walk,
    Run,
    AttackClaw,
    AttackBite,
    Howl,
    Hurt,
    Purr,
    Dead,
    Halfdead,
    Smell,
}

/*
 * NOMBRE CLASE: SFXBeast
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 22/05/2025
 * DESCRIPCIÓN: Clase que gestiona la lógica de los efectos de sonido de la Bestia.
 * VERSIÓN: 1.0.
 */

public class SFXBeast : MonoBehaviour
{
    [SerializeField] private AudioSource beastAudioSource;

    [Header("Tipos de Audios Bestia")]
    [SerializeField] private AudioClip[] idle;
    [SerializeField] private AudioClip[] walk;
    [SerializeField] private AudioClip[] run;
    [SerializeField] private AudioClip[] attackClaw;
    [SerializeField] private AudioClip[] attackBite;
    [SerializeField] private AudioClip[] howl;
    [SerializeField] private AudioClip[] hurt;
    [SerializeField] private AudioClip[] purr;
    [SerializeField] private AudioClip[] dead;
    [SerializeField] private AudioClip[] halfDead;
    [SerializeField] private AudioClip[] smell;

    private Dictionary<BeastSFXType, AudioClip[]> sfxBeastClips;

    private void Awake()
    {
        sfxBeastClips = new Dictionary<BeastSFXType, AudioClip[]>
        {
            { BeastSFXType.Idle, idle },
            { BeastSFXType.Walk, walk },
            { BeastSFXType.Run, run },
            { BeastSFXType.AttackClaw, attackClaw},
            { BeastSFXType.AttackBite, attackBite},
            { BeastSFXType.Howl, howl },
            { BeastSFXType.Hurt, hurt },
            { BeastSFXType.Purr, purr},
            { BeastSFXType.Dead, dead},
            { BeastSFXType.Halfdead, halfDead},
            { BeastSFXType.Smell, smell},
        };
    }

    /// <summary>
    /// Método que reproduce aleatoriamente un sonido asociado al tipo específico (recibe como parámetro).
    /// Si el AudioSource ya está reproduciendo algo, no hace nada.
    /// </summary>
    /// <param name="_soundType">Tipo de sonido que se quiere reproducir.</param>
    /// <param name="_volume">Nivel del volumen que se quiere reproducir por defecto (esto luego es modificable al llamar a la función).</param>
    public void PlayRandomSFX(BeastSFXType _soundType, float _volume = 1f)
    {
        if (sfxBeastClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];
            beastAudioSource.PlayOneShot(selectedClip, _volume * AudioSettings.GeneralVolumeMultiplier * AudioSettings.SFXVolumeMultiplier);
        }
    }

    /// <summary>
    /// Método que detiene el sonido indicado.
    /// </summary>
    /// <param name="_soundType">Tipo de sonido que se quiere detener.</param>
    public void StopSound(BeastSFXType _soundType)
    {
        if (sfxBeastClips.TryGetValue(_soundType, out AudioClip[] clips) && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            beastAudioSource.clip = selectedClip;
            beastAudioSource.Stop();
        }
    }

    //public void PlayLoopSound(BeastSFXType _soundType, float _volume = 1.0f)
    //{
    //    if (sfxBeastClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
    //    {
    //        int randomIndex = Random.Range(0, clips.Length);
    //        AudioClip selectedClip = clips[randomIndex];

    //        beastAudioSource.clip = selectedClip;
    //        beastAudioSource.volume = _volume;
    //        beastAudioSource.loop = true;
    //        beastAudioSource.Play();
    //    }
    //}
}
