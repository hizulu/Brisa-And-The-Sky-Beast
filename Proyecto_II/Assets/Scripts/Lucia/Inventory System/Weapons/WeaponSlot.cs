#region Bibliotecas
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
#endregion

/* NOMBRE CLASE: Weapon Slot
 * AUTOR: Lucía García López
 * FECHA: 05/04/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar el slot de las armas
 * VERSIÓN: 1.0
 */

public class WeaponSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image weaponIconImage;

    private WeaponData weaponData;

    public bool weaponSelected = false;
    private ObjectsImageAndDescription objectsImageAndDescription;


    private void Start()
    {
        objectsImageAndDescription = FindObjectOfType<ObjectsImageAndDescription>();

        if (IsEmpty())
        {
            gameObject.SetActive(false);
        }
    }

    // Método para asignar un arma al slot
    public void SetWeapon(WeaponData newWeaponData)
    {
        // Si ya hay un arma, limpia el icono
        if (weaponData != null)
        {
            // Aquí podrías agregar lógica para manejar el reemplazo de las armas, si es necesario
            Debug.Log("Reemplazando el arma anterior con la nueva");
        }

        weaponData = newWeaponData;

        if (weaponData != null && weaponIconImage != null)
        {
            weaponIconImage.sprite = weaponData.weaponVerticalIcon;
            gameObject.SetActive(true); // Asegura que el slot se activa
        }
    }


    public bool IsEmpty()
    {
        return weaponData == null;
    }

    public WeaponData GetWeaponData()
    {
        return weaponData;
    }

    public bool HasWeapon()
    {
        return weaponData != null;
    }

    private void SelectWeapon()
    {
        weaponSelected = !weaponSelected;

        if (weaponSelected && objectsImageAndDescription != null)
        {
            objectsImageAndDescription.WeaponSetImageAndDescription(weaponData);
        }
        else
        {
            objectsImageAndDescription.ClearDisplay(); //Si se deselecciona, ocultar la descripción
        }
    }

    public void DeselectWeapon()
    {
        weaponSelected = false;
        objectsImageAndDescription.ClearDisplay(); // Limpiar la descripción
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !IsEmpty())
        {
            SelectWeapon();
        }
    }
}
