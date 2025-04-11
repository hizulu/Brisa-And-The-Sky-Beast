[System.Serializable]
public class DialogEntry
{
    public int ID;
    public string Name;
    public string Text;
    public bool HasOptions;
    public string[] OptionTexts;
    public int[] OptionNextIDs;
    public string OptionWithRequirementText;
    public int OptionWithRequirementID;
    public int NextLineID;
    public int RequiredID;
}