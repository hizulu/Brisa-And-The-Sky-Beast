using UnityEngine;

/*
 * NOMBRE CLASE: LootBox
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 06/05/2025
 * DESCRIPCI�N: Script que gestiona el loot disponible.
 * VERSI�N: 1.0. 
 */

public class LootBox : MonoBehaviour
{
    [SerializeField] private LootItem[] lootItems;

    /*
     * M�todo que instancia los items del loot y los distribuye aleatoriamente de posici�n dentro de un l�mite cercano al LootBox.
     * Cada item del loot tiene una probabilidad (se le asigna en el inspector) de instanciarse, para as� no asegurar siempre las mismas recompensas.
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