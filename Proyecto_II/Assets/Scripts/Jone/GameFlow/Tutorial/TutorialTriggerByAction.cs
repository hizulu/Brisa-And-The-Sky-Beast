using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 26/04/2025
public class TutorialTriggerByAction : MonoBehaviour
{
    [SerializeField] private string inputActionName;
    [TextArea]
    [SerializeField] private string tutorialText;

    [SerializeField] private string actionName;

    private bool triggered = false;

    private void OnEnable()
    {
        EventsManager.CallNormalEvents(actionName, OnTriggeredByAction);
    }

    private void OnDisable()
    {
        EventsManager.StopCallNormalEvents(actionName, OnTriggeredByAction);
    }

    private void OnTriggeredByAction()
    {
        if (triggered) return;

        triggered = true;
        //TutorialManager.Instance.ShowMessage(inputActionName, tutorialText);
    }
}
