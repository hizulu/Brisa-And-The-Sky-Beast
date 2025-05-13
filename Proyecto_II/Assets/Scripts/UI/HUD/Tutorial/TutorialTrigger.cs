using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Jone Sainz Egea
// 18/04/2025 basic simple tutorial triggered on trigger enter
    // 26/04/2025 Added option to queue messages if there are more tutorials to be triggered after the first is completed
    // 26/04/2025 Added option to trigger messages by action

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private List<Tutorial> tutorials = new List<Tutorial>();

    private int currentIndex = 0;
    private TutorialMessage currentMessage;
    private bool triggered = false;

    private bool canceled = false;

    private void OnEnable()
    {
        foreach (var tutorial in tutorials)
        {
            if (tutorial.triggeredByAction)
                EventsManager.CallNormalEvents(tutorial.activationEventName, () => OnTriggeredByAction(tutorial));
        }
    }

    private void OnDisable()
    {
        foreach (var tutorial in tutorials)
        {
            if (tutorial.triggeredByAction)
                EventsManager.StopCallNormalEvents(tutorial.activationEventName, () => OnTriggeredByAction(tutorial));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canceled)
            return;

        if (other.CompareTag("Player"))
        {
            if (currentIndex >= tutorials.Count)
            {
                // Debug.Log("Todos los tutoriales de esta lista han sido completados");
                return;
            }

            if (tutorials[currentIndex].persistentWhileInsideTrigger)
            {
                DisplayTutorial(tutorials[currentIndex]);
                return;
            }

            if (triggered) return;

            triggered = true;

            if (tutorials.Count == 0)
            {
                Debug.LogWarning("TutorialTrigger: No hay tutoriales configurados.");
                return;
            }

            ShowNextMessage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (canceled || other.CompareTag("Player") == false)
            return;

        if (tutorials.Count > currentIndex && tutorials[currentIndex].persistentWhileInsideTrigger)
        {
            if (currentMessage != null)
            {
                TutorialManager.Instance.RemoveMessage(currentMessage);
                StartCoroutine(TutorialManager.Instance.FadeOutAndDestroy(currentMessage));
                currentMessage = null;
            }

            triggered = false;
        }
    }

    private void OnTriggeredByAction(Tutorial tutorial)
    {
        if (canceled)
            return;

        if (triggered) return;

        if (tutorials[currentIndex] == tutorial)
        {
            triggered = true;
            DisplayTutorial(tutorial);
        }
    }

    private void ShowNextMessage()
    {
        if (canceled)
            return;

        // Debug.Log($"Showing next message at number {currentIndex}");
        if (currentMessage != null)
        {
            Debug.LogWarning("Ya hay un tutorial en marcha");
            return;
        }

        if (currentIndex >= tutorials.Count)
        {
            // Debug.Log("Todos los tutoriales de esta lista han sido completados");
            return;
        }

        Tutorial tutorial = tutorials[currentIndex];

        if (tutorial.triggeredByAction)
        {
            // Si el siguiente tutorial depende de un evento externo, dejamos que sea ese evento el que llame a DisplayTutorial
            triggered = false;
            return;
        }

        DisplayTutorial(tutorial);
    }

    private IEnumerator TransitionToNextMessage()
    {
        if (canceled)
            yield break;

        TutorialManager.Instance.RemoveMessage(currentMessage);
        yield return StartCoroutine(TutorialManager.Instance.FadeOutAndDestroy(currentMessage));
        currentMessage = null;

        ShowNextMessage();
    }

    private void DisplayTutorial(Tutorial tutorial)
    {
        if (canceled)
            return;

        InputAction action = TutorialManager.Instance.inputActions.FindAction(tutorial.inputActionName);
        if (action == null)
        {
            Debug.LogWarning($"TutorialTrigger: No se encontró la acción '{tutorial.inputActionName}'.");
            return;
        }

        currentMessage = TutorialManager.Instance.ShowMessage(tutorial);
        currentMessage.Initialize(tutorial, tutorial.waitForCompletion ? (System.Action)null : CompleteCurrentStep);
    }

    // Método público para forzar el avance manual si es waitForCompletion
    public void CompleteCurrentStep()
    {
        if (canceled)
            return;

        currentIndex++;
        StartCoroutine(TransitionToNextMessage());
    }

    public void HasBeenCanceled()
    {
        if (currentMessage != null)
        {
            TutorialManager.Instance.RemoveMessage(currentMessage);
        }
        canceled = true;
    }
}
