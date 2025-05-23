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

/*
 * NOMBRE CLASE: SoundObjectsManager
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 22/05/2025
 * DESCRIPCI�N: Clase que gestiona la l�gica de los efectos de sonido de objetos interatuables.
 * VERSI�N: 1.0.
 */

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

    /// <summary>
    /// M�todo que reproduce aleatoriamente un sonido asociado al tipo espec�fico (recibe como par�metro).
    /// Si el AudioSource ya est� reproduciendo algo, no hace nada.
    /// </summary>
    /// <param name="_soundType">Tipo de sonido que se quiere reproducir.</param>
    /// <param name="_volume">Nivel del volumen que se quiere reproducir por defecto (esto luego es modificable al llamar a la funci�n).</param>
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

    /// <summary>
    /// M�todo que detiene el sonido indicado.
    /// (De momento no se utiliza, pero est� creado por si en un futuro se necesita).
    /// </summary>
    /// <param name="_soundType">Tipo de sonido que se quiere detener.</param>
    public void StopSFX(SoundType _soundType)
    {
        objectAudioSource.Stop();
    }
}
