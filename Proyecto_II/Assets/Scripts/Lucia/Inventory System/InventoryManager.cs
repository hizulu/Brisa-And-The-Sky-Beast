using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    private bool inventoryEnabled=false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            inventoryEnabled = true;
        }

        if (inventoryEnabled)
        {
            inventoryMenu.SetActive(true);
            Time.timeScale = 0;
            inventoryEnabled = true;
        }
        else
        {
            inventoryMenu.SetActive(false);
            Time.timeScale = 1;
            inventoryEnabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryEnabled)
        {
            inventoryEnabled = false;
            inventoryMenu.SetActive(false);
        }
    }
}
