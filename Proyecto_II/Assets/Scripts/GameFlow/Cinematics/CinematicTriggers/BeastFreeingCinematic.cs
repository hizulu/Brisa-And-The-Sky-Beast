using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastFreeingCinematic : MonoBehaviour
{
    private bool hasBeenTriggered = false;
    
    private void OnEnable()
    {
        EventsManager.CallNormalEvents("BeastFreed", OnBeastFreed);
    }

    private void OnDisable()
    {
        EventsManager.StopCallNormalEvents("BeastFreed", OnBeastFreed);
    }

    private void OnBeastFreed()
    {
        Debug.Log("Beast has been freed, checking if cinematic needs to be triggered");
        
        if (hasBeenTriggered)
            return;

        hasBeenTriggered = true;
        Debug.Log("Should play cinematic 1");
        CinematicsManager.Instance.PlayCinematic(1);
    }
}
