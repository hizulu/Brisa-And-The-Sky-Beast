using UnityEngine;

/*
 * NOMBRE CLASE: PlayerIdleState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerGroundeState
 *              Subestado que gestiona la acción de estar parado.
 * VERSIÓN: 1.0. 
 */
public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        stateMachine.MovementData.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.IdleParameterHash);
        //Debug.Log("Has entrado en el estado de IDLE.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (stateMachine.MovementData.MovementInput == Vector2.zero) return;

        OnMove();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.IdleParameterHash);
        //Debug.Log("Has salido del estado de IDLE.");
    }
    #endregion
}
