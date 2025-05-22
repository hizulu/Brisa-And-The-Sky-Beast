using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepJumpState : SheepStateTemplate
{
    public SheepJumpState(SheepStateMachine _sheepStateMachine) : base(_sheepStateMachine) { }

    #region Variables
    private float jumpForce = 3f;
    private float forwardForce = 4f;
    private float jumpCooldown = 0.8f;
    private float timeSinceLastJump;

    private bool shouldJump = false;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        shouldJump = false;
        base.Enter();
        Debug.Log("La oveja ha entrado en el estado de SALTAR");
        timeSinceLastJump = jumpCooldown;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        UpdateJumps();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (shouldJump)
        {
            JumpSheep();
            shouldJump = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("La oveja ha salido del estado de SALTAR");
    }
    #endregion

    #region Métodos Propios JumpState
    /// <summary>
    /// Método que se encarga de ejecutar el salto de la oveja.
    /// Orienta la oveja hacia el jugador y aplica la fuerza.
    /// </summary>
    private void JumpSheep()
    {
        sheepStateMachine.Sheep.SfxSheep.PlayRandomSFX(SheepSFXType.Jump);

        sheepStateMachine.Sheep.AnimSheep.SetTrigger("isJumping");

        Rigidbody rb = sheepStateMachine.Sheep.RbSheep;

        Vector3 directionToPlayer = sheepStateMachine.Sheep.PlayerTransform.position - sheepStateMachine.Sheep.transform.position;
        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();

        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        sheepStateMachine.Sheep.transform.rotation = lookRotation;

        Vector3 jumpVelocity = directionToPlayer * forwardForce;
        jumpVelocity.y = jumpForce;

        rb.velocity = jumpVelocity;
    }

    /// <summary>
    /// Método que gestiona la lógica de los saltos de las ovejas.
    /// Calculan la distancia que hay hasta Player y si la distancia es menor de 3, cambian a IdleState.
    /// Si no, continúa con la lógica del salto.
    /// </summary>
    private void UpdateJumps()
    {
        float distanceToPlayer = Vector3.Distance(sheepStateMachine.Sheep.transform.position, sheepStateMachine.Sheep.PlayerTransform.position);

        if (distanceToPlayer <= 3f)
        {
            sheepStateMachine.ChangeState(sheepStateMachine.SheepIdleState);
            return;
        }

        timeSinceLastJump += Time.deltaTime;

        if (timeSinceLastJump >= jumpCooldown)
        {
            JumpSheep();
            timeSinceLastJump = 0f;
        }
    }
    #endregion
}
