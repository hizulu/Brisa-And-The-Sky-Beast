/*
 * NOMBRE CLASE: PlayerAttackState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 
 * DESCRIPCIÓN: Clase que hereda de PlayerGroundedState
 * VERSIÓN: 1.0. 
 */
public class PlayerAttackState : PlayerGroundedState
{
    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    protected bool attackFinish;

    protected float attackTimeElapsed;
    protected float maxTimeToNextAttack = 1f;
    protected int currentNumAttack;

    protected bool canContinueCombo;
    protected bool isWaitingForInput;

    protected float attackDamageModifierMin;
    protected float attackDamageModifierMax;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.AttackParameterHash);
        //Debug.Log("Has entrado en el estado de ATACAR");
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.AttackParameterHash);
        //Debug.Log("Has salido del estado de ATACAR");
    }
    #endregion
}
