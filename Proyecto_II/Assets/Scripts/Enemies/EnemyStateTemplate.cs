using UnityEngine;

/*
 * NOMBRE CLASE: EnemyStateTemplate
 * AUTOR: Sara Yue Madruga Mart�n, Jone Sainz Egea
 * FECHA: 
 * DESCRIPCI�N: Clase abstracta que sirve de base para los estados del enemigo
 *              Hereda de la interfaz IState, implementando sus m�todos.
 *              Las clases que hereden de esta pueden implementar sus m�todos.
 *              Al crealo recibe la referencia de la m�quina de estados.
 *              Se guarda la referencia a la m�quina de estados y se deja accesible para las clases que hereden de esta.
 * VERSI�N: 1.0. Script base para usar como molde de los estados del enemigo
 */
public abstract class EnemyStateTemplate : IState
{
    protected EnemyStateMachine enemyStateMachine;

    /*
     * Constructor del EnemyStateTemplate
     * @param1 _enemyStateMachine - Recibe una referencia del EnemyStateMachine para que los estados puedan llamar al cambio de estado.
     */
    public EnemyStateTemplate(EnemyStateMachine _enemyStateMachine)
    {
        enemyStateMachine = _enemyStateMachine;
    }
    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput() { }

    public virtual void OnTriggerEnter(Collider collider) { }

    public virtual void OnTriggerExit(Collider collider) { }

    public virtual void UpdateLogic() { }

    public virtual void UpdatePhysics() { }
}
