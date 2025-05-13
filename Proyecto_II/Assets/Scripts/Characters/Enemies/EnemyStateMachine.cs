using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyStateMachine
 * AUTOR: Sara Yue Madruga Mart�n, Jone Sainz Egea
 * FECHA: 
 * DESCRIPCI�N: Script que gestiona toda la l�gica de la FSM y el funcionamiento modular de comportamientos con SO
 *              Hereda de StateMachine, donde se gestiona el cambio de estados y la ejecuci�n del estado actual
 *              Al crearla llamando al constructor crea los nuevos estados. Mantiene las referencias a los estados.
 * VERSI�N: 1.0. Script base para la gesti�n de la FSM
 */
public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy { get; }
    public EnemyIdle EnemyIdleState { get; }
    public EnemyPatrol EnemyPatrolState { get; }
    public EnemyChase EnemyChaseState { get; }
    public EnemyAttack EnemyAttackState { get; }
    public EnemyRetreat EnemyRetreatState { get; }

    /*
     * Constructor de la m�quina de estados de los Enemigos.
     * Inicializa los estados para dejarlos preparados para los cambios de estado.
     * @param1 _enemy - Recibe una referencia del Enemy para poder acceder a su informaci�n en la FSM.
     */
    public EnemyStateMachine(Enemy _enemy)
    {
        Enemy = _enemy;

        EnemyIdleState = new EnemyIdle(this);
        EnemyPatrolState = new EnemyPatrol(this);
        EnemyChaseState = new EnemyChase(this);
        EnemyAttackState = new EnemyAttack(this);
        EnemyRetreatState = new EnemyRetreat(this);
    }
}
