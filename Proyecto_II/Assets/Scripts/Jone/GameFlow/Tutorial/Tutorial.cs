using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 26/04/2025
[System.Serializable]
public class Tutorial
{
    [Tooltip("Nombre del InputAction que muestra en el tutorial.")]
    [TextArea]
    public string inputActionName;

    [Tooltip("Texto del tutorial que se mostrará.")]
    [TextArea]
    public string tutorialText;

    [Tooltip("¿Este tutorial se activa por evento externo?")]
    public bool triggeredByAction;

    [Tooltip("Nombre del evento que activa este tutorial si triggeredByAction es true.")]
    [TextArea]
    public string activationEventName;

    [Tooltip("¿Debe esperar una confirmación externa para completar el tutorial?")]
    public bool waitForCompletion;
}
