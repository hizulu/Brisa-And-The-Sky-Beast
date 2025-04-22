using UnityEngine;

/*
 * NOMBRE CLASE: EnemyStateTemplate
 * AUTOR: Sara Yue Madruga Martín, Jone Sainz Egea
 * FECHA: 
 * DESCRIPCIÓN: Clase abstracta que sirve de base para los estados del enemigo
 *              Hereda de la interfaz IState, implementando sus métodos.
 *              Las clases que hereden de esta pueden implementar sus métodos.
 *              Al crealo recibe la referencia de la máquina de estados.
 *              Se guarda la referencia a la máquina de estados y se deja accesible para las clases que hereden de esta.
 * VERSIÓN: 1.0. Script base para usar como molde de los estados del enemigo
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
