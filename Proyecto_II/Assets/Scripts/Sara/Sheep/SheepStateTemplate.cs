using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepStateTemplate
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/05/2025
 * DESCRIPCIÓN: Clase abstracta que sirve como plantilla base para la máquina de estados de las ovejas.
 *              Hereda de la interfaz IState, implementando sus métodos.
 *              Al crealo recibe la referencia de la máquina de estados.
 *              Se guarda la referencia a la máquina de estados y se deja accesible para las clases que hereden de esta.
 * VERSIÓN: 1.0. Script base para utilizar sus métodos en los diferentes estados de las ovejas.
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
