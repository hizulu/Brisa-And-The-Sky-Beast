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
        Debug.Log("Beast is in, doing open the hollow exit action");

        // TODO: implement door open animation

        beast.anim.SetTrigger("openDoor");
    }
}
