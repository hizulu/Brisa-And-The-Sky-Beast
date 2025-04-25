using UnityEngine;

/*
 * NOMBRE CLASE: PlayerHealState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 14/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerGroundedState.
 *              Subestado que gestiona la acción de curar a Player.
 * VERSIÓN: 1.0. 
 */
public class PlayerHealState : PlayerStopState
{
    public PlayerHealState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    bool healFinish;
    private ItemData healIncreaseSpecificItem;
    #endregion

    #region Métodos Base de la Máquina de Estados
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
    #endregion

    #region Métodos Propios HealState
    /*
     * Método para comprobar que la animación de curar se ha terminado para pasar al siguiente estado requerido.
     */
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("HealBrisa") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
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
    #endregion
}
