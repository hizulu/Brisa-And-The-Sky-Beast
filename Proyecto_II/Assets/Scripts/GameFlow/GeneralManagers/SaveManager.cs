using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

/* NOMBRE CLASE: SaveManager
 * AUTOR: Jone Sainz Egea
 * FECHA: 12/03/2025
 * DESCRIPCIÓN: Script base que se encarga del guardado de datos
 * VERSIÓN: 1.0 estructura básica de singleton
 */

#region InformationToSave
[System.Serializable]
public struct PlayerState
{
    public Vector3 playerPosition;
    public float playerHealth;
    public string equippedWeaponID;
}
[System.Serializable]
public struct Beast_State
{
    public Vector3 beastPosition;
    public float beastHealth;
    // Beast State? Leave Beast free?
}
[System.Serializable]
public struct ItemState
{
    public int itemID;
    public string itemName;
    public bool itemPickedUp;
    public Vector3 itemPosition;
    public bool[] activeItems;
    public bool[] activeItemColliders;
}
[System.Serializable]
public struct InventoryState
{
    public string itemID;
    public int amount;
}
[System.Serializable]
public struct EnemyState
{
    public int enemyID;
    public bool[] activeEnemyColliders;
    public bool isEnemyDead;
}
[System.Serializable]
public struct CheckpointState
{
    public Vector3 position;
    public bool isActive;
}
[System.Serializable]
public struct SceneState
{
    public bool isBeastFreed;
    public int activeScene;
    public PlayerState playerState;
    public BeastState beastState;
    public List<ItemState> itemsState;
    public List<InventoryState> inventoryState;
    public List <EnemyState> enemiesState;
    public List<CheckpointState> checkpoints;
}
#endregion
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private SceneState savedSceneState;

    private Player player;
    [SerializeField] private WeaponDatabase weaponDatabase;

    public static List<InventoryState> pendingInventoryLoad;

    // Singleton
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
        player = FindObjectOfType<Player>();

        if (!GameSession.IsNewGame && PlayerPrefs.HasKey("SavedSceneState"))
        {
            string sceneStateJson = PlayerPrefs.GetString("SavedSceneState");
            savedSceneState = JsonUtility.FromJson<SceneState>(sceneStateJson);
            LoadSceneState();
        }
        else
        {
            savedSceneState = new SceneState();
            Debug.Log("La escena empieza sin datos guardados (nueva partida).");
        }
    }

    public void SaveSceneState() 
    {
        Debug.Log("Entered save scene state");
        SceneState sceneState = new SceneState();
        Debug.Log("new scene state created");

        #region Saving Player
        Player infoPlayer = FindObjectOfType<Player>();
        if (infoPlayer != null)
        {
            Debug.Log("Player found");
            //sceneState.playerState.playerPosition = Checkpoint.GetActiveCheckPointPosition();
            //Debug.Log($"Player position saved: {sceneState.playerState.playerPosition}");
            sceneState.playerState.playerHealth = player.GetHealth();
            Debug.Log($"Player health saved: {sceneState.playerState.playerHealth}");
            sceneState.playerState.equippedWeaponID = player.weaponSlot.GetWeaponData()?.weaponID;
            Debug.Log($"Player weaponID saved: {player.weaponSlot.GetWeaponData()?.weaponID}");
        }
        #endregion

        //#region Saving Checkpoint
        //sceneState.checkpoints.Clear();
        //foreach (GameObject cp in Checkpoint.CheckPointsList)
        //{
        //    Checkpoint checkpoint = cp.GetComponent<Checkpoint>();
        //    sceneState.checkpoints.Add(new CheckpointState
        //    {
        //        position = cp.transform.position,
        //        isActive = checkpoint.Activated
        //    });
        //}
        //#endregion

        sceneState.inventoryState = InventoryManager.Instance.SaveInventory();

        // Saving in a JSON
        string sceneStateJson = JsonUtility.ToJson(sceneState);
        PlayerPrefs.SetString("SavedSceneState", sceneStateJson);
        PlayerPrefs.Save();
    }

    public void LoadSceneState()
    {
        if (PlayerPrefs.HasKey("SavedSceneState"))
        {
            string sceneStateJson = PlayerPrefs.GetString("SavedSceneState");
            savedSceneState = JsonUtility.FromJson<SceneState>(sceneStateJson);

            Debug.Log("Starting coroutine");
            StartCoroutine(LoadGameDataCoroutine());
        }
    }

    private IEnumerator LoadGameDataCoroutine()
    {
        Debug.Log("Dentro de la corrutina1");
        // yield return null;
        Debug.Log("Dentro de la corrutina2");
        #region Loading Player
        // Asegurarse que el jugador existe
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            Debug.Log("Looking for player");
        } else
            Debug.Log("Ya había player");


        if (player != null)
        {
            Debug.Log("Player wasn't null");
            //savedPlayer.SetPosition(savedSceneState.playerState.playerPosition);
            // Cargar vida
            player.SetHealth(savedSceneState.playerState.playerHealth);
            Debug.Log($"Player health should be: {savedSceneState.playerState.playerHealth}");

            // Cargar arma
            if (!string.IsNullOrEmpty(savedSceneState.playerState.equippedWeaponID))
            {
                WeaponData savedWeapon = weaponDatabase.GetWeaponByID(savedSceneState.playerState.equippedWeaponID);
                if (savedWeapon != null)
                {
                    player.weaponSlot.SetWeapon(savedWeapon);
                }
            }

            // Cargar inventario
            if (InventoryManager.Instance != null && savedSceneState.inventoryState != null)
            {
                yield return new WaitUntil(() => InventoryManager.Instance.IsReady);
                InventoryManager.Instance.LoadInventory(savedSceneState.inventoryState);
            }
        }
        #endregion

        //#region Loading Checkpoints
        //for (int i = 0; i < savedSceneState.checkpoints.Count; i++)
        //{
        //    Checkpoint checkpoint = Checkpoint.CheckPointsList[i].GetComponent<Checkpoint>();
        //    checkpoint.Activated = savedSceneState.checkpoints[i].isActive;
        //    checkpoint.GetComponent<MeshRenderer>().material = checkpoint.Activated ? checkpoint.green : checkpoint.magenta;
        //}
        //#endregion

        //InventoryManager.Instance.LoadInventory(savedSceneState.inventoryState);
    }

    public void LoadInventoryState()
    {
        Debug.Log("Loading inventory");
        if (PlayerPrefs.HasKey("SavedSceneState"))
        {
            string sceneStateJson = PlayerPrefs.GetString("SavedSceneState");
            savedSceneState = JsonUtility.FromJson<SceneState>(sceneStateJson);

            pendingInventoryLoad = savedSceneState.inventoryState;
        }
    }

    // Para crear una partida nueva y empezar con todos los datos de 0
    public void ResetProgress()
    {

    }

    public void SavePlayerSettings()
    {
        // Debug.Log("Saving player settings");
    }

    public void LoadPlayerSettings()
    {
        // Debug.Log("Loading player settings");
    }
}
