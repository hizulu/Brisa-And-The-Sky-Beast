using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 18/04/2025
public class BeastOpenTheHollowExit : BeastActionable
{
    [SerializeField] GameObject camGO;
    private CameraFade cam;

    private void Start()
    {
        cam = camGO.GetComponent<CameraFade>();
    }

    public override bool OnBeast()
    {
        if (!beastIsIn)
        {
            Debug.Log("Beast is not in");
            return false;
        }
        Debug.Log("Triggerea evento de salir de la hondonada");
        EventsManager.TriggerNormalEvent("LeaveTheHollow");
        return true;
    }
}
