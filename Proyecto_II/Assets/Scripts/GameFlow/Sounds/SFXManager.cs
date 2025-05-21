using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource brisaAudioSource;
    [SerializeField] private AudioSource brisaLoopAudioSource;

    [Header("Tipos de Audios Brisa")]
    [SerializeField] private AudioClip[] idle;
    [SerializeField] private AudioClip[] walk;
    [SerializeField] private AudioClip[] run;
    [SerializeField] private AudioClip[] crouch;
    [SerializeField] private AudioClip[] attack;
    [SerializeField] private AudioClip[] takeDamage;
    [SerializeField] private AudioClip[] callBeast;
    [SerializeField] private AudioClip[] jump;
    [SerializeField] private AudioClip[] doubleJump;
    [SerializeField] private AudioClip[] fall;
    [SerializeField] private AudioClip[] land;
    [SerializeField] private AudioClip[] hardLand;

    private Dictionary<BrisaSFXType, AudioClip[]> sfxBrisaClips;

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //    Destroy(gameObject);

        sfxBrisaClips = new Dictionary<BrisaSFXType, AudioClip[]>
        {
            { BrisaSFXType.Idle, idle },
            { BrisaSFXType.Walk, walk },
            { BrisaSFXType.Run, run },
            { BrisaSFXType.Crouch, crouch },
            { BrisaSFXType.Attack, attack },
            { BrisaSFXType.TakeDamage, takeDamage },
            { BrisaSFXType.CallBeast, callBeast },
            { BrisaSFXType.Jump, jump },
            { BrisaSFXType.DoubleJump, doubleJump },
            { BrisaSFXType.Fall, fall },
            { BrisaSFXType.Land, land },
            { BrisaSFXType.HardLand, hardLand }
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
}

public enum BrisaSFXType
{
    Idle,
    Walk,
    Run,
    Crouch,
    Attack,
    TakeDamage,
    CallBeast,
    Jump,
    DoubleJump,
    Fall,
    Land,
    HardLand,
}
