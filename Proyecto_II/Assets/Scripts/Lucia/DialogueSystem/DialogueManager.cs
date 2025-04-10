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

        currentID = id;
        currentEntry = entry;
        unlockedDialogIDs.Add(id);  // Mark dialog as unlocked
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
                ShowOptions(lastOptionsEntry); // Show previous options
            }
        }));
    }

    IEnumerator ShowOptionsAfterText(DialogEntry entry)
    {
        yield return new WaitForSeconds(0.5f);
        HideAllOptions();

        int buttonIndex = 0;

        // Show normal options
        for (int i = 0; i < 3; i++)
        {
            if (!string.IsNullOrEmpty(entry.OptionTexts[i]) && entry.OptionNextIDs[i] != -1)
            {
                SetupOption(buttonIndex++, entry.OptionTexts[i], entry.OptionNextIDs[i]);
            }
        }

        // Show requirement-based option
        if (!string.IsNullOrEmpty(entry.OptionWithRequirementText))
        {
            if (unlockedDialogIDs.Contains(entry.RequiredID))
            {
                SetupOption(buttonIndex++, entry.OptionWithRequirementText, entry.OptionWithRequirementID);
            }
        }

        // Add "Goodbye" option if space allows
        if (buttonIndex < optionButtons.Length)
        {
            SetupOption(buttonIndex, "Goodbye", -1);
        }
    }

    IEnumerator TypeText(string text, System.Action onComplete)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.015f);
        }

        isTyping = false;
        onComplete?.Invoke();
    }

    IEnumerator WaitForInputThenContinue(int nextID)
    {
        waitingForInput = true;
        while (waitingForInput)
            yield return null;

        ShowDialogue(nextID);
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

    void OnOptionSelected(int nextID)
    {
        HideAllOptions();

        if (nextID != -1)
        {
            DialogEntry nextEntry = dialogDict[nextID];

            if (nextEntry.RequiredID != -1 && !unlockedDialogIDs.Contains(nextEntry.RequiredID))
            {
                Debug.Log($"Option with RequiredID {nextEntry.RequiredID} is not unlocked. Cannot proceed.");
                return;
            }

            unlockedDialogIDs.Add(nextID);

            if (!nextEntry.HasOptions && nextEntry.NextLineID == -1)
            {
                StartCoroutine(TypeText(nextEntry.Text, () =>
                {
                    ShowOptions(lastOptionsEntry); // Show original options
                }));
            }
            else
            {
                ShowDialogue(nextID);
            }
        }
        else
        {
            CloseDialog();
        }
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
