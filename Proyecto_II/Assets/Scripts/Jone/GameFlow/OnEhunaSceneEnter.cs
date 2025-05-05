using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEhunaSceneEnter : MonoBehaviour
{
    [SerializeField] private BeastTrapped beastTrapped;
    [SerializeField] private Player player;
    [SerializeField] private LeverActionsTempleDoor leverActionsTempleDoor;
    void Start()
    {
        beastTrapped.SetBeastFreeFromCage();
        player.PaloRecogido();
        leverActionsTempleDoor.isDoorOpen = false;
    }
}
