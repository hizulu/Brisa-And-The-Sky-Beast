using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeesawTrigger : MonoBehaviour
{
    [SerializeField] bool isLeft;
    public Seesaw seesaw;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            seesaw.AddWeight(isLeft);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            seesaw.RemoveWeight(isLeft);
    }
}
