using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    [Header("Data")]
    public TextAsset csvFile;

    [Header("UI References")]
    public GameObject dialogPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;

    private Dictionary<int, DialogEntry> dialogDict = new();
    public List<int> unlockedDialogIDs = new();

    private int currentID, startID, endID;
    private DialogEntry currentEntry;
    private DialogEntry lastOptionsEntry;

    private bool isDialogActive = false;
    private bool waitingForInput = false;
    private bool isTyping = false;

    void Awake()
    {
        LoadDialogFromCSV();
        dialogPanel.SetActive(false);
        HideAllOptions();
    }

    public void ToggleDialog(int start, int end)
    {
        if (isDialogActive)
        {
            CloseDialog();
        }
        else
        {
            startID = start;
            endID = end;
            currentID = startID;
            isDialogActive = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            dialogPanel.SetActive(true);
            ShowDialogue(currentID);
        }
    }

    public void AdvanceDialog()
    {
        if (!isDialogActive || !waitingForInput || isTyping) return;
        waitingForInput = false;
    }

    void LoadDialogFromCSV()
    {
        dialogDict.Clear();
        using StringReader reader = new(csvFile.text);
        while (reader.Peek() > -1)
        {
            string[] values = reader.ReadLine().Split(';');
            if (values.Length < 14 || !int.TryParse(values[0], out int id)) continue;

            DialogEntry entry = new DialogEntry
            {
                ID = id,
                Name = values[1],
                Text = values[2],
                HasOptions = values[3] == "1",
                OptionTexts = new string[3] { values[4], values[6], values[8] },
                OptionNextIDs = new int[3]
                {
                    int.TryParse(values[5], out int o1) ? o1 : -1,
                    int.TryParse(values[7], out int o2) ? o2 : -1,
                    int.TryParse(values[9], out int o3) ? o3 : -1
                },
                OptionWithRequirementText = values[10],
                OptionWithRequirementID = int.TryParse(values[11], out int o4) ? o4 : -1,
                NextLineID = int.TryParse(values[12], out int next) ? next : -1,
                RequiredID = int.TryParse(values[13], out int req) ? req : -1
            };

            dialogDict[id] = entry;
        }
    }

    void ShowDialogue(int id)
    {
        if (!dialogDict.TryGetValue(id, out DialogEntry entry)) return;

        // Normal dialogue flow
        currentID = id;
        currentEntry = entry;

        // Solo marcamos como desbloqueado si no lo estaba ya
        if (!unlockedDialogIDs.Contains(id))
        {
            unlockedDialogIDs.Add(id);
        }

        nameText.text = entry.Name;

        StopAllCoroutines();
        StartCoroutine(TypeText(entry.Text, () =>
        {
            if (entry.HasOptions)
            {
                lastOptionsEntry = entry;
                ShowOptions(entry);
            }
            else if (entry.NextLineID != -1)
            {
                StartCoroutine(WaitForInputThenContinue(entry.NextLineID));
            }
            else
            {
                ShowOptions(lastOptionsEntry);
            }
        }));
    }

    IEnumerator ShowOptionsAfterText(DialogEntry entry)
    {
        yield return new WaitForSeconds(0.5f);
        HideAllOptions();

        int buttonIndex = 0;

        // Mostrar opciones normales (siempre mostramos estas primero)
        for (int i = 0; i < 3; i++)
        {
            if (!string.IsNullOrEmpty(entry.OptionTexts[i]) && entry.OptionNextIDs[i] != -1)
            {
                SetupOption(buttonIndex++, entry.OptionTexts[i], entry.OptionNextIDs[i]);
            }
        }

        // Mostrar opción condicional solo si está desbloqueada
        if (!string.IsNullOrEmpty(entry.OptionWithRequirementText) &&
            entry.OptionWithRequirementID != -1 &&
            unlockedDialogIDs.Contains(entry.RequiredID))
        {
            SetupOption(buttonIndex++, entry.OptionWithRequirementText, entry.OptionWithRequirementID);
        }

        // Añadir opción "Adiós" si hay espacio
        if (buttonIndex < optionButtons.Length)
        {
            SetupOption(buttonIndex, "Adiós.", -1);
        }

        lastOptionsEntry = entry;
    }

    void OnOptionSelected(int nextID)
    {
        HideAllOptions();

        if (nextID != -1)
        {
            if (!dialogDict.TryGetValue(nextID, out DialogEntry nextEntry)) return;

            // Mostrar primero el texto de la opción seleccionada (pregunta del jugador)
            if (lastOptionsEntry != null)
            {
                string optionText = "";
                for (int i = 0; i < 3; i++)
                {
                    if (nextID == lastOptionsEntry.OptionNextIDs[i])
                    {
                        optionText = lastOptionsEntry.OptionTexts[i];
                        break;
                    }
                }

                if (string.IsNullOrEmpty(optionText) && nextID == lastOptionsEntry.OptionWithRequirementID)
                {
                    optionText = lastOptionsEntry.OptionWithRequirementText;
                }

                if (!string.IsNullOrEmpty(optionText))
                {
                    nameText.text = "Brisa";
                    StartCoroutine(TypeText(optionText, () =>
                    {
                        // Solo después de mostrar la pregunta, mostramos la respuesta del NPC
                        unlockedDialogIDs.Add(nextID);
                        ShowDialogue(nextID);
                    }));
                    return;
                }
            }

            // Si no era una pregunta del jugador, continuar normalmente
            unlockedDialogIDs.Add(nextID);
            ShowDialogue(nextID);
        }
        else
        {
            CloseDialog();
        }
    }

    IEnumerator TypeText(string text, System.Action onComplete)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.015f); // Ajusta este valor si el texto va demasiado rápido o lento
        }

        isTyping = false;
        onComplete?.Invoke();
    }

    void ShowOptions(DialogEntry entry)
    {
        StartCoroutine(ShowOptionsAfterText(entry));
    }


    void SetupOption(int index, string text, int nextID)
    {
        if (index >= optionButtons.Length) return;

        optionButtons[index].gameObject.SetActive(true);
        optionTexts[index].text = text;
        optionButtons[index].onClick.RemoveAllListeners();
        optionButtons[index].onClick.AddListener(() => OnOptionSelected(nextID));
    }

        IEnumerator WaitForInputThenContinue(int nextID)
    {
        waitingForInput = true;
        while (waitingForInput)
            yield return null;

        ShowDialogue(nextID);
    }

    void HideAllOptions()
    {
        foreach (var btn in optionButtons)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
        }
    }

    void CloseDialog()
    {
        StopAllCoroutines();
        HideAllOptions();
        dialogueText.text = "";
        nameText.text = "";
        dialogPanel.SetActive(false);
        isDialogActive = false;
        waitingForInput = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartDialog(int start, int end)
    {
        if (isDialogActive) return;

        startID = start;
        endID = end;
        currentID = startID;
        isDialogActive = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        dialogPanel.SetActive(true);
        ShowDialogue(currentID);
    }

    public void ForceCloseDialog()
    {
        if (!isDialogActive) return;

        StopAllCoroutines();
        HideAllOptions();
        dialogueText.text = "";
        nameText.text = "";
        dialogPanel.SetActive(false);
        isDialogActive = false;
        waitingForInput = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
