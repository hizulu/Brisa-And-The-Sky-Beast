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

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput() { }

    public virtual void OnTriggerEnter(Collider collider) { }

    public virtual void OnTriggerExit(Collider collider) { }

    public virtual void UpdateLogic() { }

    public virtual void UpdatePhysics() { }
}
