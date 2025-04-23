using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerWalkState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2015
 * DESCRIPCIÓN: Clase que hereda de PlayerMovedState.
 *              Subestado que gestiona la acción de caminar.
 * VERSIÓN: 1.0. 
 */
public class PlayerWalkState : PlayerMovedState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        stateMachine.MovementData.MovementSpeedModifier = groundedData.WalkData.WalkSpeedModif;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.WalkParameterHash);
        //Debug.Log("Has entrado en el estado de CAMINAR.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        audioManager.PlaySFX(audioManager.walk);
    }

    public override void Exit()
    {
        base.Exit();
        audioManager.StopSFX();
        StopAnimation(stateMachine.Player.PlayerAnimationData.WalkParameterHash);
        //Debug.Log("Has salido del estado de CAMINAR.");
    }
    #endregion

    #region Método Cancelar Entrada Input
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
    #endregion
}
