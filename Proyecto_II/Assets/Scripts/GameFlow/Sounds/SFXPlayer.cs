using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum de los tipos de efectos de sonido existentes para el Player.
/// </summary>
public enum BrisaSFXType
{
    Idle,
    Walk,
    Run,
    Crouch,
    Sprint,
    PickUp,
    Attack,
    TakeDamage,
    CallBeast,
    Jump,
    DoubleJump,
    Fall,
    Land,
    HardLand,
    HalfDeath,
    Death
}

/*
 * NOMBRE CLASE: SFXPlayer
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 22/05/2025
 * DESCRIPCIÓN: Clase que gestiona la lógica de los efectos de sonido de Player.
 * VERSIÓN: 1.0.
 */

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource brisaAudioSource;

    [Header("Tipos de Audios Brisa")]
    [SerializeField] private AudioClip[] idle;
    [SerializeField] private AudioClip[] walk;
    [SerializeField] private AudioClip[] run;
    [SerializeField] private AudioClip[] crouch;
    [SerializeField] private AudioClip[] sprint;
    [SerializeField] private AudioClip[] pickUp;
    [SerializeField] private AudioClip[] attack;
    [SerializeField] private AudioClip[] takeDamage;
    [SerializeField] private AudioClip[] callBeast;
    [SerializeField] private AudioClip[] jump;
    [SerializeField] private AudioClip[] doubleJump;
    [SerializeField] private AudioClip[] fall;
    [SerializeField] private AudioClip[] land;
    [SerializeField] private AudioClip[] hardLand;
    [SerializeField] private AudioClip[] halfDeath;
    [SerializeField] private AudioClip[] death;

    private Dictionary<BrisaSFXType, AudioClip[]> sfxBrisaClips;

    private void Awake()
    {
        sfxBrisaClips = new Dictionary<BrisaSFXType, AudioClip[]>
        {
            { BrisaSFXType.Idle, idle },
            { BrisaSFXType.Walk, walk },
            { BrisaSFXType.Run, run },
            { BrisaSFXType.Crouch, crouch },
            { BrisaSFXType.Sprint, sprint},
            { BrisaSFXType.PickUp, pickUp},
            { BrisaSFXType.Attack, attack },
            { BrisaSFXType.TakeDamage, takeDamage },
            { BrisaSFXType.CallBeast, callBeast },
            { BrisaSFXType.Jump, jump },
            { BrisaSFXType.DoubleJump, doubleJump },
            { BrisaSFXType.Fall, fall },
            { BrisaSFXType.Land, land },
            { BrisaSFXType.HardLand, hardLand },
            { BrisaSFXType.HalfDeath, halfDeath},
            { BrisaSFXType.Death, death }
        };
    }

    /// <summary>
    /// Método que reproduce aleatoriamente un sonido asociado al tipo específico (recibe como parámetro).
    /// Si el AudioSource ya está reproduciendo algo, no hace nada.
    /// </summary>
    /// <param name="_soundType">Tipo de sonido que se quiere reproducir.</param>
    /// <param name="_volume">Nivel del volumen que se quiere reproducir por defecto (esto luego es modificable al llamar a la función).</param>
    public void PlayRandomSFX(BrisaSFXType _soundType, float _volume = 0.1f)
    {
        if (brisaAudioSource.isPlaying) return;

        if (sfxBrisaClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            brisaAudioSource.clip = selectedClip;
            brisaAudioSource.volume = _volume * AudioSettings.GeneralVolumeMultiplier * AudioSettings.SFXVolumeMultiplier;
            Debug.Log($"Volumen al que se reproduce: {brisaAudioSource.volume}, multiplicador volumen general: {AudioSettings.GeneralVolumeMultiplier}, multipplicador SFX: {AudioSettings.SFXVolumeMultiplier}");
            brisaAudioSource.loop = false;
            brisaAudioSource.Play();
        }
    }

    /// <summary>
    /// Método que detiene el sonido indicado.
    /// </summary>
    /// <param name="_soundType">Tipo de sonido que se quiere detener.</param>
    public void StopSound(BrisaSFXType _soundType)
    {
        if (sfxBrisaClips.TryGetValue(_soundType, out AudioClip[] clips) && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            brisaAudioSource.clip = selectedClip;
            brisaAudioSource.Stop();
        }
    }


    //public void PlayLoopSound(BrisaSFXType _soundType, float _volume = 1.0f)
    //{
    //    if (sfxBrisaClips.TryGetValue(_soundType, out AudioClip[] clips) && clips.Length > 0)
    //    {
    //        int randomIndex = Random.Range(0, clips.Length);
    //        AudioClip selectedClip = clips[randomIndex];

    //        brisaLoopAudioSource.clip = selectedClip;
    //        brisaLoopAudioSource.volume = _volume;
    //        brisaLoopAudioSource.loop = true;
    //        brisaLoopAudioSource.Play();

    //        Debug.Log(selectedClip);
    //    }
    //}
}
