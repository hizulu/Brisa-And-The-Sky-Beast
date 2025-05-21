using UnityEngine;

/*
 * NOMBRE CLASE: PlayerAttack02
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 04/04/2025
 * DESCRIPCIÓN: Gestiona la lógica del segundo ataque (combo) del Player.
 * VERSIÓN: 1.0. 
 */
public class PlayerAttack02 : PlayerAttackState
{
    public PlayerAttack02(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
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
        EventsManager.TriggerSpecialEvent<float>("OnAttack02Enemy", attackDamageCombo02); // EVENTO: Crear evento de dañar al enemigo con daño del ComboAttack02.
        //Debug.Log("Daño del ataque 2: " + " " + attackDamageCombo02);
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

    #region Métodos Propios Attack02
    /// <summary>
    /// Método sobreescrito para comprobar que la animación del ataque 2 se ha terminado.
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
    /// Método sobreescrito para cambiar la expresión de Brisa cuando está realizando el segundo ataque.
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
