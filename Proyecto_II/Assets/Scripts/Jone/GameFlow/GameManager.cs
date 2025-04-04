using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.ShaderData;


/* NOMBRE CLASE: GameManager
 * AUTOR: Jone Sainz Egea
 * FECHA: 12/03/2025
 * DESCRIPCI�N: Script base que se encarga del flujo del juego
 * VERSI�N: 1.0 estructura b�sica de singleton y funciones de pausa
 *              1.1 funciones de guardado y cargado
 */

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private SaveManager saveManager;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject beastSelectionPanel;
    bool isPaused = false;
    bool beastSelectionActive = false;

    // Estructura Singleton
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        saveManager = SaveManager.instance;
    }

    private void OnEnable()
    {
        Pause.OnPause += PauseGame;
        Pause.OnResume += ResumeGame;
    }
    private void OnDisable()
    {
        Pause.OnPause -= PauseGame;
        Pause.OnResume -= ResumeGame;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
            {
                Pause.TriggerPause();

                Debug.Log("Juego pausado");
            }
            else
            {
                Pause.TriggerResume();
                Debug.Log("Juego en marcha");
            }
            /*int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (sceneIndex != 0)
                if (!Cinematicas.CineReproduciendo && !Inventario.estadoInvent)
                {
                    // Manejo normal del panel de pausa
                    if (Time.timeScale != 0)
                        Pause.TriggerPause();
                    else
                        Pause.TriggerResume();
                }*/
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (beastSelectionActive)
            {
                beastSelectionPanel.SetActive(false);
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                beastSelectionActive = false;
            }
            else
            {
                beastSelectionPanel.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                beastSelectionActive = true;
            }
        }
    }

    public void SaveSceneData()
    {
        saveManager.SaveSceneState();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += LoadSavedSceneChanges; // Registered when scene is loaded
    }

    private void LoadSavedSceneChanges(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadSavedSceneChanges; // Removes the method so that it isn't called multiple times
        saveManager.LoadSceneState();
        Checkpoint.GetActiveCheckPointPosition();
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //UIManager.Instance.CargarPantallaPausa();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //UIManager.Instance.QuitarPantallaPausa();
    }

    public void LoadNextScene()
    {
        // TODO
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
