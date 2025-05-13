using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

// Jone Sainz Egea
// 16/05/2025
public class Cinematics : MonoBehaviour
{
    public static Cinematics Instance; // Singleton

    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject cintaVHS;
    [SerializeField] private GameObject cintaFinBueno;
    [SerializeField] private GameObject cintaFinMalo;
    [SerializeField] private GameObject cintaCreditos;
    private GameObject cinta;
    private VideoPlayer cinematica;
    private int numCinem = 0;
    private bool pausado = false;

    public static bool CineReproduciendo { get; set; } = false;

    [SerializeField] GameObject puertaGO;

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
        cintaVHS.SetActive(false);
        panelPausa.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CineReproduciendo)
        {
            if (!pausado)
                Pausar();
            else
                Reanudar();
        }
    }

    public void Reproducir(int cinem)
    {
        switch (cinem)
        {
            case 0:
                numCinem = 0;
                cinta = cintaVHS;
                break;
            case 1:
                numCinem = 1;
                cinta = cintaFinBueno;
                break;
            case 2:
                numCinem = 2;
                cinta = cintaFinMalo;
                break;
            default:
                numCinem = 3;
                cinta = cintaCreditos;
                break;
        }
        cinematica = cinta.GetComponent<VideoPlayer>();

        cinta.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cinematica.Play();
        CineReproduciendo = true;
        //AudioManager.instance.StopAllSounds();
        cinematica.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        cinta.SetActive(false);
        //if (numCinem == 0)
        //{
        //    puerta.VHSVisto();
        //    Debug.Log("puerta abre");
        //}
        CineReproduciendo = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (numCinem == 1 || numCinem == 2)
        {
            Reproducir(3);
        }
        else if (numCinem == 3)
        {
            //GameManager.instance.GuardarDatosEscena();
            //GameManager.instance.VolverAlInicio();
        }
    }

    public void Pausar()
    {
        Debug.Log("Pausar llamada"); // Mensaje de depuración
        cinematica.Pause();
        panelPausa.SetActive(true);
        Debug.Log("Panel de pausa activado"); // Mensaje de depuración
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausado = true;
    }

    public void Reanudar()
    {
        Debug.Log("Reanudar llamada"); // Mensaje de depuración
        cinematica.Play();
        panelPausa.SetActive(false);
        Debug.Log("Panel de pausa desactivado"); // Mensaje de depuración
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausado = false;
    }

    public void SaltarCinematica()
    {
        Debug.Log("SaltarCinematica llamada"); // Mensaje de depuración
        cinematica.Pause();
        //puerta.VHSVisto();
        cinta.SetActive(false);
        CineReproduciendo = false;
        panelPausa.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
