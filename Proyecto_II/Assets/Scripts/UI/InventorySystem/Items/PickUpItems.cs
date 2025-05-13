#region Bibliotecas
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

/* NOMBRE CLASE: Pick Up Items
 * AUTOR: Lucía García López
 * FECHA: 13/03/2025
 * DESCRIPCIÓN: Script que se encarga de recoger objetos del escenario.
 * VERSIÓN: 1.1 
 * 1.1 ChangeOutline: Cambiar el color y el tamaño del outline.
 */

public class PickUpItems : MonoBehaviour
{
    #region Variables
    [Header("Configuración")]
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


        player.PlayerInput.UIPanelActions.PickUpItem.performed += PickUpItem; // Suscribirse a la acción de recoger objetos.
    }

    private void OnDestroy()
    {
        player.PlayerInput.UIPanelActions.PickUpItem.performed -= PickUpItem; // Desuscribirse a la acción de recoger objetos.
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