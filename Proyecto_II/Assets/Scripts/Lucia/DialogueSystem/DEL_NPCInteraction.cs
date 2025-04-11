using System.Collections;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private bool isCoroutineRunning = false;
    public int npcID;
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCoroutineRunning)
        {
            isPlayerInRange = true;
            StartCoroutine(WaitForInteraction());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            StopAllCoroutines();
            isCoroutineRunning = false;
        }
    }

    private IEnumerator WaitForInteraction()
    {
        isCoroutineRunning = true;
        while (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartDialogue();
                yield break;
            }
            yield return null;
        }
        isCoroutineRunning = false;
    }

    private void StartDialogue()
    {
        if (dialogueManager != null)
            dialogueManager.StartDialogueForNPC(npcID);
    }
}
