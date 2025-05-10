using UnityEngine;

/*
 * NOMBRE CLASE (Abstracta): StateMachine
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase abstracta que gestiona el estado actual y las transciones de los diferentes estados.
 * VERSIÓN: 1.0. Métodos básicos que gestionan los estados y sus transiciones.
 * VERSIÓN: 2.0. Creación de la variable "PreviousState" para establecer condiciones de transiciones de estados.
*/

public abstract class StateMachine
{
    #region Variables
    public IState CurrentState { get; private set; }
    public IState PreviousState {  get; private set; }
    #endregion

    #region Métodos Gestión Estados SM
    /// <summary>
    /// Método que se encarga de cambiar el estado actual por el nuevo que entre.
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
    /// Método que lee la entrada de inputs del estado actual (si existe).
    /// </summary>
    public void HandleInput()
    {
        CurrentState?.HandleInput();
    }

    /// <summary>
    /// Método que actualiza la lógica del estado actual (si existe).
    /// </summary>
    public void UpdateLogic()
    {
        CurrentState?.UpdateLogic();
    }

    /// <summary>
    /// Método que actualiza la física del estado actual (si existe).
    /// </summary>
    public void UpdatePhysics()
    {
        CurrentState?.UpdatePhysics();
    }

    /// <summary>
    /// Método que recibe la entrada de una colisión de un trigger del estado actual (si existe).
    /// </summary>
    /// <param name="collider">El collider con el que choca el Player.</param>
    public void OnTriggerEnter(Collider collider)
    {
        CurrentState?.OnTriggerEnter(collider);
    }

    /// <summary>
    /// Método que recibe la salida de una colisión de un trigger del estado actual (si existe).
    /// </summary>
    /// <param name="collider">El collider del que sale el Player.</param>
    public void OnTriggerExit(Collider collider)
    {
        CurrentState?.OnTriggerExit(collider);
    }
    #endregion
}
