using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastFreeingCinematic : MonoBehaviour
{  
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
        Debug.Log("Should play cinematic 0");
        CinematicsManager.Instance.PlayCinematic(0);
    }
}
