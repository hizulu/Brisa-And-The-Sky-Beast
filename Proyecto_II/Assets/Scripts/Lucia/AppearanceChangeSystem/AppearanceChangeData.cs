using UnityEngine;

[CreateAssetMenu(fileName = "NewAppearance", menuName = "AppearanceChangePanel/Appearance")]

/* NOMBRE CLASE: Appearance Change Data
 * AUTOR: Lucía García López
 * FECHA: 22/03/2025
 * DESCRIPCIÓN: Script que se encarga de almacenar la información del cambio de apariencia.
 * VERSIÓN: 1.0 appearanceID, appearanceName, appearanceIcon, appearanceDescription.
 */

public class AppearanceChangeData : ScriptableObject
{
    public string appearanceID;
    public string appearanceName;
    public Sprite appearanceIcon;
    public string appearanceDescription;
    public Material appearanceMaterial;
}