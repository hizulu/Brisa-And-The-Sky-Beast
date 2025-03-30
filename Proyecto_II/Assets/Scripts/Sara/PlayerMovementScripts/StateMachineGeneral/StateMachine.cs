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
    public IState CurrentState { get; private set; }
    public IState PreviousState {  get; private set; }

    /*
     * M�todo que se encarga de cambiar el estado actual por el nuevo que entre.
    */
    public void ChangeState(IState newState)
    {
        PreviousState = CurrentState;
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void HandleInput()
    {
        CurrentState?.HandleInput();
    }

    /*
     * M�todo que actualiza la l�gica del estado actual. 
    */
    public void UpdateLogic()
    {
        CurrentState?.UpdateLogic();
    }

    /*
     * M�todo que actualiza la f�sica del estado actual. 
    */
    public void UpdatePhysics()
    {
        CurrentState?.UpdatePhysics();
    }

    public void OnTriggerEnter(Collider collider)
    {
        CurrentState?.OnTriggerEnter(collider);
    }

    public void OnTriggerExit(Collider collider)
    {
        CurrentState?.OnTriggerExit(collider);
    }
}
