using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Jone Sainz Egea
// 18/04/2025
// Clase que se encarga del control de los mensajes de tutorial activos
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private GameObject tutorialMessagePrefab;
    [SerializeField] private Transform canvasParent;

    public InputActionAsset inputActions;

    private List<TutorialMessage> activeMessages = new List<TutorialMessage>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowMessage(string actionName, string messageText)
    {
        GameObject obj = Instantiate(tutorialMessagePrefab, canvasParent);
        TutorialMessage msg = obj.GetComponent<TutorialMessage>();
        InputAction action = inputActions.FindAction(actionName);

        if (action == null)
        {
            Debug.LogWarning($"TutorialManager: No se encontró la acción '{actionName}' en el InputActionAsset.");
            Destroy(obj);
            return;
        }

        msg.Initialize(action, messageText);
        StartCoroutine(FadeCanvasGroup(msg.CanvasGroup, 0f, 1f, 0.25f));
        activeMessages.Add(msg);
    }

    public void RemoveMessage(TutorialMessage message)
    {
        if (activeMessages.Contains(message))
        {
            activeMessages.Remove(message);
            StartCoroutine(FadeOutAndDestroy(message));
        }
    }

    private IEnumerator FadeOutAndDestroy(TutorialMessage msg)
    {
        yield return StartCoroutine(FadeCanvasGroup(msg.CanvasGroup, 1f, 0f, 0.4f));
        Destroy(msg.gameObject);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
    {
        float t = 0f;
        group.alpha = from;

        while (t < duration)
        {
            group.alpha = Mathf.Lerp(from, to, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        group.alpha = to;
    }
}
