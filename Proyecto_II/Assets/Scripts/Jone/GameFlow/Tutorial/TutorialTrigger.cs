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
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            TryTriggerTutorial();
        }
    }

    private void OnTriggeredByAction(Tutorial tutorial)
    {
        if (triggered) return;

        if (tutorials[currentIndex] == tutorial)
        {
            triggered = true;
            DisplayTutorial(tutorial);
            currentIndex++; // Avanzamos manualmente aquí después de mostrarlo
        }
    }

    private void TryTriggerTutorial()
    {
        triggered = true;
        StartTutorialSequence();
    }

    private void StartTutorialSequence()
    {
        if (tutorials.Count == 0)
        {
            Debug.LogWarning("TutorialTrigger: No hay tutoriales configurados.");
            return;
        }

        ShowNextMessage();
    }

    private void ShowNextMessage()
    {
        if (currentMessage != null)
        {
            StartCoroutine(TransitionToNextMessage());
            return;
        }

        if (currentIndex >= tutorials.Count)
        {
            // Todos los tutoriales completados
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
        yield return StartCoroutine(TutorialManager.Instance.FadeOutAndDestroy(currentMessage));
        currentMessage = null;
        currentIndex++;

        // Después de destruir el mensaje anterior, mostramos el siguiente
        ShowNextMessage();
    }

    private void DisplayTutorial(Tutorial tutorial)
    {
        InputAction action = TutorialManager.Instance.inputActions.FindAction(tutorial.inputActionName);
        if (action == null)
        {
            Debug.LogWarning($"TutorialTrigger: No se encontró la acción '{tutorial.inputActionName}'.");
            return;
        }

        currentMessage = TutorialManager.Instance.ShowMessage(tutorial);
        currentMessage.Initialize(tutorial, tutorial.waitForCompletion ? (System.Action)null : CompleteCurrentStep);

        currentIndex++;
    }

    // Método público para forzar el avance manual si es waitForCompletion
    public void CompleteCurrentStep()
    {
        StartCoroutine(TransitionToNextMessage());
    }
}
