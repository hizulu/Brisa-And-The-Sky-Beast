using UnityEngine;

[CreateAssetMenu(fileName = "NewPower", menuName = "Inventory/Power")]

/* NOMBRE CLASE: Powers Data
 * AUTOR: Lucía García López
 * FECHA: 26/03/2025
 * DESCRIPCIÓN: Script que se encarga de almacenar la información de los poderes
 * VERSIÓN: 1.0 powerID, powerIcon, powerBrisaName, powerBestiaName, powerBrisaDescription y powerBestiaDescription.
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