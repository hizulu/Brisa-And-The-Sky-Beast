using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 26/04/2025
public class TutorialTriggerByTutorialEnd : MonoBehaviour
{
    [SerializeField] private string[] inputActionName;
    [TextArea]
    [SerializeField] private string[] tutorialText;

    private bool triggered = false;
    private int i = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("Debería aparecer mensaje de tutorial");
            TutorialManager.Instance.ShowMessage(inputActionName[i], tutorialText[i], true);
            while (i < inputActionName.Length - 2)
            {
                i += 1;
                TutorialManager.Instance.QueueMessage(i, inputActionName[i], tutorialText[i], true);
            }
            i += 1;
            TutorialManager.Instance.QueueMessage(i, inputActionName[i], tutorialText[i]); // El último de la lista
        }
    }

    public void TriggerNextTutorial()
    {
        i += 1;
    }
}
