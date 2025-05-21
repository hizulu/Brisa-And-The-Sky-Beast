using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEhunaSceneEnter : MonoBehaviour
{
    [SerializeField] private BeastTrapped beastTrapped;
    [SerializeField] private LeverActionsTempleDoor leverActionsTempleDoor;

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        beastTrapped.SetBeastFreeFromCage();
        leverActionsTempleDoor.isDoorOpen = false;
    }
}
