using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;
    public AudioClip walk;
    public AudioClip run;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
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
