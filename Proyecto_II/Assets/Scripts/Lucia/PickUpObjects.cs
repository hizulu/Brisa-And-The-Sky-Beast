#region Bibliotecas
using System.Collections;
using UnityEngine;
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

    void Start()
    {
        if (adviseObject != null)
            adviseObject.SetActive(false);

        itemScript = GetComponent<Item>();  // Obtiene la referencia del Item asociado
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && itemScript != null)
        {
            itemScript.CollectItem();  // Llama al m�todo de Item para recogerlo
            adviseObject.SetActive(false);
        }
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
}
