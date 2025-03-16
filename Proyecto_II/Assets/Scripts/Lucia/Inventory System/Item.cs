using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int itemQuantity = 1;
    private bool isCollected = false;

    public void CollectItem()
    {
        if (isCollected) return;  // Evita recogerlo m�s de una vez
        isCollected = true;

        InventoryManager.Instance.AddItem(itemData, itemQuantity);
        gameObject.SetActive(false);  // Desactiva el objeto
        Destroy(gameObject, 0.1f);  // Lo destruye despu�s de un peque�o delay
    }
}
