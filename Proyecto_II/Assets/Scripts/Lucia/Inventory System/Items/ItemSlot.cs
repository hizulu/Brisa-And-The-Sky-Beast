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
 * VERSIÓN: 1.0 SetWeapon, IsEmpty, UpdateQuantity, GetItemData, HasItem, SelectWeapon, OnPointerClick.
 * 1.1 Cambios en UpdateQuantity, + OnValidate.
 */

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TMP_Text itemQuantityText;

    private ItemData itemData;
    [SerializeField] private int itemQuantity;

    public bool itemSelected = false;
    private ObjectsImageAndDescription itemImageAndDescription;


    private void Start()
    {
        itemImageAndDescription = FindObjectOfType<ObjectsImageAndDescription>();

        if (IsEmpty())
        {
            gameObject.SetActive(false);
        }
    }

    //Solo para pruebas
    // Método llamado cuando un valor cambia en el Inspector
    //public void OnValidate()
    //{
    //    if (itemData != null && InventoryManager.Instance.inventory.ContainsKey(itemData))
    //    {
    //        // Actualizar la cantidad en el diccionario cuando cambie el valor en el Inspector
    //        InventoryManager.Instance.inventory[itemData] = itemQuantity;

    //        // Asegúrate de actualizar la visibilidad del slot
    //        InventoryManager.Instance.UpdateItemSlotVisibility(itemData);
    //    }
    //}

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
        return itemData == null;
    }

    // Método para actualizar la cantidad de un ítem en un slot
    public void UpdateQuantity(int newQuantity)
    {
        itemQuantity = newQuantity;
        itemQuantityText.text = newQuantity.ToString(); // Actualizamos el texto

        // Si la cantidad es 0 o menor, ocultamos el texto
        if (newQuantity <= 0)
        {            
            itemQuantityText.enabled = false; // Desactivamos el texto
            itemSelected = false; // Deseleccionamos el ítem
        }
        else
        {
            itemQuantityText.enabled = true; // Mostramos el texto si la cantidad es mayor que 0
        }
    }

    // Método para obtener la información de un ítem
    public ItemData GetItemData()
    {
        return itemData;
    }

    // Método para comprobar si un slot tiene un ítem
    public bool HasItem()
    {
        return itemData != null;
    }

    // Método para seleccionar un ítem
    private void SelectItem()
    {
        itemSelected = !itemSelected;

        if (itemSelected && itemImageAndDescription != null)
        {
            itemImageAndDescription.ItemSetImageAndDescription(itemData);
        }
        else
        {
            itemImageAndDescription.ClearDisplay(); //Si se deselecciona, ocultar la descripción
        }
    }

    // Método para deseleccionar un ítem
    public void DeselectItem()
    {
        itemSelected = false;
        itemImageAndDescription.ClearDisplay(); // Limpiar la descripción
    }


    // Método para detectar el clic en un slot
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !IsEmpty())
        {
            SelectItem();
        }
    }
}
