using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : HittableElement
{
    [SerializeField] private LeverActionBase leverAction;

    private bool isActivated = false;

    public override void OnHit()
    {
        DoLeverAnimation();

        if (leverAction == null) return;

        if (leverAction.IsActionReversible())
        {
            if (isActivated)
            {
                leverAction.UndoLeverAction();
            }
            else
            {
                leverAction.DoLeverAction();
            }
            isActivated = !isActivated;
        }
        else if (!isActivated)
        {
            leverAction.DoLeverAction();
            isActivated = true;
        }
    }

    public void DoLeverAnimation()
    {

    }
}
