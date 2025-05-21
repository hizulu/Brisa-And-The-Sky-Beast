using UnityEngine;

/*
 * NOMBRE CLASE: PlayerAttack02
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 04/04/2025
 * DESCRIPCI�N: Gestiona la l�gica del segundo ataque (combo) del Player.
 * VERSI�N: 1.0. 
 */
public class PlayerAttack02 : PlayerAttackState
{
    public PlayerAttack02(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        maxTimeToNextAttack = 0.7f;
        attackTimeElapsed = 0;
        attackFinish = false;
        attackDamageModifierMin = 1.31f;
        attackDamageModifierMin = 1.5f;
        base.Enter();
        stateMachine.Player.GolpearPrueba();
        StartAnimation(stateMachine.Player.PlayerAnimationData.Attack02ParameterHash);
        float attackDamageModifier = UnityEngine.Random.Range(attackDamageModifierMin, attackDamageModifierMax);
        float attackDamageCombo02 = stateMachine.StatsData.AttackDamageBase * attackDamageModifier;
        EventsManager.TriggerSpecialEvent<float>("OnAttack02Enemy", attackDamageCombo02); // EVENTO: Crear evento de da�ar al enemigo con da�o del ComboAttack02.
        //Debug.Log("Da�o del ataque 2: " + " " + attackDamageCombo02);
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.Attack);
    }

    public override void HandleInput()
    {
        if (stateMachine.Player.PlayerInput.PlayerActions.Attack.triggered && attackTimeElapsed < maxTimeToNextAttack)
        {
            canContinueCombo = true;
        }
    }

    public override void UpdateLogic()
    {
        FinishAnimation();
        attackTimeElapsed += Time.deltaTime;

        if (attackFinish && canContinueCombo)
        {
            stateMachine.ChangeState(stateMachine.Attack03State);
        }
        else if (attackTimeElapsed >= maxTimeToNextAttack && !canContinueCombo)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
    public override void Exit()
    {
        canContinueCombo = false;
        attackFinish = false;
        //stateMachine.Player.SfxPlayer.StopSound(BrisaSFXType.Attack);
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.Attack02ParameterHash);
    }
    #endregion

    #region M�todos Propios Attack02
    /// <summary>
    /// M�todo sobreescrito para comprobar que la animaci�n del ataque 2 se ha terminado.
    /// </summary>
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            attackFinish = true;
    }

    protected override void Move()
    {
        if (!attackFinish) return;
    }

    /// <summary>
    /// M�todo sobreescrito para cambiar la expresi�n de Brisa cuando est� realizando el segundo ataque.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.78f, 0f));
        SetFaceProperty(2, new Vector2(0.125f, 0f));
        SetFaceProperty(3, new Vector2(0.66f, 0f));
    }
    #endregion
}
