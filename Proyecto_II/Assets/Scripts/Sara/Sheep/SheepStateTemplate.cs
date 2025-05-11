using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepStateTemplate
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 10/05/2025
 * DESCRIPCI�N: Clase abstracta que sirve como plantilla base para la m�quina de estados de las ovejas.
 *              Hereda de la interfaz IState, implementando sus m�todos.
 *              Al crealo recibe la referencia de la m�quina de estados.
 *              Se guarda la referencia a la m�quina de estados y se deja accesible para las clases que hereden de esta.
 * VERSI�N: 1.0. Script base para utilizar sus m�todos en los diferentes estados de las ovejas.
 */

public abstract class SheepStateTemplate : IState
{
    protected SheepStateMachine sheepStateMachine;

    public SheepStateTemplate(SheepStateMachine _sheepStateMachine)
    {
        sheepStateMachine = _sheepStateMachine;
    }

    #region M�todos Base de la M�quina de Estados
    public virtual void Enter()
    {
        EventsManager.CallNormalEvents("CallBeast", ListenPlayerWhistle);
    }

    public virtual void Exit()
    {
        EventsManager.StopCallNormalEvents("CallBeast", ListenPlayerWhistle);
    }

    public virtual void HandleInput() { }

    public virtual void UpdateLogic()
    {
        DetectPlayer();
    }

    public virtual void UpdatePhysics() { }

    public virtual void OnTriggerEnter(Collider collider) { }

    public virtual void OnTriggerExit(Collider collider) { }
    #endregion

    #region M�todos Propios Compartidos Todos Estados
    /// <summary>
    /// M�todo que comprueba si dentro de un rango est� Player.
    /// </summary>
    /// <returns>Si detecta a Player, devuelve True, si no, devuelve False.</returns>
    protected bool DetectPlayer()
    {
        float detectionRadius = 15f;
        Collider[] colliders = Physics.OverlapSphere(sheepStateMachine.Sheep.transform.position, detectionRadius); // Radio para detectar si el collider de Player est� dentro.

        foreach (Collider playerCollider in colliders)
        {
            if (playerCollider.CompareTag("Player"))
            {
                Debug.Log("Player detectado por las ovejas.");
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Si el m�todo de detecta a Player devuelve True, cambia al estado de saltar.
    /// </summary>
    private void ListenPlayerWhistle()
    {
        if (DetectPlayer())
        {
            Debug.Log("Escuchando a Player");
            sheepStateMachine.ChangeState(sheepStateMachine.SheepJumpState);
        }
    }
    #endregion
}
