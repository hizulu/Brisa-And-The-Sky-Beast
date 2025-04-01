using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] private string groundedParameterName = "Grounded";
    [SerializeField] private string movedParameterName = "Moved";
    [SerializeField] private string attackParameterName = "Attack";
    [SerializeField] private string airborneParameterName = "Airborne";
    [SerializeField] private string deathParameterName = "Death";

    [SerializeField] private string idleParameterName = "isIdle";
    [SerializeField] private string walkParameterName = "isWalking";
    [SerializeField] private string runParameterName = "isRunning";
    [SerializeField] private string crouchParameterName = "isCrouching";
    [SerializeField] private string attack01ParameterName = "isAttacking01";
    [SerializeField] private string attack02ParameterName = "isAttacking02";
    [SerializeField] private string attack03ParameterName = "isAttacking03";
    [SerializeField] private string jumpParameterName = "isJumping";
    [SerializeField] private string fallParameterName = "isFalling";
    [SerializeField] private string landParameterName = "isLanding";
    [SerializeField] private string halfDeadParameterName = "isHalfDeading";
    [SerializeField] private string finalDeadParameterName = "isFinalDeading";

    public int GroundedParameterHash { get; private set; }
    public int MovedParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int AirborneParameterHash { get; private set; }
    public int DeathParameterHash { get; private set; }

    public int IdleParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int CrouchParameterHash { get; private set; }
    public int Attack01ParameterHash { get; private set; }
    public int Attack02ParameterHash { get; private set; }
    public int Attack03ParameterHash { get; private set; }
    public int JumpParameterHash { get; private set; }
    public int FallParameterHash { get; private set; }
    public int LandParameterHash { get; private set; }
    public int HalfDeadParameterHash { get; private set; }
    public int FinalDeadParameterHash { get; private set; }

    public void Initialize()
    {
        GroundedParameterHash = Animator.StringToHash(groundedParameterName);
        MovedParameterHash = Animator.StringToHash(movedParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
        AirborneParameterHash = Animator.StringToHash(airborneParameterName);
        DeathParameterHash = Animator.StringToHash(deathParameterName);

        IdleParameterHash = Animator.StringToHash(idleParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        CrouchParameterHash = Animator.StringToHash(crouchParameterName);

        Attack01ParameterHash = Animator.StringToHash(attack01ParameterName);
        Attack02ParameterHash = Animator.StringToHash(attack02ParameterName);
        Attack03ParameterHash = Animator.StringToHash(attack03ParameterName);

        JumpParameterHash = Animator.StringToHash(jumpParameterName);
        FallParameterHash = Animator.StringToHash(fallParameterName);
        LandParameterHash = Animator.StringToHash(landParameterName);

        HalfDeadParameterHash = Animator.StringToHash(halfDeadParameterName);
        FinalDeadParameterHash = Animator.StringToHash(finalDeadParameterName);
    }
}
