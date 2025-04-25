using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerCrouchState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 11/03/2025
 * DESCRIPCI�N: Clase que hereda de PlayerMovedState.
 *              Subestado que gestiona la acci�n de andar en sigilo.
 * VERSI�N: 1.0. 
 */
public class PlayerCrouchState : PlayerMovedState
{
    public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        stateMachine.MovementData.MovementSpeedModifier = groundedData.CrouchData.CrouchSpeedModif;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.CrouchParameterHash);
        EventsManager.TriggerSpecialEvent<bool>("CrouchState", true);
        Debug.Log("Has entrado en el estado de AGACHARSE.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (stateMachine.MovementData.MovementInput == Vector2.zero)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.CrouchParameterHash);
        EventsManager.TriggerSpecialEvent<bool>("CrouchState", false);
        Debug.Log("Has salido del estado de AGACHARSE.");
    }
    #endregion

    #region M�todo Cancelar Entrada Input
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
    #endregion
}
