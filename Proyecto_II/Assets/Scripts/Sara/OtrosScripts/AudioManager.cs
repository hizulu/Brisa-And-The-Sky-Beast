using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;


    public AudioClip walk;
    public AudioClip run;

    private bool isLoopPlaying = false;
    [SerializeField] private float overlapTime = 0.01f; // Tiempo de solapamiento (1 milisegundo)

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
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