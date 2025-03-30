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
    public IState CurrentState { get; private set; }
    public IState PreviousState {  get; private set; }

    /*
     * Método que se encarga de cambiar el estado actual por el nuevo que entre.
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
     * Método que actualiza la lógica del estado actual. 
    */
    public void UpdateLogic()
    {
        CurrentState?.UpdateLogic();
    }

    /*
     * Método que actualiza la física del estado actual. 
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
