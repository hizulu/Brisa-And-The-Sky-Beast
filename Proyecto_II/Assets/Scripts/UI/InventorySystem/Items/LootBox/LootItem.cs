using UnityEngine;

/*
 * NOMBRE CLASE: LootItem
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 06/05/2025
 * DESCRIPCIÓN: Script que gestiona el item concreto que está disponible para el lootbox.
 * VERSIÓN: 1.0. 
 */

[System.Serializable]
public class LootItem
{
    public GameObject prefab;
    [Range(0f, 1f)] public float dropChance = 1f; // Posibilidad de que salga un item en el loot.
}
