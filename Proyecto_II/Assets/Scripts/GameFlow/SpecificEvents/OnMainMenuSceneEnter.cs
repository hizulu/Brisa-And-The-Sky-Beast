using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea 
// 21/05/2025
public class OnMainMenuSceneEnter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
