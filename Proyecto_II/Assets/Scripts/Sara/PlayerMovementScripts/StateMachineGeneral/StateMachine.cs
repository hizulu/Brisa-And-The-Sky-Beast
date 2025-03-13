using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE SCRIPT: StateMachine
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase abstracta que gestiona el estado actual y las transciones de los diferentes estados.
 * VERSIÓN: 1.0. Métodos básicos que gestionan los estados y sus transiciones.
*/

public abstract class StateMachine
{
    private IState currentState;

    /*
     * Método que se encarga de cambiar el estado actual por el nuevo que entre.
    */
    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    /*
     * Método que actualiza la lógica del estado actual. 
    */
    public void UpdateLogic()
    {
        currentState?.UpdateLogic();
    }

    /*
     * Método que actualiza la física del estado actual. 
    */
    public void UpdatePhysics()
    {
        currentState?.UpdatePhysics();
    }
}
