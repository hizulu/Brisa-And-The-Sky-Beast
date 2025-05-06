using UnityEngine;

/*
 * NOMBRE CLASE: LootBox
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 06/05/2025
 * DESCRIPCIÓN: Script que gestiona el loot disponible.
 * VERSIÓN: 1.0. 
 */

public class LootBox : MonoBehaviour
{
    [SerializeField] private LootItem[] lootItems;

    /*
     * Método que instancia los items del loot y los distribuye aleatoriamente de posición dentro de un límite cercano al LootBox.
     * Cada item del loot tiene una probabilidad (se le asigna en el inspector) de instanciarse, para así no asegurar siempre las mismas recompensas.
     */
    public void DropLoot()
    {
        foreach (LootItem item in lootItems)
        {
            if (Random.value <= item.dropChance)
            {
                Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f));
                Vector3 dropPosition = transform.position + offset;
                Instantiate(item.prefab, dropPosition, Quaternion.identity);
            }
        }
    }
}