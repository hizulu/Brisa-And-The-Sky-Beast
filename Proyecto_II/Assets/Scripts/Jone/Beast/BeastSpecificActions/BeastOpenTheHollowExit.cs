using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 18/04/2025
public class BeastOpenTheHollowExit : BeastActionable
{
    public override void OnBeast()
    {
        if (!beastIsIn)
        {
            Debug.Log("Beast is not in");
            return;
        }
        GameManager.Instance.Victory();

        // TODO: implement door open animation

        beast.anim.SetTrigger("openDoor");
    }
}
