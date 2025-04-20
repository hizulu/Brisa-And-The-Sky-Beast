using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPDamageBeast : MonoBehaviour
{
    private Beast beast;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beast"))
        {
            beast = other.GetComponent<Beast>();
            beast.DamageBeast(200f);
        }
    }
}
