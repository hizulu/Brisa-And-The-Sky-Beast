using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEhunaSceneEnter : MonoBehaviour
{
    [SerializeField] private BeastTrapped beastTrapped;
    void Start()
    {
        beastTrapped.SetBeastFreeFromCage();
    }
}
