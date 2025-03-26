#region Bibliotecas
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
#endregion

/* NOMBRE CLASE: Item Slot
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 13/03/2025
 * DESCRIPCI�N: Script que se encarga de gestionar los slots de los �tems en el inventario.
 * VERSI�N: 1.1 
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

    // M�todo para asignar un �tem a un slot
    public void SetItem(ItemData newItemData, int quantity)
    {
        itemData = newItemData;
        itemQuantity = quantity;

        itemIconImage.sprite = itemData.itemIcon;
        itemQuantityText.text = quantity.ToString();
        itemQuantityText.enabled = true;
        gameObject.SetActive(true); // Se activa cuando recibe un �tem
    }

    public bool IsEmpty()
    {
        return itemData == null; // Si no tiene un �tem asignado, est� vac�o
    }

    // M�todo para actualizar la cantidad de un �tem en un slot
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

    // M�todo para detectar el clic en un slot
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
            itemImageAndDescription.ClearDisplay(); //Si se deselecciona, ocultar la descripci�n
        }
    }

}
