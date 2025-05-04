using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealBeastState : PlayerInteractionState
{
    public PlayerHealBeastState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    private bool healBeastFinishAnimation;
    private bool healBeastFinish;

    private float healDelay = 0.7f;
    private float currentTime = 0f;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        healBeastFinish = false;
        healBeastFinishAnimation = false;
        stateMachine.Player.Mango.SetActive(true);
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.HealBeastParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (currentTime < healDelay)
            currentTime += Time.deltaTime;
        else if (!healBeastFinish)
        {
            HealBeast();
            stateMachine.Player.Mango.SetActive(false);
        }

        FinishAnimation();
    }

    public override void Exit()
    {
        currentTime = 0f;
        healBeastFinish = false;
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.HealBeastParameterHash);
    }
    #endregion

    #region Métodos Propios HealBeastState
    /*
     * Método para comprobar que la animación de acariciar se ha terminado para pasar al siguiente estado requerido.
     */
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("HealBeast") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            healBeastFinishAnimation = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void SetHealingMango(ItemData _mango)
    {
        healIncreaseSpecificItem = _mango;
    }

    private void HealBeast()
    {
        Debug.Log($"Curando a la Bestia con: {healIncreaseSpecificItem.healIncrease} puntos");
        stateMachine.Player.Beast.currentHealth += healIncreaseSpecificItem.healIncrease;
        stateMachine.Player.Beast.currentHealth = Mathf.Min(stateMachine.Player.Beast.currentHealth, stateMachine.Player.Beast.maxHealth);

        InventoryManager.Instance.RemoveItem(healIncreaseSpecificItem);
        healBeastFinish = true;
    }

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.555f, 0f));
        SetFaceProperty(2, new Vector2(0.125f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }
    #endregion
}
