using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEhunaSceneEnter : MonoBehaviour
{
    [SerializeField] private BeastTrapped beastTrapped;
    [SerializeField] private Player player;
    void Start()
    {
        beastTrapped.SetBeastFreeFromCage();
        player.PaloRecogido();
    }
}
