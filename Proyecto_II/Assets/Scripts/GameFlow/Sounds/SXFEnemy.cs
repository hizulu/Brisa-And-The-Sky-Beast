using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SXFEnemy : MonoBehaviour
{
    [SerializeField] private AudioSource enemyAudioSource;

    [Header("Tipos de Audios Brisa")]
    [SerializeField] private AudioClip[] idle;
    [SerializeField] private AudioClip[] walk;
    [SerializeField] private AudioClip[] detectTarget;
    [SerializeField] private AudioClip[] takeDamage;
    [SerializeField] private AudioClip[] retreat;
    [SerializeField] private AudioClip[] jump;
    [SerializeField] private AudioClip[] attack;
    [SerializeField] private AudioClip[] death;

    private Dictionary<EnemySFXType, AudioClip[]> sfxEnemyClips;

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //    Destroy(gameObject);

        sfxEnemyClips = new Dictionary<EnemySFXType, AudioClip[]>
        {
            { EnemySFXType.Idle, idle },
            { EnemySFXType.Walk, walk },
            { EnemySFXType.DetectTarget, detectTarget },
            { EnemySFXType.TakeDamage, takeDamage },
            { EnemySFXType.Retreat, retreat },
            { EnemySFXType.Jump, jump },
            { EnemySFXType.Attack, attack },
            { EnemySFXType.Death, death }
        };
    }

    public void PlayRandomSFX(EnemySFXType _soundType, float _volume = 1.0f)
    {
        if (enemyAudioSource.isPlaying) return;

        if (sfxEnemyClips.TryGetValue(_soundType, out AudioClip[] clips) && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            enemyAudioSource.clip = selectedClip;
            enemyAudioSource.volume = _volume;
            enemyAudioSource.loop = false;
            enemyAudioSource.Play();
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

    public void StopSound(EnemySFXType _soundType)
    {
        if (sfxEnemyClips.TryGetValue(_soundType, out AudioClip[] clips) && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip selectedClip = clips[randomIndex];

            enemyAudioSource.clip = selectedClip;
            enemyAudioSource.Stop();
        }
    }
}

public enum EnemySFXType
{
    Idle,
    Walk,
    DetectTarget,
    TakeDamage,
    Retreat,
    Jump,
    Attack,
    Death
}
