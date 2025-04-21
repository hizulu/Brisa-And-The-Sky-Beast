using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttack
 * AUTOR: Sara Yue Madruga Martín, Jone Sainz Egea
 * FECHA: 
 * DESCRIPCIÓN: Clase que define el estado de Attack del enemigo.
 *              Hereda de EnemyStateTemplete, por lo que tiene acceso a la máquina de estados y a Enemy.
 *              Se encarga de ejecutar la lógica de la instancia específica que contiene Enemy para el estado de Attack.
 * VERSIÓN: 1.0. Script base que ejecuta la lógica de Attack del enemigo
 */
public class EnemyAttack : EnemyStateTemplate
{
    /*
     * Constructor del estado de Attack del enemigo.
     * @param1 _stateMachine - Recibe una referencia de la máquina de estados del enemigo para poder acceder a su información.
     */
    public EnemyAttack(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoFrameUpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoPhysicsLogic();
    }
}
