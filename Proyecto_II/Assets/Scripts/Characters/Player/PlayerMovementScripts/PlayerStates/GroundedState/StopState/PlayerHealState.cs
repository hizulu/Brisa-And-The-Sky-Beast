using UnityEngine;

/*
 * NOMBRE CLASE: PlayerHealState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 14/04/2025
 * DESCRIPCI�N: Clase que hereda de PlayerGroundedState.
 *              Subestado que gestiona la acci�n de curar a Player.
 * VERSI�N: 1.0. 
 */
public class PlayerHealState : PlayerStopState
{
    public PlayerHealState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    bool healFinish;
    
    private float healDelay = 0.5f;
    private float currentTime = 0f;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        healFinish = false;
        stateMachine.Player.Baya.SetActive(true);
        
        base.Enter();
        //Debug.Log("Has entrado en el estado de curarte");
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
        stateMachine.Player.Baya.SetActive(false);
        //Debug.Log("Has salido del estado de curarte");
        StopAnimation(stateMachine.Player.PlayerAnimationData.HealParameterHash);
    }
    #endregion

    #region M�todos Propios HealState
    /// <summary>
    /// M�todo sobreescrito para comprobar que la animaci�n de curar se ha terminado para pasar a idle.
    /// </summary>
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("HealBrisa") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    /// <summary>
    /// M�todo que asigna el valor del par�metro del item concreto para curar vida a Brisa.
    /// </summary>
    /// <param name="_berry">Valor espec�fico del item que aumenta la vida de Player.</param>
    public void SetHealingBerry(ItemData _berry)
    {
        healIncreaseSpecificItem = _berry;
    }

    /// <summary>
    /// M�todo para curar al Player.
    /// Accede al item concreto del inventario y aumenta la vida de Brisa en base a su valor.
    /// </summary>
    private void HealPlayer()
    {
        statsData.CurrentHealth += healIncreaseSpecificItem.healIncrease;
        statsData.CurrentHealth = Mathf.Min(statsData.CurrentHealth, statsData.MaxHealth);

        InventoryManager.Instance.RemoveItem(healIncreaseSpecificItem);
        healFinish = true;
    }

    /// <summary>
    /// M�todo sobreescrito para cambiar la expresi�n de Brisa cuando est� cur�ndose.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.11f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }
    #endregion
}