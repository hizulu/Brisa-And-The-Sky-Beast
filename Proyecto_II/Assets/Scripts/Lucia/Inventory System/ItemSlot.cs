using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TMP_Text itemQuantityText;

    private ItemData itemData;
    private int itemQuantity;

    public void SetItem(ItemData newItemData, int quantity)
    {
        itemData = newItemData;
        itemQuantity = quantity;

        itemIconImage.sprite = itemData.itemIcon;
        itemQuantityText.text = quantity.ToString();
        itemQuantityText.enabled = true;
    }

    public bool IsEmpty()
    {
        return itemData == null; // Si no tiene un ítem asignado, está vacío
    }


    public void UpdateQuantity(int newQuantity)
    {
        itemQuantity = newQuantity;
        itemQuantityText.text = newQuantity.ToString(); // Actualizar el texto
    }

    public ItemData GetItemData()
    {
        return itemData;
    }

    public bool HasItem()
    {
        return itemData != null;
    }
}
