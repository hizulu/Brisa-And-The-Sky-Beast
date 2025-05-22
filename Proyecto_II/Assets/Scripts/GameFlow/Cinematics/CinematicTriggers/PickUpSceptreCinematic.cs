using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSceptreCinematic : MonoBehaviour
{
    private bool hasBeenTriggered = false;
    
    private void OnEnable()
    {
        EventsManager.CallNormalEvents("PickUpSceptre", OnPickUpSceptre);
    }

    private void OnDisable()
    {
        EventsManager.StopCallNormalEvents("PickUpSceptre", OnPickUpSceptre);
    }

    private void OnPickUpSceptre()
    {
        if (hasBeenTriggered)
            return;

        hasBeenTriggered = true;
        CinematicsManager.Instance.PlayCinematic(0);
    }
}
