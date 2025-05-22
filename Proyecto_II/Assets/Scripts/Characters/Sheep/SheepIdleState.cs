using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepIdleState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/05/2025
 * DESCRIPCIÓN: Clase que hereda de SheepStateTemplate y define la lógica del estado de Idle de las ovejas.
 * VERSIÓN: 1.0.
 */

public class SheepIdleState : SheepStateTemplate
{
    public SheepIdleState(SheepStateMachine _stateMachine) : base(_stateMachine) { }

    #region Variables
    private float currentTimeIdle;
    private float maxTimeInIdle;
    #endregion

    #region Métodos Base de la Máquina de Estados
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

    #region Métodos Propios IdleState
    /// <summary>
    /// Método para actualizar el tiempo máximo que las ovejas pueden estar en estado de idle.
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
