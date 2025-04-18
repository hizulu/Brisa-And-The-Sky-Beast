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

    private Dictionary<int, DialogEntry> dialogDict = new Dictionary<int, DialogEntry>();
    private HashSet<int> unlockedDialogIDs = new HashSet<int>();
    private HashSet<int> seenDialogIDs = new HashSet<int>();

    private int currentID, startID, endID;
    private DialogEntry currentEntry;
    private DialogEntry lastOptionsEntry;

    private bool isDialogActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Awake()
    {
        LoadDialogFromCSV();
        dialogPanel.SetActive(false);
        HideAllOptions();
    }

    void LoadDialogFromCSV()
    {
        dialogDict.Clear();
        string[] lines = csvFile.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');
            if (values.Length < 14 || !int.TryParse(values[0], out int id)) continue;

            dialogDict[id] = new DialogEntry
            {
                ID = id,
                Name = values[1],
                Text = values[2].Replace("\\n", "\n"),
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
        }
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

    public void AdvanceDialog()
    {
        if (!isDialogActive || isTyping) return;

        if (currentEntry.NextLineID != -1)
        {
            ShowDialogue(currentEntry.NextLineID);
        }
        else if (lastOptionsEntry != null)
        {
            //Debug.Log("Volviendo a mostrar opciones anteriores al finalizar diálogo");
            ShowOptions(lastOptionsEntry);
        }
        else
        {
            CloseDialog();
        }
    }

    void ShowDialogue(int id)
    {
        if (!dialogDict.TryGetValue(id, out currentEntry))
        {
            //Debug.LogError($"Diálogo con ID {id} no encontrado");
            return;
        }

        currentID = id;

        // Registrar que hemos visto este diálogo
        if (!seenDialogIDs.Contains(id))
        {
            //Debug.Log($"Nuevo diálogo visto - ID: {id}, Texto: {currentEntry.Text}");
            seenDialogIDs.Add(id);
        }

        // Si este diálogo tiene un RequiredID, desbloquearlo
        if (currentEntry.RequiredID != -1 && !unlockedDialogIDs.Contains(currentEntry.RequiredID))
        {
            //Debug.Log($"Desbloqueando diálogo requerido - ID: {currentEntry.RequiredID}");
            unlockedDialogIDs.Add(currentEntry.RequiredID);
        }

        nameText.text = currentEntry.Name;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(currentEntry.Text, () =>
        {
            if (currentEntry.HasOptions)
            {
                //Debug.Log($"Mostrando opciones para diálogo ID: {id}");
                lastOptionsEntry = currentEntry;
                ShowOptions(currentEntry);
            }
            else if (currentEntry.NextLineID == -1 && lastOptionsEntry != null)
            {
                //Debug.Log("No hay siguiente línea, volviendo a opciones anteriores");
                ShowOptions(lastOptionsEntry);
            }
        }));
    }

    IEnumerator TypeText(string text, System.Action onComplete)
    {
        isTyping = true;
        dialogueText.text = "";
        float typingSpeed = 0.015f;
        float timer = 0f;
        int visibleChars = 0;

        while (visibleChars < text.Length && isDialogActive)
        {
            timer += Time.deltaTime;
            if (timer >= typingSpeed)
            {
                timer = 0f;
                visibleChars++;
                dialogueText.text = text.Substring(0, visibleChars);
            }
            yield return null;
        }

        dialogueText.text = text;
        isTyping = false;
        onComplete?.Invoke();
    }

    void ShowOptions(DialogEntry entry)
    {
        //Debug.Log($"Mostrando opciones para entrada ID: {entry.ID}");
        //Debug.Log($"Diálogos vistos: {string.Join(",", seenDialogIDs)}");
        //Debug.Log($"Diálogos desbloqueados: {string.Join(",", unlockedDialogIDs)}");

        HideAllOptions();

        int buttonIndex = 0;

        // Mostrar opciones normales
        for (int i = 0; i < 3 && buttonIndex < optionButtons.Length; i++)
        {
            if (!string.IsNullOrEmpty(entry.OptionTexts[i]) && entry.OptionNextIDs[i] != -1)
            {
                //Debug.Log($"Mostrando opción normal {i}: {entry.OptionTexts[i]} -> ID: {entry.OptionNextIDs[i]}");
                SetupOption(buttonIndex++, entry.OptionTexts[i], entry.OptionNextIDs[i]);
            }
        }

        // Mostrar opción condicional si:
        // 1. Tiene texto definido
        // 2. Tiene un ID válido
        // 3. El diálogo requerido ha sido visto (no solo desbloqueado)
        if (buttonIndex < optionButtons.Length &&
            !string.IsNullOrEmpty(entry.OptionWithRequirementText) &&
            entry.OptionWithRequirementID != -1)
        {
            if (seenDialogIDs.Contains(entry.RequiredID))
            {
                //Debug.Log($"Mostrando opción condicional: {entry.OptionWithRequirementText} -> ID: {entry.OptionWithRequirementID} (Requiere ID: {entry.RequiredID})");
                SetupOption(buttonIndex++, entry.OptionWithRequirementText, entry.OptionWithRequirementID);
            }
            else
            {
                //Debug.Log($"Opcion condicional requerida no vista aún: {entry.RequiredID}");
            }
        }

        // Opción "Adiós" siempre disponible
        if (buttonIndex < optionButtons.Length)
        {
            //Debug.Log("Mostrando opción 'Adiós'");
            SetupOption(buttonIndex, "Adiós.", -1);
        }
    }

    void SetupOption(int index, string text, int nextID)
    {
        if (index < 0 || index >= optionButtons.Length || optionButtons[index] == null)
        {
            //Debug.LogError($"Índice de opción inválido: {index}");
            return;
        }

        optionButtons[index].gameObject.SetActive(true);
        optionTexts[index].text = text;
        optionButtons[index].onClick.RemoveAllListeners();
        optionButtons[index].onClick.AddListener(() => OnOptionSelected(nextID));

        //Debug.Log($"Configurada opción {index}: {text} -> {nextID}");
    }

    void OnOptionSelected(int nextID)
    {
        if (!isDialogActive) return;

        //Debug.Log($"Opción seleccionada, siguiente ID: {nextID}");

        HideAllOptions();

        if (nextID == -1)
        {
            //Debug.Log("Cerrando diálogo");
            CloseDialog();
            return;
        }

        if (!dialogDict.TryGetValue(nextID, out DialogEntry nextEntry))
        {
            //Debug.LogError($"Siguiente diálogo con ID {nextID} no encontrado");
            return;
        }

        ShowDialogue(nextID);
    }

    void HideAllOptions()
    {
        foreach (var btn in optionButtons)
        {
            if (btn != null)
            {
                btn.gameObject.SetActive(false);
                btn.onClick.RemoveAllListeners();
            }
        }
    }

    void CloseDialog()
    {
        if (!isDialogActive) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        //Debug.Log("Cerrando diálogo");

        HideAllOptions();
        dialogueText.text = "";
        nameText.text = "";
        dialogPanel.SetActive(false);
        isDialogActive = false;

        EventsManager.TriggerNormalEvent("ResetCameraDialogue");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ForceCloseDialog()
    {
        CloseDialog();
    }
}