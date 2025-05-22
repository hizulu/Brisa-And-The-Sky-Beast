using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource music;
    [SerializeField] public AudioSource smallSounds;
    [SerializeField] public AudioSource environmentSounds;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip[] smallClips;
    [SerializeField] private AudioClip[] ambientClips;

    private void Start()
    {
        music.volume = 0.1f;
        smallSounds.volume = 0.07f;
        environmentSounds.volume = 0.15f;

        EnviromentSoundPlayLoop();

        StartCoroutine(PlayIntroMusicThenLoopSoundscape());
    }

    private void EnviromentSoundPlayLoop()
    {
        environmentSounds.loop = true;
        environmentSounds.Play();
    }

    private IEnumerator PlayIntroMusicThenLoopSoundscape()
    {
        StartCoroutine(PlayRandomSmallSound());

        yield return new WaitForSeconds(2f);

        if (musicClip != null)
        {
            music.clip = musicClip;
            music.loop = false;

            yield return StartCoroutine(FadeInMusic(2f, 0.1f));

            yield return new WaitWhile(() => music.isPlaying);

            yield return StartCoroutine(FadeOutMusic(2f));
        }

        StartCoroutine(SoundscapeLoop());
    }


    private IEnumerator FadeInMusic(float duration, float targetVolume)
    {
        float startVolume = 0f;
        music.volume = 0f;
        music.Play();

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            music.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            yield return null;
        }

        music.volume = targetVolume;
    }

    private IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = music.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            music.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        music.volume = 0f;
        music.Stop();
    }


    private IEnumerator SoundscapeLoop()
    {
        while (true)
        {
            bool playBird = Random.value > 0.5f;

            if (playBird && smallClips.Length > 0)
            {
                yield return StartCoroutine(PlayRandomSmallSound());
            }

            yield return new WaitForSeconds(Random.Range(0f, 7f));
        }
    }

    private IEnumerator PlayRandomSmallSound()
    {
        AudioClip clip = smallClips[Random.Range(0, smallClips.Length)];
        smallSounds.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
    }
}