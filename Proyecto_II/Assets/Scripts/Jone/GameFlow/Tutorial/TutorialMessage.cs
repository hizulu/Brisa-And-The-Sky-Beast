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
    [SerializeField] private TMPro.TMP_Text messageTextUI;

    private InputAction inputAction;
    private System.Action onActionPerformedCallback;
    private bool waitForCompletion;

    public CanvasGroup CanvasGroup { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        CanvasGroup.alpha = 0f;
    }

    public void Initialize(InputAction action, string text, System.Action callback = null, bool waitForCompletion = false)
    {
        inputAction = action;
        messageTextUI.text = text;
        onActionPerformedCallback = callback;
        this.waitForCompletion = waitForCompletion;

        inputAction.Enable();
    }

    private void Update()
    {
        if (inputAction == null) return;

        if (!waitForCompletion)
        {
            if (inputAction.triggered)
            {
                onActionPerformedCallback?.Invoke();
            }
        }
    }
}
