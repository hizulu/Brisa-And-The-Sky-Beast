using UnityEngine;

/*
 * NOMBRE CLASE (Abstracta): StateMachine
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

    /*
     * M�todo que lee la entrada de inputs del estado actual (si existe).
    */
    public void HandleInput()
    {
        CurrentState?.HandleInput();
    }

    /*
     * M�todo que actualiza la l�gica del estado actual (si existe).
    */
    public void UpdateLogic()
    {
        CurrentState?.UpdateLogic();
    }

    /*
     * M�todo que actualiza la f�sica del estado actual (si existe).
    */
    public void UpdatePhysics()
    {
        CurrentState?.UpdatePhysics();
    }

    /*
     * M�todo que recibe la entrada de una colisi�n de un trigger del estado actual (si existe).
     * @param1: collider - El collider con el que choca el Player.
    */
    public void OnTriggerEnter(Collider collider)
    {
        CurrentState?.OnTriggerEnter(collider);
    }

    /*
     * M�todo que recibe la salida de una colisi�n de un trigger del estado actual (si existe).
     * @param1: collider - El collider del que sale el Player.
    */
    public void OnTriggerExit(Collider collider)
    {
        CurrentState?.OnTriggerExit(collider);
    }
}
