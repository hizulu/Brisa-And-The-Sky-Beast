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
    private HashSet<int> completedDialogIDs = new HashSet<int>();

    private int currentID, startID, endID;
    private DialogEntry currentEntry;
    private DialogEntry lastOptionsEntry;

    private bool isDialogActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private Coroutine waitForInputCoroutine;

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
        else
        {
            completedDialogIDs.Add(currentID);
            CloseDialog();
        }
    }

    void ShowDialogue(int id)
    {
        if (!dialogDict.TryGetValue(id, out currentEntry)) return;

        currentID = id;
        unlockedDialogIDs.Add(id);
        nameText.text = currentEntry.Name;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(currentEntry.Text, () =>
        {
            if (currentEntry.HasOptions)
            {
                lastOptionsEntry = currentEntry;
                ShowOptions(currentEntry); // Mostrar opciones al terminar de escribir
            }
            else
            {
                // Iniciar espera para la entrada del jugador
                waitForInputCoroutine = StartCoroutine(WaitForInput());
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

    IEnumerator WaitForInput()
    {
        bool inputReceived = false;
        while (!inputReceived && isDialogActive)
        {
            if (Input.GetKeyDown(KeyCode.E)) // Espera por la tecla 'E'
            {
                inputReceived = true;
                AdvanceDialog(); // Avanza el diálogo después de que se presiona 'E'
            }
            yield return null;
        }
    }

    void ShowOptions(DialogEntry entry)
    {
        StartCoroutine(ShowOptionsWithDelay(entry));
    }

    IEnumerator ShowOptionsWithDelay(DialogEntry entry)
    {
        yield return new WaitForSeconds(0.1f);
        HideAllOptions();

        int buttonIndex = 0;

        // Mostrar opciones normales
        for (int i = 0; i < 3 && buttonIndex < optionButtons.Length; i++)
        {
            if (!string.IsNullOrEmpty(entry.OptionTexts[i]) && entry.OptionNextIDs[i] != -1)
            {
                SetupOption(buttonIndex++, entry.OptionTexts[i], entry.OptionNextIDs[i]);
            }
        }

        // Mostrar opción condicional si cumple requisitos
        if (buttonIndex < optionButtons.Length &&
            !string.IsNullOrEmpty(entry.OptionWithRequirementText) &&
            entry.OptionWithRequirementID != -1 &&
            unlockedDialogIDs.Contains(entry.RequiredID) &&
            !completedDialogIDs.Contains(entry.OptionWithRequirementID))
        {
            SetupOption(buttonIndex++, entry.OptionWithRequirementText, entry.OptionWithRequirementID);
        }

        // Mostrar siempre la opción de "Adiós"
        if (buttonIndex < optionButtons.Length)
        {
            SetupOption(buttonIndex, "Adiós.", -1);
        }
    }

    void SetupOption(int index, string text, int nextID)
    {
        if (index < 0 || index >= optionButtons.Length || optionButtons[index] == null) return;

        optionButtons[index].gameObject.SetActive(true);
        optionTexts[index].text = text;
        optionButtons[index].onClick.RemoveAllListeners();
        optionButtons[index].onClick.AddListener(() => OnOptionSelected(nextID));
    }

    void OnOptionSelected(int nextID)
    {
        if (!isDialogActive) return;

        HideAllOptions();

        if (nextID == -1)
        {
            CloseDialog();
            return;
        }

        if (!dialogDict.TryGetValue(nextID, out DialogEntry nextEntry)) return;

        // Marcar como completado si es opción condicional
        if (lastOptionsEntry != null && lastOptionsEntry.OptionWithRequirementID == nextID)
            completedDialogIDs.Add(nextID);

        // Mostrar directamente el diálogo de la siguiente ID
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

        HideAllOptions();
        dialogueText.text = "";
        nameText.text = "";
        dialogPanel.SetActive(false);
        isDialogActive = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ForceCloseDialog()
    {
        CloseDialog();
    }
}
