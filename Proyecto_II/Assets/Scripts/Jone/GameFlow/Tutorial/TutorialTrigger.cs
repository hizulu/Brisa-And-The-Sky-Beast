using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 18/04/2025
public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private string inputActionName;
    [TextArea]
    [SerializeField] private string tutorialText;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            TutorialManager.Instance.ShowMessage(inputActionName, tutorialText);
        }
    }
}
