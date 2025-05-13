using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyStateMachine
 * AUTOR: Sara Yue Madruga Martín, Jone Sainz Egea
 * FECHA: 
 * DESCRIPCIÓN: Script que gestiona toda la lógica de la FSM y el funcionamiento modular de comportamientos con SO
 *              Hereda de StateMachine, donde se gestiona el cambio de estados y la ejecución del estado actual
 *              Al crearla llamando al constructor crea los nuevos estados. Mantiene las referencias a los estados.
 * VERSIÓN: 1.0. Script base para la gestión de la FSM
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
     * Constructor de la máquina de estados de los Enemigos.
     * Inicializa los estados para dejarlos preparados para los cambios de estado.
     * @param1 _enemy - Recibe una referencia del Enemy para poder acceder a su información en la FSM.
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
