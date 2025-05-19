using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Inventory/Weapon Database")]
public class WeaponDatabase : ScriptableObject
{
    public List<WeaponData> allWeapons;

    public WeaponData GetWeaponByID(string weaponID)
    {
        return allWeapons.Find(w => w.weaponID == weaponID);
    }
}