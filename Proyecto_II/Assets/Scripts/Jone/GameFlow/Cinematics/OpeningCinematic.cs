using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// Jone Sainz Egea
// 16/05/2025
public class OpeningCinematic : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    [SerializeField] GameObject panelPausa;

    private bool pausado = false;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        videoPlayer.loopPointReached += OnVideoEnd;
        panelPausa.SetActive(false);
        Cinematics.CineReproduciendo = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausado)
                Pause();
            else
                Resume();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        NextScene();
    }

    public void NextScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cinematics.CineReproduciendo = false;
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        int siguienteEscena = escenaActual + 1;
        SceneManager.LoadScene(siguienteEscena);
    }

    private void Pause()
    {
        videoPlayer.Pause();
        panelPausa.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausado = true;
    }

    public void Resume()
    {
        videoPlayer.Play();
        panelPausa.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausado = false;
    }
}
