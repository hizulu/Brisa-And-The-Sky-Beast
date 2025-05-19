using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Walk,
    Run,
    Crouch,
    Attack,
    Jump,
    Fall,
    Land,
    Hurt,
    Whistle,
    Howl,
    BeastAttack
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundPlayerManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundPlayerManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for(int i = 0; i < soundList.Length; i++)
            soundList[i].name = names[i];
    }
#endif
}

[System.Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
