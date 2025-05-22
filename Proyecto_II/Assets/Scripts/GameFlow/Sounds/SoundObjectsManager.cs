using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum SoundType
{
    BreakBox,
    Chain,
    Lever,
    Seesaw,
}

public class SoundObjectsManager : MonoBehaviour
{
    public static SoundObjectsManager Instance;

    [SerializeField] private AudioSource objectAudioSource;

    [Header("Tipos de Audios")]
    [SerializeField] private AudioClip breakBox;
    [SerializeField] private AudioClip chain;
    [SerializeField] private AudioClip lever;
    [SerializeField] private AudioClip seesaw;

    private Dictionary<SoundType, AudioClip> sfxObjectClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        sfxObjectClip = new Dictionary<SoundType, AudioClip>
        {
            { SoundType.BreakBox, breakBox },
            { SoundType.Chain, chain },
            { SoundType.Lever, lever },
            { SoundType.Seesaw, seesaw }
        };
    }

    public void PlaySFX(SoundType _soundType, float _volume = 0.2f)
    {
        if (objectAudioSource.isPlaying) return;

        if (sfxObjectClip.TryGetValue(_soundType, out AudioClip clip) && clip != null)
        {
            objectAudioSource.clip = clip;
            objectAudioSource.volume = _volume;
            objectAudioSource.loop = false;
            objectAudioSource.Play();
        }
    }

    public void StopSFX(SoundType _soundType)
    {
        objectAudioSource.Stop();
    }
}
