using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerSprintState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 23/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerMovedState.
 *              Subestado que gestiona la acción de sprintar.
 * VERSIÓN: 1.0. 
 */
public class PlayerSprintState : PlayerMovedState
{
    public PlayerSprintState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private bool sprintFinish;
    private Vector3 sprintDirection;
    private float sprintDistance = 5f;
    private float sprintSpeed = 10f;
    private float distanceTraveled = 0f;

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        sprintFinish = false;
        distanceTraveled = 0f;
        GetHorizontalDirection();
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.SprintParameterHash);
        //Debug.Log("Has entrado en el estado de SPRINTAR");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishAnimation();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        MovementSprint();
    }

    public override void Exit()
    {
        sprintFinish = false;
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.SprintParameterHash);
        //Debug.Log("Has salido del estado de SPRINTAR");
    }
    #endregion

    #region Métodos Propios SprintState
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("Sprint") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            sprintFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    private void GetHorizontalDirection()
    {
        sprintDirection = stateMachine.Player.transform.forward;
        sprintDirection.y = 0f;
        sprintDirection.Normalize();
    }

    /*
     * Método para que Player avance la distancia concreta del sprint.
     */
    protected virtual void MovementSprint()
    {
        if (distanceTraveled >= sprintDistance) return;

        float stepDistance = sprintSpeed * Time.deltaTime;
        stateMachine.Player.RbPlayer.MovePosition(stateMachine.Player.transform.position + sprintDirection * stepDistance);
        distanceTraveled += stepDistance;
    }

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0f, 0f));
        SetFaceProperty(2, new Vector2(0.125f, 0f));
        SetFaceProperty(3, new Vector2(0.66f, 0f));
    }
    #endregion
}
