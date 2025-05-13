using UnityEngine;

/*
 * NOMBRE CLASE: LootItem
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 06/05/2025
 * DESCRIPCI�N: Script que gestiona el item concreto que est� disponible para el lootbox.
 * VERSI�N: 1.0. 
 */

[System.Serializable]
public class LootItem
{
    public GameObject prefab;
    [Range(0f, 1f)] public float dropChance = 1f; // Posibilidad de que salga un item en el loot.
}
