using UnityEngine;

/*
 * NOMBRE CLASE (Abstracta): StateMachine
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase abstracta que gestiona el estado actual y las transciones de los diferentes estados.
 * VERSI�N: 1.0. M�todos b�sicos que gestionan los estados y sus transiciones.
 * VERSI�N: 2.0. Creaci�n de la variable "PreviousState" para establecer condiciones de transiciones de estados.
*/

public abstract class StateMachine
{
    #region Variables
    public IState CurrentState { get; private set; }
    public IState PreviousState {  get; private set; }
    #endregion

    #region M�todos Gesti�n Estados SM
    /// <summary>
    /// M�todo que se encarga de cambiar el estado actual por el nuevo que entre.
    /// Almacena en una variable el estado actual que va a cambiar para poder realizar comprobaciones en transiciones de estados.
    /// </summary>
    /// <param name="newState">Nuevo estado al que se va a cambiar.</param>
    public void ChangeState(IState newState)
    {
        PreviousState = CurrentState;
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    /// <summary>
    /// M�todo que lee la entrada de inputs del estado actual (si existe).
    /// </summary>
    public void HandleInput()
    {
        CurrentState?.HandleInput();
    }

    /// <summary>
    /// M�todo que actualiza la l�gica del estado actual (si existe).
    /// </summary>
    public void UpdateLogic()
    {
        CurrentState?.UpdateLogic();
    }

    /// <summary>
    /// M�todo que actualiza la f�sica del estado actual (si existe).
    /// </summary>
    public void UpdatePhysics()
    {
        CurrentState?.UpdatePhysics();
    }

    /// <summary>
    /// M�todo que recibe la entrada de una colisi�n de un trigger del estado actual (si existe).
    /// </summary>
    /// <param name="collider">El collider con el que choca el Player.</param>
    public void OnTriggerEnter(Collider collider)
    {
        CurrentState?.OnTriggerEnter(collider);
    }

    /// <summary>
    /// M�todo que recibe la salida de una colisi�n de un trigger del estado actual (si existe).
    /// </summary>
    /// <param name="collider">El collider del que sale el Player.</param>
    public void OnTriggerExit(Collider collider)
    {
        CurrentState?.OnTriggerExit(collider);
    }
    #endregion
}
