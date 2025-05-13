using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepGrazeState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 10/05/2025
 * DESCRIPCI�N: Clase que hereda de SheepStateTemplate y define la l�gica del estado de Pastar de las ovejas.
 * VERSI�N: 1.0.
 */

public class SheepGrazeState : SheepStateTemplate
{
    public SheepGrazeState(SheepStateMachine _sheepStateMachine) : base(_sheepStateMachine) { }

    #region Variables
    private float currentTimeGrazing;
    private float maxTimeGrazing;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        base.Enter();
        sheepStateMachine.Sheep.AnimSheep.SetBool("isGrazing", true);
        Debug.Log("La oveja ha entrado en el estado de PASTAR");
        currentTimeGrazing = 0f;
        maxTimeGrazing = Random.Range(3f, 7f);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        UpdateGrazingTime();
    }

    public override void Exit()
    {
        base.Exit();
        sheepStateMachine.Sheep.AnimSheep.SetBool("isGrazing", false);
        Debug.Log("La oveja ha salido del estado de PASTAR.");
    }
    #endregion

    #region M�todos Propios GrazeState
    /// <summary>
    /// M�todo para actualizar el tiempo m�ximo que las ovejas pueden estar en estado de pastar.
    /// </summary>
    private void UpdateGrazingTime()
    {
        currentTimeGrazing += Time.deltaTime;

        if(currentTimeGrazing > maxTimeGrazing)
            sheepStateMachine.ChangeState(sheepStateMachine.SheepIdleState);
    }
    #endregion
}
