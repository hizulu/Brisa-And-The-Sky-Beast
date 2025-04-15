using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastTrapped : MonoBehaviour
{
    BeastBehaviorTree beastBTScript;

    private void Awake()
    {
        beastBTScript = GetComponent<BeastBehaviorTree>();
        beastBTScript.enabled = false;
    }

    public void SetBeastFreeFromCage()
    {
        beastBTScript.enabled = true;
    }
}
