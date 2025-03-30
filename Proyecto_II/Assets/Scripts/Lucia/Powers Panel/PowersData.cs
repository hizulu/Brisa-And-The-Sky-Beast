using UnityEngine;

[CreateAssetMenu(fileName = "NewPower", menuName = "Inventory/Power")]

/* NOMBRE CLASE: Powers Data
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 26/03/2025
 * DESCRIPCI�N: Script que se encarga de almacenar la informaci�n de los poderes
 * VERSI�N: 1.0 powerID, powerIcon, powerBrisaName, powerBestiaName, powerBrisaDescription y powerBestiaDescription.
 */

public class PowersData : ScriptableObject
{
    public string powerID;
    public Sprite powerIcon;
    public string powerBrisaName;
    public string powerBestiaName;
    public string powerBrisaDescription;
    public string powerBestiaDescription;
}