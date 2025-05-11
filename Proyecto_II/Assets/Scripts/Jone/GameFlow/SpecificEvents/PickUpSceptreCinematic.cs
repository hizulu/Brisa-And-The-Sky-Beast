using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSceptreCinematic : MonoBehaviour
{
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
        CinematicsManager.Instance.PlayCinematic(0);
    }
}
