using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int itemQuantity = 1; // Cantidad del objeto

    private bool playerInRange = false; // Detecta si el jugador est� cerca

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) // Recoger con 'E'
        {
            CollectItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // El jugador est� dentro del �rea
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // El jugador sale del �rea
        }
    }

    private void CollectItem()
    {
        // Asegurar que el objeto sigue activo antes de sumarlo al inventario
        gameObject.SetActive(true);

        // Agregar al inventario
        InventoryManager.Instance.AddItem(itemData, itemQuantity);

        // Destruir el objeto tras recogerlo (con peque�o retraso para evitar errores)
        Destroy(gameObject, 0.05f);
    }
}
