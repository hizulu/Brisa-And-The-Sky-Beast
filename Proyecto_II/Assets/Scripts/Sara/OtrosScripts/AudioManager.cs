using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    public AudioClip walk;
    public AudioClip run;

    private bool isLoopPlaying = false;
    [SerializeField] private float overlapTime = 0.01f; // Tiempo de solapamiento (1 milisegundo)


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