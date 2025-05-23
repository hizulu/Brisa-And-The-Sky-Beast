using System.Collections;
using UnityEngine;

/*
 * NOMBRE CLASE: AudioManager
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 22/05/2025
 * DESCRIPCIÓN: Clase que gestiona la lógica de los sonidos ambientales para crear un paisaje sonoro.
 * VERSIÓN: 1.0.
 */

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

    /// <summary>
    /// Método que ejecuta en bucle sonidos de fondo de ambiente.
    /// </summary>
    private void EnviromentSoundPlayLoop()
    {
        environmentSounds.loop = true;
        environmentSounds.Play();
    }

    /// <summary>
    /// Corrutina que da entrada, al empezar la escena, a un sonido y luego reproduce la música.
    /// Una vez terminada la canción, ejecuta en bucle sonidos.
    /// </summary>
    /// <returns>Corrutina que realiza las acciones.</returns>
    private IEnumerator PlayIntroMusicThenLoopSoundscape()
    {
        StartCoroutine(PlayRandomSmallSound());

        yield return new WaitForSeconds(Random.Range(3f, 5f));

        if (musicClip != null)
        {
            music.clip = musicClip;
            music.loop = false;

            yield return StartCoroutine(FadeInMusic(2f, 0.1f));

            yield return new WaitWhile(() => music.isPlaying);

            yield return StartCoroutine(FadeOutMusic(3f));
        }

        StartCoroutine(SoundscapeLoop());
    }

    /// <summary>
    /// Corrutina para que la música no entre de golpe, sino que modifique el nivel de volumen de menos a más.
    /// </summary>
    /// <param name="duration">Tiempo del FadeIn.</param>
    /// <param name="targetVolume">Valor del volumen al que se quiere llegar.</param>
    /// <returns>Corrutina que realiza las acciones.</returns>
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

    /// <summary>
    /// Corrutina para que la música no termine de golpe, sino que modifique el nivel de volumen de más a mmenos.
    /// </summary>
    /// <param name="duration">Tiempo del FadeIn.</param>
    /// <returns>Corrutina que realiza las acciones.</returns>
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

    /// <summary>
    /// Corrutina que reproduce aleatoriamente sonidos del entorno.
    /// Reproduce los sonidos en intervalos de tiempo aleatorios.
    /// Se ha planteado con aleatoriedad por si en un futuro se quiere insertar más tipos de sonidos.
    /// </summary>
    /// <returns>Corrutina que realiza las acciones.</returns>
    private IEnumerator SoundscapeLoop()
    {
        while (true)
        {
            bool playSmallSounds = Random.value > 0.5f;

            if (playSmallSounds && smallClips.Length > 0)
            {
                yield return StartCoroutine(PlayRandomSmallSound());
            }

            yield return new WaitForSeconds(Random.Range(0f, 7f));
        }
    }

    /// <summary>
    /// Corrutina que reproduce aleatoriamente un sonido pequeño del array.
    /// </summary>
    /// <returns>Corrutina que realiza las acciones.</returns>
    private IEnumerator PlayRandomSmallSound()
    {
        AudioClip clip = smallClips[Random.Range(0, smallClips.Length)];
        smallSounds?.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
    }
}