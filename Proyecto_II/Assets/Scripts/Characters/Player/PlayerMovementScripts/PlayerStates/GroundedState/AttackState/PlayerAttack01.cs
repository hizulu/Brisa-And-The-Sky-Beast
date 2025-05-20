using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerAttack01
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 04/04/2025
 * DESCRIPCI�N: Gestiona la l�gica del primer ataque (combo) del Player.
 * VERSI�N: 1.0. 
 */
public class PlayerAttack01 : PlayerAttackState
{
    public PlayerAttack01(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        maxTimeToNextAttack = 0.5f;
        attackTimeElapsed = 0;
        attackFinish = false;
        attackDamageModifierMin = 1f;
        attackDamageModifierMax = 1.3f;
        float attackDamageModifier = UnityEngine.Random.Range(attackDamageModifierMin, attackDamageModifierMax);
        float attackDamageCombo01 = stateMachine.StatsData.AttackDamageBase * attackDamageModifier;
        EventsManager.TriggerSpecialEvent<float>("OnAttack01Enemy", attackDamageCombo01); // EVENTO: Crear evento de da�ar al enemigo con da�o del ComboAttack01.
        base.Enter();
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.Attack);
        stateMachine.Player.GolpearPrueba();
        StartAnimation(stateMachine.Player.PlayerAnimationData.Attack01ParameterHash);
        //Debug.Log("Da�o del ataque 1: " + " " + attackDamageCombo01);

        //audioManager.PlaySFX(audioManager.attack01);
    }

    public override void HandleInput()
    {
        if (stateMachine.Player.PlayerInput.PlayerActions.Attack.triggered && !isWaitingForInput)
        {
            canContinueCombo = true;
            isWaitingForInput = true;
        }

        if (attackFinish && canContinueCombo)
        {
            if (attackTimeElapsed < maxTimeToNextAttack && isWaitingForInput)
                stateMachine.ChangeState(stateMachine.Attack02State);
            else
            {
                canContinueCombo = false;
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }

    public override void UpdateLogic()
    {
        FinishAnimation();
        attackTimeElapsed += Time.deltaTime;            
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        canContinueCombo = false;
        isWaitingForInput = false;
        attackFinish = false;
        stateMachine.Player.SfxPlayer.StopSound(BrisaSFXType.Attack);
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.Attack01ParameterHash);
    }
    #endregion

    #region M�todos Propios Attack01State
    /// <summary>
    /// M�todo para comprobar que la animaci�n del ataque 1 se ha terminado.
    /// </summary>
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            attackFinish = true;
            if(!stateMachine.Player.PlayerInput.PlayerActions.Movement.IsPressed() && !canContinueCombo)
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    protected override void Move()
    {
        if (!attackFinish) return;
    }

    /// <summary>
    /// M�todo sobreescrito para cambiar la expresi�n de Brisa cuando est� realizando el primer ataque.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.66f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0.66f, 0f));
    }
    #endregion
}
