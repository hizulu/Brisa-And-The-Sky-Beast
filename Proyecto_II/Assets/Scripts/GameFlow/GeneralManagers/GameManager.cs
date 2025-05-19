using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using static UnityEditor.ShaderData;

// Para la gestión del estado del juego:
public enum GameState { MainMenu, Playing, Paused, GameOver, Victory }


/* NOMBRE CLASE: GameManager
 * AUTOR: Jone Sainz Egea
 * FECHA: 12/03/2025
 * DESCRIPCIÓN: Script base que se encarga del flujo del juego
 * VERSIÓN: 1.0 estructura básica de singleton y funciones de pausa
 *              1.1 funciones de guardado y cargado
 *              1.2. (20/04/2025) Corrección pausa
 *              1.3. (19/05/2025) Guardado de sesión
 */

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    private SaveManager saveManager;
    private UIManager uiManager;

    [SerializeField] private PlayerInput playerInput;

    // Estructura Singleton
    void Awake()
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
    }

    private void Start()
    {
        saveManager = SaveManager.Instance;
        uiManager = UIManager.Instance;

        // TODO: cambiar a estado main menu, por ahora empieza en playing
        ChangeGameState(GameState.Playing);
        LoadPlayerSettings();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscape();
        }
    }

    private void OnEscape()
    {
        if (UIManager.Instance.CheckForOpenedMenus())
            return;

        // Debug.Log("Detecta escape");
        if (CurrentState == GameState.Paused)
        {
            ResumeGame();
            // Debug.Log("Debería reanudar juego");
        }
        else if (CurrentState == GameState.Playing)
        {
            PauseGame();
            // Debug.Log("Debería pausar juego");
        }
    }

    public void ChangeGameState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);

        Time.timeScale = (newState == GameState.Paused || newState == GameState.GameOver || newState == GameState.Victory) ? 0 : 1;
    }

    public static class GameSession
    {
        public static bool IsNewGame = false;
    }

    #region Pause & Resume
    void PauseGame()
    {
        ChangeGameState(GameState.Paused);
        EventsManager.TriggerNormalEvent("UIPanelOpened");
        Time.timeScale = 0f;
        uiManager.OpenPauseMenu();
    }

    public void ResumeGame()
    {
        ChangeGameState(GameState.Playing);
        EventsManager.TriggerNormalEvent("UIPanelClosed");
        Time.timeScale = 1f;
        uiManager.ClosePauseMenu();
    }
    #endregion

    #region Scene Management
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        EventsManager.CleanAllEvents();

        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadNextScene()
    {
        saveManager.SaveSceneState();
        EventsManager.CleanAllEvents();
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        saveManager.LoadInventoryState();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Save & Load

    public void SaveSceneData()
    {
        saveManager.SaveSceneState();
    }

    public void ReloadScene()
    {
        EventsManager.CleanAllEvents();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += LoadSavedSceneChanges; 
    }

    private void LoadSavedSceneChanges(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadSavedSceneChanges;
        saveManager.LoadSceneState();
        Checkpoint.GetActiveCheckPointPosition();
    }
    #endregion

    #region Global Events

    //public void StartTutorial()
    //{
    //    LoadScene(tutorialSceneName);
    //    // También puedes lanzar aquí eventos relacionados si los necesitas
    //}

    public void StartNewGame()
    {
        SaveManager.Instance.ResetProgress();
        // TODO: sustituir por la escena que sea la primera (cinemática?)
        LoadScene("TheHollow");
        ChangeGameState(GameState.Playing);
    }

    public void Victory()
    {
        StartCoroutine(VictoryScreen());
    }

    public void GameOver()
    {
        Debug.Log("Game over called");
        StartCoroutine(GameOverScreen());
    }

    private IEnumerator VictoryScreen()
    {
        yield return new WaitForSeconds(1f);
        ChangeGameState(GameState.Victory);
        EventsManager.TriggerNormalEvent("UIPanelOpened");
        uiManager.OpenVictoryMenu();
    }
    
    private IEnumerator GameOverScreen()
    {
        yield return new WaitForSeconds(3f);
        ChangeGameState(GameState.GameOver);
        EventsManager.TriggerNormalEvent("UIPanelOpened");
        uiManager.OpenGameOverMenu();
        Debug.Log("Finished game over");
    }
    #endregion

    #region Player Settings

    // TODO: llamar cada vez que se cierre la pantalla de ajustes? un botón para guardar ajustes?
    public void SavePlayerSettings()
    {
        saveManager.SavePlayerSettings();
    }

    private void LoadPlayerSettings()
    {
        saveManager.LoadPlayerSettings();
    }

    #endregion
   
}
