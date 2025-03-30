#region Bibliotecas
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

/* NOMBRE CLASE: Pick Up Objects
 * AUTOR: Lucía García López
 * FECHA: 13/03/2025
 * DESCRIPCIÓN: Script que se encarga de recoger objetos del escenario.
 * VERSIÓN: 1.0 
 */

public class PickUpObjects : MonoBehaviour
{
    public GameObject adviseObject;
    private bool playerInRange = false;
    private Item itemScript;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();

        if (adviseObject != null)
            adviseObject.SetActive(false);

        itemScript = GetComponent<Item>();  // Obtiene la referencia del Item asociado

        player.PlayerInput.PlayerActions.Interact.performed += PickUpItem; // Suscribirse a la acción de recoger objetos.
    }

    //void Update()
    //{
    //    if (playerInRange && Input.GetKeyDown(KeyCode.E) && itemScript != null)
    //    {
    //        itemScript.CollectItem();  // Llama al método de Item para recogerlo
    //        adviseObject.SetActive(false);
    //    }
    //}

    private void OnDestroy()
    {
        player.PlayerInput.PlayerActions.Interact.performed -= PickUpItem; // Desuscribirse a la acción de recoger objetos.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (adviseObject != null)
                adviseObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (adviseObject != null)
                adviseObject.SetActive(false);
        }
    }

    private void PickUpItem(InputAction.CallbackContext context)
    {
        if (playerInRange && itemScript != null)
        {
            itemScript.CollectItem();
            adviseObject.SetActive(false);

            if (itemScript.itemData.itemName == "Palo")
                player.PaloRecogido();
        }
    }
}
