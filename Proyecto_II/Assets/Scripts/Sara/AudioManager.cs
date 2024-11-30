using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip backNoLoop;
    public AudioClip backLoop;
    public AudioClip walk;
    public AudioClip run;

    bool noLoop = false;

    private void Start()
    {
        musicSource.clip = backNoLoop;
        musicSource.loop = false;
        musicSource.Play();
    }

    private void Update()
    {
        if (!musicSource.isPlaying && !noLoop)
        {
            PlayLoopingBackground();
        }
    }

    private void PlayLoopingBackground()
    {
        musicSource.clip = backLoop;
        musicSource.loop = true;
        musicSource.Play();
        noLoop = true;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        if (!SFXSource.isPlaying)
        {
            SFXSource.clip = clip;
            SFXSource.loop = false;
            SFXSource.Play();
            Debug.Log("Reproduciendo SFX: " + clip);
        }
    }

    public bool IsPlaying(AudioClip clip)
    {
        return SFXSource.isPlaying && SFXSource.clip == clip;
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }
}
