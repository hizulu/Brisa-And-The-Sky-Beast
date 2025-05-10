using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepWalkState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 10/05/2025
 * DESCRIPCI�N: Clase que hereda de SheepStateTemplate y define la l�gica del estado de Caminar de las ovejas.
 * VERSI�N: 1.0.
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
