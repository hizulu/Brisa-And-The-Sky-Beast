using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// Jone Sainz Egea
// 10/05/2025
public class CinematicsManager : MonoBehaviour
{
    public static CinematicsManager Instance;
    public static bool CineReproduciendo { get; set; } = false;

    public GameObject[] videos;

    [SerializeField] private GameObject pausePanel;

    private bool paused = false;
    private GameObject videoGO;
    private VideoPlayer videoPlayer;
    private int numCinem = 0;

    [SerializeField] GameObject camGO;
    private CameraFade cam;

    PlayerInput playerInput;
    int originalGeneralAudioVolume;
    AudioManager audioManager;

    private void Awake()
    {
        // Estructura singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        cam = camGO.GetComponent<CameraFade>();
        playerInput = FindObjectOfType<PlayerInput>();
        AudioSettings.Instance.generalVolume = originalGeneralAudioVolume;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayCinematic(int cinem)
    {
        numCinem = cinem;
        videoGO = videos[numCinem];

        videoPlayer = videoGO.GetComponent<VideoPlayer>();

        StartCoroutine(StartCinematic());
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        CinematicEnd();
    }

    private void CinematicEnd()
    {
        Debug.Log("OnVideoEnd triggered");

        playerInput.PlayerActions.Enable();
        AudioSettings.Instance.generalVolume = originalGeneralAudioVolume;
        AudioSettings.Instance.UpdateAllVolumes();
        audioManager.music.UnPause();
        audioManager.smallSounds.UnPause();
        audioManager.environmentSounds.UnPause();

        videoGO.SetActive(false);
        CineReproduciendo = false;

        Time.timeScale = 1f;

        // TODO: smooth transition

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        switch (numCinem)
        {
            case 0:
                return;
            case 1:
                if(SceneManager.GetActiveScene().buildIndex == 2)
                {
                    // Hondonada
                    Debug.Log("Termina de reproducir segunda cinemática en hondonada");
                    // TODO: Reposicionar Brisa y Bestia salida jaula
                }
                if(SceneManager.GetActiveScene().buildIndex == 3) // Ehuna
                    GameManager.Instance.Victory();
                return;
            default:
                return;
        }
    }

    IEnumerator StartCinematic()
    {
        yield return null;

        playerInput.PlayerActions.Disable();
        AudioSettings.Instance.generalVolume = 0;
        AudioSettings.Instance.UpdateAllVolumes();
        audioManager.music.Pause();
        audioManager.smallSounds.Pause();
        audioManager.environmentSounds.Pause();

        videoGO.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        videoPlayer.Play();
        videoPlayer.SetDirectAudioVolume(0, 1);// multiplicador sonido)

        CineReproduciendo = true;
        Time.timeScale = 0f;
        // TODO: stop all sounds
        //AudioManager.instance.StopAllSounds();
        videoPlayer.loopPointReached += OnVideoEnd;

    }

    #region Pause & Resume
    public void TogglePauseResume()
    {
        if (!paused)
            Pause();
        else
            Resume();
    }

    private void Pause()
    {
        videoPlayer.Pause();

        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        paused = true;
    }

    private void Resume()
    {
        videoPlayer.Play();

        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        paused = false;
    }

    public void SkipCinematic()
    {
        videoPlayer.Pause();
        pausePanel.SetActive(false);

        CinematicEnd();
    }
    #endregion
}
