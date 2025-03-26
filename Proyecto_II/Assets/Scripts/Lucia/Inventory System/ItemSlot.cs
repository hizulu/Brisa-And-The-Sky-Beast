#region Bibliotecas
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
#endregion

/* NOMBRE CLASE: Item Slot
 * AUTOR: Lucía García López
 * FECHA: 13/03/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar los slots de los ítems en el inventario.
 * VERSIÓN: 1.1 
 */

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TMP_Text itemQuantityText;

    private ItemData itemData;
    private int itemQuantity;

    public bool itemSelected = false;
    private ItemImageAndDescription itemImageAndDescription;

    private void Start()
    {
        itemImageAndDescription = FindObjectOfType<ItemImageAndDescription>();

        if (IsEmpty())
        {
            gameObject.SetActive(false);
        }
    }

    // Método para asignar un ítem a un slot
    public void SetItem(ItemData newItemData, int quantity)
    {
        itemData = newItemData;
        itemQuantity = quantity;

        itemIconImage.sprite = itemData.itemIcon;
        itemQuantityText.text = quantity.ToString();
        itemQuantityText.enabled = true;
        gameObject.SetActive(true); // Se activa cuando recibe un ítem
    }

    public bool IsEmpty()
    {
        return itemData == null; // Si no tiene un ítem asignado, está vacío
    }

    // Método para actualizar la cantidad de un ítem en un slot
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

    // Método para detectar el clic en un slot
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !IsEmpty())
        {
            SelectItem();
        }
    }

    private void SelectItem()
    {
        itemSelected = !itemSelected;

        if (itemSelected && itemImageAndDescription != null)
        {
            itemImageAndDescription.SetImageAndDescription(itemData);
        }
        else
        {
            itemImageAndDescription.ClearDisplay(); //Si se deselecciona, ocultar la descripción
        }
    }

}
