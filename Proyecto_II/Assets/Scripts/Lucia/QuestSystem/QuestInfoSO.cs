using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esto es un Scriptable Object que se usa para guardar la información de las misiones
[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]

public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set;}

    //Nombre de la misión
    [Header("General")]
    public string displayName;

    //Requerimientos para obtener la misión
    [Header("Requirements")]
    public int levelRequirement;
    public QuestInfoSO[] questPrerequisites;

    //Descripción de la misión
    [Header("Steps")]
    public GameObject[] questStepPrefabs;

    //Recompensas de la misión
    [Header("Rewards")]
    public int goldReward;
    public int experienceReward;

    //Se asegura de que el nombre del objeto sea el mismo que el id
    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
