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
        Debug.Log("Should play cinematic 0");
        CinematicsManager.Instance.PlayCinematic(1);
    }
}
