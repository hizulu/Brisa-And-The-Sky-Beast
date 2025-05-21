using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea 
// 21/05/2025
public class GameEndTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        EventsManager.CallNormalEvents("GameEndTrigger", TriggerEnd);
    }

    private void OnDisable()
    {
        EventsManager.StopCallNormalEvents("GameEndTrigger", TriggerEnd);
    }

    private void TriggerEnd()
    {
        Debug.Log("Game ending");

        EventsManager.TriggerNormalEvent("EnterTemple");
    }
}
