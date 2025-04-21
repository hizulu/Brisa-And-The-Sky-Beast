using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyIdle
 * AUTOR: Sara Yue Madruga Martín, Jone Sainz Egea
 * FECHA: 
 * DESCRIPCIÓN: Clase que define el estado de Idle del enemigo.
 *              Hereda de EnemyStateTemplete, por lo que tiene acceso a la máquina de estados y a Enemy.
 *              Se encarga de ejecutar la lógica de la instancia específica que contiene Enemy para el estado de Idle.
 * VERSIÓN: 1.0. Script base que ejecuta la lógica de Idle del enemigo
 */
public class EnemyIdle : EnemyStateTemplate
{
    /*
     * Constructor del estado de Idle del enemigo.
     * @param1 _stateMachine - Recibe una referencia de la máquina de estados del enemigo para poder acceder a su información.
     */
    public EnemyIdle(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    { 
        base.Enter();

        enemyStateMachine.Enemy.anim.SetBool("isMoving", false);
        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoFrameUpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoPhysicsLogic();
    }
}
