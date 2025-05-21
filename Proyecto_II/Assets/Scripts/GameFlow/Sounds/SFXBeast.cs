using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeastSFXType
{
    Howl,
    Attack,
    Walk,
    Run,
    Idle
}

public class SFXBeast : MonoBehaviour
{
    [SerializeField] private AudioSource beastAudioSource;

    [Header("Tipos de Audios Bestia")]
    [SerializeField] private AudioClip[] howlClips;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] walkClips;
    [SerializeField] private AudioClip[] runClips;
    [SerializeField] private AudioClip[] idleClips;

    private Dictionary<BeastSFXType, AudioClip[]> sfxBeastClips;

    private void Awake()
    {
        sfxBeastClips = new Dictionary<BeastSFXType, AudioClip[]>
        {
            { BeastSFXType.Howl, howlClips },
            { BeastSFXType.Attack, attackClips },
            { BeastSFXType.Walk, walkClips },
            { BeastSFXType.Run, runClips },
            { BeastSFXType.Idle, idleClips }
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

    public void PlayLoopSound(BeastSFXType _soundType, float _volume = 1.0f)
    {
        if (sfxBeastClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            beastAudioSource.clip = selectedClip;
            beastAudioSource.volume = _volume;
            beastAudioSource.loop = true;
            beastAudioSource.Play();
        }
    }
}
