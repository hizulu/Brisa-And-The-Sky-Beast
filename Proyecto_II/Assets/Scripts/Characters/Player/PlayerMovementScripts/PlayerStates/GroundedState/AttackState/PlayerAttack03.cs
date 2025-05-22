using UnityEngine;

/*
 * NOMBRE CLASE: PlayerAttack03
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 04/04/2025
 * DESCRIPCIÓN: Gestiona la lógica del último ataque (fin combo) del Player.
 * VERSIÓN: 1.0. 
 */
public class PlayerAttack03 : PlayerAttackState
{
    public PlayerAttack03(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
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
        EventsManager.TriggerSpecialEvent<float>("OnAttack03Enemy", attackDamageCombo03); // EVENTO: Crear evento de dañar al enemigo con daño del ComboAttack03.
        //Debug.Log("Daño del ataque 3: " + " " + attackDamageCombo03);
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.Attack, 0.6f);
    }

    public override void UpdateLogic()
    {
        FinishAnimation();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        attackFinish = false;
        //stateMachine.Player.SfxPlayer.StopSound(BrisaSFXType.Attack);
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.Attack03ParameterHash);
    }
    #endregion

    #region Métodos Propios ComboAttack03
    /// <summary>
    /// Método sobreescrito para comprobar que la animación del ataque 3 se ha terminado para pasar a idleState.
    /// </summary>
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

    /// <summary>
    /// Método sobreescrito para cambiar la expresión de Brisa cuando está realizando el último ataque.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.22f, 0f));
        SetFaceProperty(2, new Vector2(0.375f, 0f));
        SetFaceProperty(3, new Vector2(0.66f, 0f));
    }
    #endregion
}
