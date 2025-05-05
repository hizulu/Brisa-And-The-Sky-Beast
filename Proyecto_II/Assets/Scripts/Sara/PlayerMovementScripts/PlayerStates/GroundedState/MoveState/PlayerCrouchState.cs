using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerCrouchState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 11/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerMovedState.
 *              Subestado que gestiona la acción de andar en sigilo.
 * VERSIÓN: 1.0. 
 */
public class PlayerCrouchState : PlayerMovedState
{
    public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
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

    #region Método Cancelar Entrada Input
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
    #endregion

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.44f, 0f));
        SetFaceProperty(2, new Vector2(0.875f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }
}
