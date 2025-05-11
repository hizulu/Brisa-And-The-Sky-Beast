using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Jone Sainz Egea
// 18/04/2025
    // 26/04/2025 added option to show queued messages
    // 11/05/2025 tutorial gets removed after 5 seconds
public class TutorialMessage : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text messageTextUI;
    [SerializeField] private float messageDuration = 5f;

    private InputAction inputAction;
    private System.Action onTutorialCompleted;
    private bool waitForCompletion;

    public CanvasGroup CanvasGroup { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        CanvasGroup.alpha = 0f;
    }

    public void Initialize(Tutorial tutorial, System.Action callback = null)
    {
        inputAction = TutorialManager.Instance.inputActions.FindAction(tutorial.inputActionName);
        messageTextUI.text = tutorial.tutorialText;
        waitForCompletion = tutorial.waitForCompletion;

        onTutorialCompleted = callback;

        inputAction.Enable();

        if (!waitForCompletion && !tutorial.persistentWhileInsideTrigger)
            StartCoroutine(WaitAndDeactivate());
    }

    private void Update()
    {
        if (inputAction == null) return;

        if (!waitForCompletion)
        {
            if (inputAction.triggered)
            {
                onTutorialCompleted?.Invoke();
            }
        }
    }

    private IEnumerator WaitAndDeactivate()
    {
        float elapsedTime = 0f;
        while(elapsedTime < messageDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        onTutorialCompleted?.Invoke();
    }
}
