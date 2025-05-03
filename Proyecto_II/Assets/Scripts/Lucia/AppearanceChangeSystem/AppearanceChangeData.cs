using UnityEngine;

[CreateAssetMenu(fileName = "NewAppearance", menuName = "Inventory/Appearance")]

/* NOMBRE CLASE: Appearance Change Data
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 22/03/2025
 * DESCRIPCI�N: Script que se encarga de almacenar la informaci�n del cambio de apariencia.
 * VERSI�N: 1.0 appearanceID, appearanceName, appearanceIcon, appearanceDescription, appearanceMaterial
 * 1.1 objectsNeeded, isUnlocked.
 * 1.2 eyebrowsBaseMap.
 */

public class AppearanceChangeData : ScriptableObject
{
    public string appearanceID;
    public string appearanceName;
    public Sprite appearanceIcon;
    public string appearanceDescription;
    public Texture2D bodyBaseMap;
    public Texture2D eyebrowsBaseMap;
    public Texture2D eyesBaseMap;
    public string objectsNeeded;
    public int toUnlockQuantity;
    public GameObject objectsNeededPrefab;
    public bool isUnlocked;
}