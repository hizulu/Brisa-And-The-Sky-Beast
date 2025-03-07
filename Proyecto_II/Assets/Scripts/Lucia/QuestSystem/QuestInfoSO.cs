using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esto es un Scriptable Object que se usa para guardar la informaci�n de las misiones
[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]

public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set;}

    //Nombre de la misi�n
    [Header("General")]
    public string displayName;

    //Requerimientos para obtener la misi�n
    [Header("Requirements")]
    public int levelRequirement;
    public QuestInfoSO[] questPrerequisites;

    //Descripci�n de la misi�n
    [Header("Steps")]
    public GameObject[] questStepPrefabs;

    //Recompensas de la misi�n
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
