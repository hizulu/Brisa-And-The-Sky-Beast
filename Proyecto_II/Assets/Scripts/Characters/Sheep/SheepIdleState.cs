using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepIdleState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 10/05/2025
 * DESCRIPCI�N: Clase que hereda de SheepStateTemplate y define la l�gica del estado de Idle de las ovejas.
 * VERSI�N: 1.0.
 */

public class SheepIdleState : SheepStateTemplate
{
    public SheepIdleState(SheepStateMachine _stateMachine) : base(_stateMachine) { }

    #region Variables
    private float currentTimeIdle;
    private float maxTimeInIdle;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        RandomSheepSound();
        base.Enter();
        sheepStateMachine.Sheep.AnimSheep.SetBool("isIdle", true);
        //Debug.Log("La oveja ha entrado en el estado de IDLE");
        currentTimeIdle = 0f;
        maxTimeInIdle = Random.Range(1f, 4f);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        UpdateIdleTime();
    }

    public override void Exit()
    {
        base.Exit();
        sheepStateMachine.Sheep.AnimSheep.SetBool("isIdle", false);
        //Debug.Log("La oveja ha salido del estado de IDLE.");
    }
    #endregion

    #region M�todos Propios IdleState
    /// <summary>
    /// M�todo para actualizar el tiempo m�ximo que las ovejas pueden estar en estado de idle.
    /// </summary>
    private void UpdateIdleTime()
    {
        currentTimeIdle += Time.deltaTime;

        if (currentTimeIdle > maxTimeInIdle)
            sheepStateMachine.ChangeState(sheepStateMachine.SheepWalkState);
    }

    private void RandomSheepSound()
    {
        if(Random.value < 0.05f)
            sheepStateMachine.Sheep.SfxSheep.PlayRandomSFX(SheepSFXType.Idle);
    }
    #endregion
}
