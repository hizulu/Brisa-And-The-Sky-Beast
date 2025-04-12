using System.Collections.Generic;

[System.Serializable]
public class DialogueLine
{
    public int ID;
    public string name;
    public string initialText;
    public List<DialogueOption> options = new();
    public int requirementID;
    public int npcID;
    public int nextLineID;
}