using UnityEngine;

[CreateAssetMenu(fileName = "NewAppearance", menuName = "AppearanceChangePanel/Appearance")]

/* NOMBRE CLASE: Appearance Change Data
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 22/03/2025
 * DESCRIPCI�N: Script que se encarga de almacenar la informaci�n del cambio de apariencia.
 * VERSI�N: 1.0 appearanceID, appearanceName, appearanceIcon, appearanceDescription.
 */

public class AppearanceChangeData : ScriptableObject
{
    public string appearanceID;
    public string appearanceName;
    public Sprite appearanceIcon;
    public string appearanceDescription;
    public Material appearanceMaterial;
}