using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* NOMBRE CLASE: UIManager
 * AUTOR: Jone Sainz Egea
 * FECHA: 12/03/2025
 * DESCRIPCI�N: Script base que se encarga del flujo del juego
 * VERSI�N: 1.0 estructura b�sica de singleton
 */
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
