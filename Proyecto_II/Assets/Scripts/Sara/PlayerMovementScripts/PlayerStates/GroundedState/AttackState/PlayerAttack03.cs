/*
 * NOMBRE CLASE: PlayerAttack03
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 
 * DESCRIPCI�N: Clase que hereda de PlayerAttackState
 * VERSI�N: 1.0. 
 */
public class PlayerAttack03 : PlayerAttackState
{
    public PlayerAttack03(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        attackFinish = false;
        attackDamageModifierMin = 1.51f;
        attackDamageModifierMax = 2f;
        base.Enter();
        stateMachine.Player.GolpearPrueba();
        StartAnimation(stateMachine.Player.PlayerAnimationData.Attack03ParameterHash);
        float attackDamageModifier = UnityEngine.Random.Range(attackDamageModifierMin, attackDamageModifierMax);
        float attackDamageCombo03 = stateMachine.StatsData.AttackDamageBase * attackDamageModifier;
        EventsManager.TriggerSpecialEvent<float>("OnAttack03Enemy", attackDamageCombo03); // EVENTO: Crear evento de da�ar al enemigo con da�o del ComboAttack03.
        //Debug.Log("Da�o del ataque 3: " + " " + attackDamageCombo03);
    }

    public override void UpdateLogic()
    {
        FinishAnimation();
    }

    public override void Exit()
    {
        attackFinish = false;
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.Attack03ParameterHash);
    }
    #endregion

    #region M�todos Propios ComboAttack03
    /*
     * M�todo para comprobar que la animaci�n del ataque 3 se ha terminado para pasar al siguiente estado requerido.
     */
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("Attack03") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            attackFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    protected override void Move()
    {
        if (!attackFinish) return;
    }
    #endregion
}
