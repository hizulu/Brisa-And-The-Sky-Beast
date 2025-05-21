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
 *              1.4. (19/05/2025) Pantalla de carga
 */

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    private SaveManager saveManager;

    public static bool HasSaved = false;

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
        if(SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
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
        // En las otras escenas no afecta este escape    
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
        UIManager.Instance.OpenPauseMenu();
    }

    public void ResumeGame()
    {
        ChangeGameState(GameState.Playing);
        EventsManager.TriggerNormalEvent("UIPanelClosed");
        Time.timeScale = 1f;
        UIManager.Instance.ClosePauseMenu();
    }
    #endregion

    #region Scene Management
    public void LoadScene(string sceneName)
    {
        LoadSceneWithVideo(sceneName, loadSaved: false);
    }

    public void LoadNextScene(bool saveState = false, bool loadSaved = false)
    {
        Debug.Log($"Cargando siguiente escena con loadSaved: {loadSaved}");
        if(saveState)
            saveManager.SaveSceneState();
        EventsManager.CleanAllEvents();

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(nextSceneIndex));
        Debug.Log($"Trying to load scene with index: {nextSceneIndex} and name: {nextSceneName}");

        LoadSceneWithVideo(nextSceneName, loadSaved);
    }

    private void LoadSceneWithVideo(string targetScene, bool loadSaved = false)
    {
        StartCoroutine(LoadSceneWithVideoAsync(targetScene, loadSaved));

    }

    private IEnumerator LoadSceneWithVideoAsync(string targetScene, bool loadSaved)
    {
        EventsManager.CleanAllEvents();

        // Cargar pantalla de carga
        yield return SceneManager.LoadSceneAsync("00_LoadingScreen", LoadSceneMode.Single);
        yield return null;

        // Esperar a que el LoadingVideoPlayer esté disponible
        while (LoadingVideoPlayer.Instance == null)
            yield return null;

        // Iniciar vídeo
        LoadingVideoPlayer.Instance.PlayVideo();

        // Esperar a que el vídeo empiece realmente
        //float timeout = 3f;
        //float elapsed = 0f;
        //while (!LoadingVideoPlayer.Instance.IsPlaying && elapsed < timeout)
        //{
        //    elapsed += Time.deltaTime;
        //    yield return null;
        //}

        // Cargar escena objetivo en segundo plano
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return null;
        // yield return new WaitForSeconds(1f);

        if (LoadingVideoPlayer.Instance != null)
        {
            LoadingVideoPlayer.Instance.StopVideo();
            yield return null;
        }
        // Debug.Log("Activa escena nueva");

        asyncLoad.allowSceneActivation = true;

        // Esperar a que termine la activación
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Cargar estados guardados si es necesario
        if (loadSaved)
        {
            saveManager.LoadSceneState();
            saveManager.LoadInventoryState();
        }
    }

    public void BackToMainMenu()
    {
        LoadSceneWithVideo("00_MenuInicial", loadSaved: false);
    }
    #endregion

    #region Save & Load

    public void SaveSceneData()
    {
        saveManager.SaveSceneState();
    }

    public void ReloadScene()
    {
        saveManager.SaveSceneState();

        string currentScene = SceneManager.GetActiveScene().name;
        LoadSceneWithVideo(currentScene, loadSaved: true);
    }

    private void LoadSavedSceneChanges(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadSavedSceneChanges;
        saveManager.LoadSceneState();
        Checkpoint.GetActiveCheckPointPosition();
    }
    #endregion

    #region Global Events
    public void StartNewGame()
    {
        SaveManager.Instance.ResetProgress();
        LoadSceneWithVideo("01_OpeningCinematic", loadSaved: false);
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
        UIManager.Instance.OpenVictoryMenu();
    }
    
    private IEnumerator GameOverScreen()
    {
        yield return new WaitForSeconds(3f);
        ChangeGameState(GameState.GameOver);
        EventsManager.TriggerNormalEvent("UIPanelOpened");
        UIManager.Instance.OpenGameOverMenu();
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
