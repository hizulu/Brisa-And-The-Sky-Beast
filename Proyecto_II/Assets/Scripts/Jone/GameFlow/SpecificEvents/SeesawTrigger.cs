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
            seesaw.AddWeight(isLeft, 1);
        else if (other.CompareTag("Beast"))
            seesaw.AddWeight(isLeft, 5);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            seesaw.RemoveWeight(isLeft, 1);
        else if (other.CompareTag("Beast"))
            seesaw.RemoveWeight(isLeft, 5);
    }
}
