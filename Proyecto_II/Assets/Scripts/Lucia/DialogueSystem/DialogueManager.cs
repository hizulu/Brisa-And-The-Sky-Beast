using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public Button optionButton1;
    public Button optionButton2;
    public Button optionButton3;
    public Button goodbyeButton;
    public GameObject dialoguePanel;

    [Header("Settings")]
    public float typingSpeed = 0.02f;

    private DialogueDatabase database;
    private DialogueLine currentLine;
    private Dictionary<int, bool> usedOptions = new Dictionary<int, bool>();
    private Coroutine typingCoroutine;
    private bool isTyping;

    private void Awake()
    {
        database = GetComponent<DialogueDatabase>();

        // Desactivar todos los botones al inicio
        optionButton1.gameObject.SetActive(false);
        optionButton2.gameObject.SetActive(false);
        optionButton3.gameObject.SetActive(false);
        goodbyeButton.gameObject.SetActive(false);

        dialogueText.text = "";
        speakerText.text = "";
    }

    private void Start()
    {
        database.LoadCSV(); // Cargar los datos del CSV al inicio
    }

    public void StartDialogue(int id)
    {
        Debug.Log("Starting dialogue with ID: " + id);
        currentLine = database.GetLine(id);
        if (currentLine == null)
        {
            EndDialogue();
            return;
        }

        EnableDialoguePanel(true);
        DisplayCurrentLine();
    }

    public void EnableDialoguePanel(bool isActive)
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(isActive);  // Activa o desactiva el panel
        }
    }

    private void DisplayCurrentLine()
    {
        // Cancelar escritura anterior si existe
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // Mostrar speaker
        speakerText.text = currentLine.name;

        // Iniciar el efecto de máquina de escribir
        typingCoroutine = StartCoroutine(TypeText(currentLine.initialText));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;

        // Esperar hasta que termine de escribir antes de mostrar las opciones
        StartCoroutine(EnableOptionsAfterTyping());
    }

    private IEnumerator EnableOptionsAfterTyping()
    {
        // Esperar hasta que termine la escritura
        while (isTyping)
            yield return null;

        int optionIndex = 0;

        // Asignar opciones a los botones
        foreach (var option in currentLine.options)
        {
            if (usedOptions.ContainsKey(option.nextID)) continue;
            if (optionIndex >= 3) break; // Solo 3 botones de opción

            Button button = null;

            // Asignar el botón adecuado según el índice
            switch (optionIndex)
            {
                case 0:
                    button = optionButton1;
                    break;
                case 1:
                    button = optionButton2;
                    break;
                case 2:
                    button = optionButton3;
                    break;
            }

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
            });

            optionIndex++;
        }

        // Mostrar el botón "Adiós" si hay opciones válidas
        if (optionIndex > 0)
        {
            goodbyeButton.gameObject.SetActive(true);
            goodbyeButton.onClick.RemoveAllListeners();
            goodbyeButton.onClick.AddListener(EndDialogue);
        }
    }

    private void EndDialogue()
    {
        currentLine = null;
        speakerText.text = "";
        dialogueText.text = "";

        // Desactivar todos los botones
        optionButton1.gameObject.SetActive(false);
        optionButton2.gameObject.SetActive(false);
        optionButton3.gameObject.SetActive(false);
        goodbyeButton.gameObject.SetActive(false);

        EnableDialoguePanel(false);
    }

    // Función que se llama cuando se presiona la tecla E (para acelerar el texto)
    public void CompleteText()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentLine.initialText;
            isTyping = false;
            StartCoroutine(EnableOptionsAfterTyping());
        }
    }
}
