using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] private string groundedParameterName = "Grounded";
    [SerializeField] private string movedParameterName = "Moved";

    [SerializeField] private string idleParameterName = "isIdle";
    [SerializeField] private string walkParameterName = "isWalking";
    public int GroundedParameterHash { get; private set; }
    public int MovedParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }

    public void Initialize()
    {
        GroundedParameterHash = Animator.StringToHash(groundedParameterName);
        MovedParameterHash = Animator.StringToHash(movedParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
    }
}
