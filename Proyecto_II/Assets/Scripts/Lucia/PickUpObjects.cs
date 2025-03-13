using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PickUpObjects : MonoBehaviour
{
    public GameObject item;
    public GameObject adviseObject;

    void Start()
    {
        adviseObject.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            adviseObject.SetActive(true);
            Debug.Log("En el area del objeto");

            if (Input.GetKeyDown(KeyCode.E))
            {
                item.gameObject.SetActive(false);
                adviseObject.gameObject.SetActive(false);
                Debug.Log("Objeto cogido");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        adviseObject.gameObject.SetActive(false);
    }
}
