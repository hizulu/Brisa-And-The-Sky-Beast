using UnityEngine;

/*
 * NOMBRE CLASE: PlayerRideBeastState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 18/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerInteractionState.
 *              Subestado que gestiona la acción de montar a la Bestia.
 * VERSIÓN: 1.0. 
 */
public class PlayerRideBeastState : PlayerInteractionState
{
    public PlayerRideBeastState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        stateMachine.MovementData.MovementSpeedModifier = groundedData.WalkData.WalkSpeedModif;
        base.Enter();
        Debug.Log("Has entrado en el estado de MONTAR A LA BESTIA");
        StartAnimation(stateMachine.Player.PlayerAnimationData.RideBeastParameterHash);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Has salido del estado de MONTAR A LA BESTIA");
        StopAnimation(stateMachine.Player.PlayerAnimationData.RideBeastParameterHash);
    }
    #endregion
}
