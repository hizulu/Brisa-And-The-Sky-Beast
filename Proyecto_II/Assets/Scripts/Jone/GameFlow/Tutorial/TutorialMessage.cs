using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Jone Sainz Egea
// 18/04/2025
public class TutorialMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text messageTextUI;

    private InputAction inputAction;
    private bool actionCompleted = false;
    public CanvasGroup CanvasGroup { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        CanvasGroup.alpha = 0f; // Inicialmente invisible
    }

    public void Initialize(InputAction action, string text)
    {
        inputAction = action;
        messageTextUI.text = text;

        inputAction.Enable();
    }

    void Update()
    {
        if (actionCompleted) return;

        if (inputAction != null && inputAction.triggered)
        {
            actionCompleted = true;
            TutorialManager.Instance.RemoveMessage(this);
        }
    }
}
