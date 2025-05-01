#region Bibliotecas
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

/* NOMBRE CLASE: Pick Up Items
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 13/03/2025
 * DESCRIPCI�N: Script que se encarga de recoger objetos del escenario.
 * VERSI�N: 1.1 
 * 1.1 ChangeOutline: Cambiar el color y el tama�o del outline.
 */

public class PickUpItems : MonoBehaviour
{
    #region Variables
    [Header("Configuraci�n")]
    [SerializeField] private OutlineDetector outlineDetector;

    private bool playerInRange = false;
    private Item itemScript;
    private ItemData itemData;
    private Player player;
    #endregion

    void Start()
    {
        player = FindObjectOfType<Player>();
        itemScript = GetComponent<Item>();


        player.PlayerInput.UIPanelActions.PickUpItem.performed += PickUpItem; // Suscribirse a la acci�n de recoger objetos.
    }

    private void OnDestroy()
    {
        player.PlayerInput.UIPanelActions.PickUpItem.performed -= PickUpItem; // Desuscribirse a la acci�n de recoger objetos.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            outlineDetector.HighlightForPickup(true);
            Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            outlineDetector.HighlightForPickup(false);
            Debug.Log("Player out of range");
        }
    }

    private void PickUpItem(InputAction.CallbackContext context)
    {
        if (playerInRange && itemScript != null)
        {
            itemScript.CollectItem();
            EventsManager.TriggerNormalEvent("PickUpItem");
        }
    }
}