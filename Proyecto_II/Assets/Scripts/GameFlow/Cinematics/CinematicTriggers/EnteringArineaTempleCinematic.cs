using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteringArineaTempleCinematic : MonoBehaviour
{
    private bool hasBeenTriggered = false;

    private void OnEnable()
    {
        EventsManager.CallNormalEvents("EnterTemple", OnEnterTemple);
    }

    private void OnDisable()
    {
        EventsManager.StopCallNormalEvents("EnterTemple", OnEnterTemple);
    }

    private void OnEnterTemple()
    {
        if (hasBeenTriggered)
            return;

        hasBeenTriggered = true;
        CinematicsManager.Instance.PlayCinematic(1);
    }
}
