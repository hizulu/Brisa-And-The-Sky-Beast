#region Bibliotecas
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

/* NOMBRE CLASE: Pick Up Objects
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 13/03/2025
 * DESCRIPCI�N: Script que se encarga de recoger objetos del escenario.
 * VERSI�N: 1.0 
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

        player.PlayerInput.PlayerActions.Interact.performed += PickUpItem; // Suscribirse a la acci�n de recoger objetos.
    }

    //void Update()
    //{
    //    if (playerInRange && Input.GetKeyDown(KeyCode.E) && itemScript != null)
    //    {
    //        itemScript.CollectItem();  // Llama al m�todo de Item para recogerlo
    //        adviseObject.SetActive(false);
    //    }
    //}

    private void OnDestroy()
    {
        player.PlayerInput.PlayerActions.Interact.performed -= PickUpItem; // Desuscribirse a la acci�n de recoger objetos.
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
