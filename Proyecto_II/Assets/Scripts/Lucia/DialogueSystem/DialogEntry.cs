[System.Serializable]

/*
 * NOMBRE CLASE: DialogEntry
 * AUTOR: Lucía García López
 * FECHA: 10/04/2025
 * DESCRIPCIÓN: Script que representa una entrada de diálogo en el sistema de diálogos.
 * VersIÓN: 1.0 Sistema de diálogos inicial.
 */

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