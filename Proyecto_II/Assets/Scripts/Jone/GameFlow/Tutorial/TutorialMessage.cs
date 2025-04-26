using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Jone Sainz Egea
// 18/04/2025
    //26/04/2025 added option to show queued messages
public class TutorialMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text messageTextUI;

    private InputAction inputAction;
    private bool actionCompleted = false;
    private bool hasToTriggerNextTutorial = false;
    private int i = 1;
    public CanvasGroup CanvasGroup { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        CanvasGroup.alpha = 0f; // Inicialmente invisible
    }

    public void Initialize(InputAction action, string text, bool triggersNextTutorial, int iCurrent = 0)
    {
        inputAction = action;
        messageTextUI.text = text;
        hasToTriggerNextTutorial = triggersNextTutorial;
        i = iCurrent;
        inputAction.Enable();
    }

    void Update()
    {
        if (actionCompleted) return;

        if (inputAction != null && inputAction.triggered)
        {
            actionCompleted = true;
            TutorialManager.Instance.RemoveMessage(this);
            if (hasToTriggerNextTutorial)
            {
                TutorialManager.Instance.ShowQueuedMessage(i + 1);
            }
        }
    }
}
