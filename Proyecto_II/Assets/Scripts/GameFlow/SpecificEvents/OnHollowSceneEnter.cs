using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 21/05/2025
public class OnHollowSceneEnter : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
