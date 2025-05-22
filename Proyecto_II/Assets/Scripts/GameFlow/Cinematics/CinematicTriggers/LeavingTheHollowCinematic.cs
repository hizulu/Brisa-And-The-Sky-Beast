using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingTheHollowCinematic : MonoBehaviour
{
    private void OnEnable()
    {
        EventsManager.CallNormalEvents("LeaveTheHollow", OnLeaveTheHollow);
    }

    private void OnDisable()
    {
        EventsManager.StopCallNormalEvents("LeaveTheHollow", OnLeaveTheHollow);
    }

    private void OnLeaveTheHollow()
    {
        // Debug.Log("Va a reproducir cinemática anterior a salir de la hondonada");
        CinematicsManager.Instance.PlayCinematic(1);
    }
}
