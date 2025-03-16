using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* NOMBRE CLASE: SaveManager
 * AUTOR: Jone Sainz Egea
 * FECHA: 12/03/2025
 * DESCRIPCI�N: Script base que se encarga del guardado de datos
 * VERSI�N: 1.0 estructura b�sica de singleton
 */

#region
struct BrisaState
{
    // Informaci�n del jugador que hay que guardar
}
struct BeastState
{
    // Informaci�n de Bestia que hay que guardar
}
struct ItemState
{
    // Informaci�n de los items de la escena que hay que guardar
}
struct InventoryState
{
    // Informaci�n del inventario que hay que guardar
}
struct EnemyState
{
    // Informaci�n de los enemigos de la escena que hay que guardar
}
struct SceneState
{
    // Informaci�n de la escena que hay que guardar
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
