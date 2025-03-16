using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE SCRIPT: StateMachine
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase abstracta que gestiona el estado actual y las transciones de los diferentes estados.
 * VERSI�N: 1.0. M�todos b�sicos que gestionan los estados y sus transiciones.
*/

public abstract class StateMachine
{
    private IState currentState;

    /*
     * M�todo que se encarga de cambiar el estado actual por el nuevo que entre.
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
     * M�todo que actualiza la l�gica del estado actual. 
    */
    public void UpdateLogic()
    {
        currentState?.UpdateLogic();
    }

    /*
     * M�todo que actualiza la f�sica del estado actual. 
    */
    public void UpdatePhysics()
    {
        currentState?.UpdatePhysics();
    }
}
