using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using static UnityEditor.ShaderData;


/* NOMBRE CLASE: GameManager
 * AUTOR: Jone Sainz Egea
 * FECHA: 12/03/2025
 * DESCRIPCIÓN: Script base que se encarga del flujo del juego
 * VERSIÓN: 1.0 estructura básica de singleton y funciones de pausa
 *              1.1 funciones de guardado y cargado
 */

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private SaveManager saveManager;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] GameObject pausePanel;

    bool isPaused = false;

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

        //playerInput.UIPanelActions.PauseGame.performed += TooglePauseResume;
    }

    private void Start()
    {
        saveManager = SaveManager.instance;
    }

    private void OnEnable()
    {
        //Pause.OnPause += PauseGame;
        //Pause.OnResume += ResumeGame;
    }

    private void OnDestroy()
    {
        //playerInput.UIPanelActions.PauseGame.performed -= TooglePauseResume;        
    }

    private void OnDisable()
    {
        //Pause.OnPause -= PauseGame;
        //Pause.OnResume -= ResumeGame;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (Time.timeScale != 0)
        //    {
        //        Pause.TriggerPause();

        //        Debug.Log("Juego pausado");
        //    }
        //    else
        //    {
        //        Pause.TriggerResume();
        //        Debug.Log("Juego en marcha");
        //    }
        //}

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
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    if (beastSelectionActive)
        //    {
        //        beastSelectionPanel.SetActive(false);
        //        Time.timeScale = 1f;
        //        Cursor.lockState = CursorLockMode.Locked;
        //        Cursor.visible = false;
        //        beastSelectionActive = false;
        //    }
        //    else
        //    {
        //        beastSelectionPanel.SetActive(true);
        //        Time.timeScale = 0f;
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //        beastSelectionActive = true;
        //    }
        //}
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

    void TooglePauseResume(InputAction.CallbackContext context)
    {
        if (InventoryManager.Instance.inventoryEnabled)
        {
            InventoryManager.Instance.OpenCloseInventory(context);
            return;
        }

        if (isPaused)
        {
            Debug.Log("Has vuelto al juego");
            ResumeGame();
        }
        else
        {
            Debug.Log("Has parado el juego");
            PauseGame();
            Debug.Log(Time.timeScale);
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
        EventsManager.TriggerNormalEvent("UIPanelOpened");
        //UIManager.Instance.CargarPantallaPausa();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        EventsManager.TriggerNormalEvent("UIPanelClosed");
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
