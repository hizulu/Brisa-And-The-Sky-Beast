using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum de los tipos de efectos de sonido existentes para los NPCs.
/// </summary>
public enum NPCSFXType
{
    Idle,
    Talk
}

/*
 * NOMBRE CLASE: SFXNPCs
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 22/05/2025
 * DESCRIPCIÓN: Clase que gestiona la lógica de los efectos de sonido de los NPCs.
 * VERSIÓN: 1.0.
 */

public class SFXNPCs : MonoBehaviour
{
    [SerializeField] private AudioSource npcAudioSource;

    [Header("Tipos de Audios NPCs")]
    [SerializeField] private AudioClip[] idle;
    [SerializeField] private AudioClip[] talk;

    private Dictionary<NPCSFXType, AudioClip[]> sfxNPCClips;

    private void Awake()
    {
        sfxNPCClips = new Dictionary<NPCSFXType, AudioClip[]>
        {
            { NPCSFXType.Idle, idle },
            { NPCSFXType.Talk, talk },
        };
    }

    public void PlayRandomSFX(NPCSFXType _soundType, float _volume = 0.5f)
    {
        if (npcAudioSource.isPlaying) return;

        if (sfxNPCClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            npcAudioSource.clip = selectedClip;
            npcAudioSource.volume = _volume;
            npcAudioSource.loop = false;
            npcAudioSource.Play();
        }
    }

    public void StopSound(NPCSFXType _soundType)
    {
        if (sfxNPCClips.TryGetValue(_soundType, out AudioClip[] clips) && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            npcAudioSource.clip = selectedClip;
            npcAudioSource.Stop();
        }
    }
}
