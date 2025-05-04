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
    bool healFinishAnimation;
    bool healFinish;
    
    private float healDelay = 0.5f;
    private float currentTime = 0f;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        healFinishAnimation = false;
        healFinish = false;
        stateMachine.Player.Baya.SetActive(true);
        
        base.Enter();
        Debug.Log("Has entrado en el estado de curarte");
        StartAnimation(stateMachine.Player.PlayerAnimationData.HealParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (currentTime < healDelay)
            currentTime += Time.deltaTime;
        else if(!healFinish)
        {
            HealPlayer();
            stateMachine.Player.Baya.SetActive(false);
        }

        FinishAnimation();
    }

    public override void Exit()
    {
        currentTime = 0f;
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
            healFinishAnimation = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void SetHealingBerry(ItemData _berry)
    {
        healIncreaseSpecificItem = _berry;
    }

    private void HealPlayer()
    {
        statsData.CurrentHealth += healIncreaseSpecificItem.healIncrease;
        statsData.CurrentHealth = Mathf.Min(statsData.CurrentHealth, statsData.MaxHealth);

        InventoryManager.Instance.RemoveItem(healIncreaseSpecificItem);
        healFinish = true;
    }

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.11f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }
    #endregion
}