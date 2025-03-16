using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* NOMBRE CLASE: SaveManager
 * AUTOR: Jone Sainz Egea
 * FECHA: 12/03/2025
 * DESCRIPCIÓN: Script base que se encarga del guardado de datos
 * VERSIÓN: 1.0 estructura básica de singleton
 */

#region
struct BrisaState
{
    // Información del jugador que hay que guardar
}
struct BeastState
{
    // Información de Bestia que hay que guardar
}
struct ItemState
{
    // Información de los items de la escena que hay que guardar
}
struct InventoryState
{
    // Información del inventario que hay que guardar
}
struct EnemyState
{
    // Información de los enemigos de la escena que hay que guardar
}
struct SceneState
{
    // Información de la escena que hay que guardar
}
#endregion
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    // Singleton
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
