using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepWalkState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/05/2025
 * DESCRIPCIÓN: Clase que hereda de SheepStateTemplate y define la lógica del estado de Caminar de las ovejas.
 * VERSIÓN: 1.0.
 */

public class SheepWalkState : SheepStateTemplate
{
    public SheepWalkState(SheepStateMachine _sheepStateMachine) : base(_sheepStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("La oveja ha entrado en el estado de CAMINAR");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("La oveja ha salido del estado de CAMINAR.");
    }
}
