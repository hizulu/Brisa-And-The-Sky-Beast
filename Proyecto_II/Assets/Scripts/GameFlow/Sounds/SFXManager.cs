using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
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

    public void PlayRandomSFX(BrisaSFXType _soundType, float _volume = 1.0f)
    {
        if (brisaAudioSource.isPlaying) return;

        if (sfxBrisaClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            brisaAudioSource.clip = selectedClip;
            brisaAudioSource.volume = _volume;
            brisaAudioSource.loop = false;
            brisaAudioSource.Play();
        }
    }

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
