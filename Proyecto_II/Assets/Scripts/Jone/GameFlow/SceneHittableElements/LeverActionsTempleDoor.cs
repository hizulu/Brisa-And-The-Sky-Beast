using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 04/05/2025
[CreateAssetMenu(fileName = "TempleDoorAction", menuName = "Lever Actions/Temple Door Action")]
public class LeverActionsTempleDoor : LeverActionBase
{
    [SerializeField] private float movementDuration = 3f;
    private TempleDoorMover templeDoorMover;

    public bool isDoorOpen = false;

    public override void DoLeverAction()
    {
        if (isDoorOpen)
            return;

        if (templeDoorMover == null)
        {
            templeDoorMover = GameObject.FindObjectOfType<TempleDoorMover>();
            if (templeDoorMover == null)
            {
                Debug.LogWarning("No se encontró TempleDoorMover en la escena.");
                return;
            }
        }
        templeDoorMover.StartMoving(Vector3.zero, movementDuration);
    }

    public override void UndoLeverAction()
    {
        if (isDoorOpen)
            return;
        if (templeDoorMover == null)
        {
            templeDoorMover = GameObject.FindObjectOfType<TempleDoorMover>();
            if (templeDoorMover == null)
            {
                Debug.LogWarning("No se encontró TempleDoorMover en la escena.");
                return;
            }
        }
        templeDoorMover.StartMoving(Vector3.zero, movementDuration);
    }
}
