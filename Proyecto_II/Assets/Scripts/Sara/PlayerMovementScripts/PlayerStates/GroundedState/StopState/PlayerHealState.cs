using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealState : PlayerGroundedState
{
    bool healFinish;
    private ItemData healIncreaseSpecificItem;

    public PlayerHealState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        HealPlayer();
        healFinish = false;
        base.Enter();
        Debug.Log("Has entrado en el estado de curarte");
        StartAnimation(stateMachine.Player.PlayerAnimationData.HealParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishAnimation();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Has salido del estado de curarte");
        StopAnimation(stateMachine.Player.PlayerAnimationData.HealParameterHash);
    }

    protected override void FinishAnimation()
    {
        if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Heal") && animPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            healFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void SetHealingItem(ItemData _item)
    {
        healIncreaseSpecificItem = _item;
    }

    private void HealPlayer()
    {
        statsData.CurrentHealth += healIncreaseSpecificItem.healIncrease;
        statsData.CurrentHealth = Mathf.Min(statsData.CurrentHealth, statsData.MaxHealth);

        InventoryManager.Instance.RemoveItem(healIncreaseSpecificItem);
    }
}
