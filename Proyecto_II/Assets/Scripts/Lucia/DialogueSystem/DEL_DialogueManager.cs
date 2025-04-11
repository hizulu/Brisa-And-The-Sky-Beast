using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public DialogueDatabase database;
    public TMP_Text characterNameText;
    public TMP_Text dialogueText;
    public Button optionButton1;
    public Button optionButton2;
    public Button optionButton3;
    public Button goodbyeButton;
    public GameObject dialoguePanel;

    private HashSet<int> readLines = new HashSet<int>();
    private DialogueLine currentLine;
    private Coroutine typingCoroutine;
    private Dictionary<int, bool> usedOptions = new();
    private bool isTyping;

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        database.LoadCSV();
    }

    public void StartDialogueForNPC(int npcID)
    {
        usedOptions.Clear();
        Debug.Log("Buscando diálogo para NPC ID: " + npcID);
        foreach (var line in database.GetAllLines())
        {
            if (line.npcID == npcID && line.ID == 0)
            {
                StartDialogue(line.ID);
                return;
            }
        }
        Debug.LogWarning("No se encontró diálogo para el NPC ID: " + npcID);
    }

    public void StartDialogue(int id)
    {
        DialogueLine nextLine = database.GetLine(id);
        if (nextLine == null)
        {
            EndDialogue();
            return;
        }

        // Verificar si la línea requiere que se haya leído otra línea antes de continuar
        if (nextLine.requirementID != -1 && !readLines.Contains(nextLine.requirementID))
        {
            // Si la línea requerida no ha sido leída, no se puede continuar con este diálogo
            Debug.Log("No se puede continuar, la línea requerida no ha sido leída aún.");
            return;
        }

        // Continuar con el diálogo normal
        if (dialoguePanel != null && !dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(true);
            Debug.Log("Activando panel de diálogo.");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        currentLine = nextLine;
        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        characterNameText.text = currentLine.name;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(currentLine.initialText));
        optionButton1.gameObject.SetActive(false);
        optionButton2.gameObject.SetActive(false);
        optionButton3.gameObject.SetActive(false);
        goodbyeButton.gameObject.SetActive(false);

        // Marcar la línea como leída
        readLines.Add(currentLine.ID);
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
        isTyping = false;

        // Mostrar opciones después de que el texto se haya escrito completamente
        DisplayOptions(currentLine.options);
    }

    private void DisplayOptions(List<DialogueOption> options)
    {
        int optionIndex = 0;
        optionButton1.gameObject.SetActive(false);
        optionButton2.gameObject.SetActive(false);
        optionButton3.gameObject.SetActive(false);
        goodbyeButton.gameObject.SetActive(false);

        Debug.Log($"Number of options available: {options.Count}");
        foreach (var option in options)
        {
            if (usedOptions.ContainsKey(option.nextID))
                continue; // Skip already used options

            if (optionIndex >= 3)
                break; // Only allow up to 3 options

            Button button = optionIndex switch
            {
                0 => optionButton1,
                1 => optionButton2,
                2 => optionButton3,
                _ => null
            };

            var buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = option.optionText;
            button.gameObject.SetActive(true);
            int nextID = option.nextID;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                usedOptions[nextID] = true;
                if (nextID == -1)
                {
                    EndDialogue();
                    return;
                }
                StartDialogue(nextID);
                Debug.Log("Siguiente linea de diálogo (en opciones): " + nextID);
            });
            optionIndex++;
        }

        // Show the goodbye button if there are options available
        if (optionIndex > 0)
        {
            goodbyeButton.gameObject.SetActive(true);
            goodbyeButton.onClick.RemoveAllListeners();
            goodbyeButton.onClick.AddListener(EndDialogue);
        }
        else
        {
            Debug.Log("No options available to display.");
        }
    }

    public void EndDialogue()
    {
        dialogueText.text = "";
        characterNameText.text = "";
        optionButton1.gameObject.SetActive(false);
        optionButton2.gameObject.SetActive(false);
        optionButton3.gameObject.SetActive(false);
        goodbyeButton.gameObject.SetActive(false);
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        Debug.Log("Dialogue ended.");
    }
}