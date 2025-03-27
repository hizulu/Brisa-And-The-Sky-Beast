using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPReloadScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.ReloadScene();
        }
    }
}
