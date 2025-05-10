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

    public override void Enter()
    {
        base.Enter();
        Debug.Log("La oveja ha entrado en el estado de IDLE");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("La oveja ha salido del estado de IDLE.");
    }
}
