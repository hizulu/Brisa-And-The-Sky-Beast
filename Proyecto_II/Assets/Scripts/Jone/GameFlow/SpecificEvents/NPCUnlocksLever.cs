using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUnlocksLever : MonoBehaviour
{
    public int requiredID;
    private bool isUnlocked = false;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private TempleDoorMover templeDoorMover;

    void UnlockLever()
    {
        if (!isUnlocked)
        {
            templeDoorMover.OnUnlockLever();
            isUnlocked = true;
            Debug.Log("Lever unlocked!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogManager.DialogIDRead(requiredID))
            {
                UnlockLever();
                Debug.Log("Dialog ID met. Lever unlocked.");
            }
            else
            {
                Debug.Log("Dialog ID not met. Lever remains locked.");
            }
        }
    }
}
