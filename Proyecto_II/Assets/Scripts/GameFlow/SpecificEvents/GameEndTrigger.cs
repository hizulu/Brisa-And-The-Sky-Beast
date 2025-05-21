using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        GameManager.Instance.Victory();
    }
}
