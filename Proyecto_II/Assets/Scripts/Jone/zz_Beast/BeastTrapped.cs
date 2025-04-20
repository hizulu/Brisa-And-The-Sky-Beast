using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 09/04/2025
public class BeastTrapped : MonoBehaviour
{
    Beast beastScript;

    public bool beasIsFree = false;

    private void Awake()
    {
        beastScript = GetComponent<Beast>();
        beastScript.enabled = false;
    }

    public void SetBeastFreeFromCage()
    {
        beastScript.enabled = true;
        beasIsFree = true;
    }
}
