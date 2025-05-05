using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 04/05/2025
[CreateAssetMenu(fileName = "TempleDoorAction", menuName = "Lever Actions/Temple Door Action")]
public class LeverActionsTempleDoor : LeverActionBase
{
    [SerializeField] private float movementDuration = 3f;
    private bool canActivate = false;
    private TempleDoorMover templeDoorMover;

    public override void DoLeverAction()
    {
        if (templeDoorMover == null)
        {
            templeDoorMover = GameObject.FindObjectOfType<TempleDoorMover>();
            if (templeDoorMover == null)
            {
                Debug.LogWarning("No se encontró DrawbridgeMover en la escena.");
                return;
            }
        }

        if (canActivate)
        {
            templeDoorMover.StartMoving(Vector3.zero, movementDuration);
        }
    }
}
