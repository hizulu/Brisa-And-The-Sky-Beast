using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeastSFXType
{
    Idle,
    Walk,
    Run,
    AttackClaw,
    AttackBite,
    Howl,
    Bark,
    Growl,
    Eat,
    Smell,
}

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
    [SerializeField] private AudioClip[] bark;
    [SerializeField] private AudioClip[] growl;
    [SerializeField] private AudioClip[] eat;
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
            { BeastSFXType.Bark, bark },
            { BeastSFXType.Growl, growl},
            { BeastSFXType.Eat, eat },
            { BeastSFXType.Smell, smell},
        };
    }

    public void PlayRandomSFX(BeastSFXType _soundType, float _volume = 1.0f)
    {
        if (sfxBeastClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];
            beastAudioSource.PlayOneShot(selectedClip, _volume);
        }
    }

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
