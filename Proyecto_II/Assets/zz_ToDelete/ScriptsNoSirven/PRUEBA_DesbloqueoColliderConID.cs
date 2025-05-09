using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRUEBA_DesbloqueoColliderConID : MonoBehaviour
{
    public GameObject colliderToUnlock;
    public int requiredID;
    private bool isUnlocked = false;
    [SerializeField] private DialogManager dialogManager;

    void Start()
    {
        if (colliderToUnlock == null)
        {
            Debug.LogError("Collider to unlock is not assigned.");
        }
    }

    void UnlockCollider()
    {
        if (!isUnlocked)
        {
            colliderToUnlock.GetComponent<Collider>().enabled = false;
            isUnlocked = true;
            Debug.Log("Collider unlocked!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogManager.DialogIDRead(requiredID))
            {
                UnlockCollider();
                Debug.Log("Dialog ID met. Collider unlocked.");
            }
            else
            {
                Debug.Log("Dialog ID not met. Collider remains locked.");
            }
        }
    }
}
