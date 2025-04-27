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
    private List<Tutorial> queuedTutorials = new List<Tutorial>();
    private List<TutorialMessage> activeMessages = new List<TutorialMessage>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public TutorialMessage ShowMessage(Tutorial tutorial)
    {
        GameObject obj = Instantiate(tutorialMessagePrefab, canvasParent);
        TutorialMessage msg = obj.GetComponent<TutorialMessage>();

        msg.Initialize(tutorial);
        StartCoroutine(FadeCanvasGroup(msg.CanvasGroup, 0f, 1f, 0.25f, 0.5f));
        activeMessages.Add(msg);
        return msg;
    }

    public void RemoveMessage(TutorialMessage message)
    {
        if (activeMessages.Contains(message))
        {
            activeMessages.Remove(message);
            StartCoroutine(FadeOutAndDestroy(message));
        }
    }

    public IEnumerator FadeOutAndDestroy(TutorialMessage msg)
    {
        yield return StartCoroutine(FadeCanvasGroup(msg.CanvasGroup, 1f, 0f, 0.4f, 0.2f));
        Destroy(msg.gameObject);
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
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
