using System.Collections;
using UnityEngine;

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
            itemScript.CollectItem();  // Llama al método de Item para recogerlo
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
