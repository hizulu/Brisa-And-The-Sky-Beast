using System.Collections.Generic;
using UnityEngine;

public class DialogueDatabase : MonoBehaviour
{
    public TextAsset csvFile;
    private Dictionary<int, DialogueLine> dialogueLines = new();

    public void LoadCSV()
    {
        Debug.Log("Loading CSV file: " + csvFile.name);
        dialogueLines.Clear();
        var lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            var data = lines[i].Split(';');
            if (data.Length <= 10)
                continue;
            DialogueLine line = new DialogueLine
            {
                ID = TryParseInt(data[0]),
                name = data[1],
                initialText = data[2],
                requirementID = string.IsNullOrEmpty(data[9]) ? -1 : TryParseInt(data[9]),
                npcID = string.IsNullOrEmpty(data[10]) ? -1 : TryParseInt(data[10])
            };
            for (int j = 0; j < 3; j++)
            {
                string optionText = data[3 + j * 2];
                string nextIdStr = data[4 + j * 2];
                if (!string.IsNullOrEmpty(optionText) && !string.IsNullOrEmpty(nextIdStr))
                {
                    int nextID = TryParseInt(nextIdStr);
                    if (nextID != -1)
                    {
                        line.options.Add(new DialogueOption
                        {
                            optionText = optionText,
                            nextID = nextID
                        });
                        Debug.Log($"Option {j + 1}: {optionText}, NextID: {nextID}");
                    }
                }
            }
            dialogueLines[line.ID] = line;
            Debug.Log($"Loaded line ID: {line.ID}, Name: {line.name}, Initial Text: {line.initialText}");
        }
    }

    private int TryParseInt(string value)
    {
        return int.TryParse(value, out int result) ? result : -1;
    }

    public DialogueLine GetLine(int id)
    {
        return dialogueLines.ContainsKey(id) ? dialogueLines[id] : null;
    }

    public IEnumerable<DialogueLine> GetAllLines()
    {
        return dialogueLines.Values;
    }
}
