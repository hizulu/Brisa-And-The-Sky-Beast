[System.Serializable]
public class DialogEntry
{
    public int ID;
    public string Name;
    public string Text;
    public bool HasOptions;

    public string[] OptionTexts = new string[3];
    public int[] OptionNextIDs = new int[3];

    public string OptionWithRequirementText;
    public int OptionWithRequirementID;
    public int RequiredID;

    public int NextLineID;
}
