using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 11/05/2025
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;
}
