using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheHollowEndingTrigger : MonoBehaviour
{
    private bool brisaIsIn = false;
    private bool beastIsIn = false; // TODO: sustituir por acción de la bestia

    private void Update()
    {
        if (brisaIsIn && beastIsIn)
            GameManager.Instance.Victory();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            brisaIsIn = true;
        if (other.CompareTag("Beast"))
            beastIsIn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            brisaIsIn = false;
        if (other.CompareTag("Beast"))
            beastIsIn = false;
    }
}
