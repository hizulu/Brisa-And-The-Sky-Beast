using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    // Weapon?
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
    public int itemID;
    public bool itemInInventory;
    public string itemName;
    public Sprite itemSprite;
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
        if (PlayerPrefs.HasKey("SavedSceneState"))
        {
            string sceneStateJson = PlayerPrefs.GetString("SavedSceneState");
            savedSceneState = JsonUtility.FromJson<SceneState>(sceneStateJson);
        }
        else
        {
            savedSceneState = new SceneState();
            Debug.Log("La escena acaba de empezar sin ningún dato guardado.");
        }
    }

    public void SaveSceneState() 
    {
        SceneState sceneState = new SceneState();

        #region Saving Player
        Player infoPlayer = FindObjectOfType<Player>();
        sceneState.playerState.playerPosition = Checkpoint.GetActiveCheckPointPosition();
        //sceneState.playerState.playerHealth = infoPlayer.GetHealth();
        Debug.Log($"Player health saved: {sceneState.playerState.playerHealth}");
        #endregion

        #region Saving Checkpoint
        sceneState.checkpoints.Clear();
        foreach (GameObject cp in Checkpoint.CheckPointsList)
        {
            Checkpoint checkpoint = cp.GetComponent<Checkpoint>();
            sceneState.checkpoints.Add(new CheckpointState
            {
                position = cp.transform.position,
                isActive = checkpoint.Activated
            });
        }
        #endregion

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

            #region Loading Player
            Player savedPlayer = FindObjectOfType<Player>();
            if (savedPlayer != null)
            {
                //savedPlayer.SetPosition(savedSceneState.playerState.playerPosition);
                //savedPlayer.SetHealth(savedSceneState.playerState.playerHealth);
                Debug.Log($"Player health should be: {savedSceneState.playerState.playerHealth}");
            }
            #endregion

            #region Loading Checkpoints
            for (int i = 0; i < savedSceneState.checkpoints.Count; i++)
            {
                Checkpoint checkpoint = Checkpoint.CheckPointsList[i].GetComponent<Checkpoint>();
                checkpoint.Activated = savedSceneState.checkpoints[i].isActive;
                checkpoint.GetComponent<MeshRenderer>().material = checkpoint.Activated ? checkpoint.green : checkpoint.magenta;
            }
            #endregion
        }
    }

    // Para crear una partida nueva y empezar con todos los datos de 0
    public void ResetProgress()
    {

    }

    public void SavePlayerSettings()
    {
        Debug.Log("Saving player settings");
    }

    public void LoadPlayerSettings()
    {
        Debug.Log("Loading player settings");
    }
}
