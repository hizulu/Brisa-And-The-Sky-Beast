using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepGrazeState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/05/2025
 * DESCRIPCIÓN: Clase que hereda de SheepStateTemplate y define la lógica del estado de Pastar de las ovejas.
 * VERSIÓN: 1.0.
 */

public class SheepGrazeState : SheepStateTemplate
{
    public SheepGrazeState(SheepStateMachine _sheepStateMachine) : base(_sheepStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        sheepStateMachine.Sheep.AnimSheep.SetBool("isGrazing", true);
        Debug.Log("La oveja ha entrado en el estado de PASTAR");
    }

    public override void Exit()
    {
        base.Exit();
        sheepStateMachine.Sheep.AnimSheep.SetBool("isGrazing", false);
        Debug.Log("La oveja ha salido del estado de PASTAR.");
    }
}
