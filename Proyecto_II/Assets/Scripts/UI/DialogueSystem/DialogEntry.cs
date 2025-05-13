[System.Serializable]

/*
 * NOMBRE CLASE: DialogEntry
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 10/04/2025
 * DESCRIPCI�N: Script que representa una entrada de di�logo en el sistema de di�logos.
 * VersI�N: 1.0 Sistema de di�logos inicial.
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