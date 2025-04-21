using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttack
 * AUTOR: Sara Yue Madruga Mart�n, Jone Sainz Egea
 * FECHA: 
 * DESCRIPCI�N: Clase que define el estado de Attack del enemigo.
 *              Hereda de EnemyStateTemplete, por lo que tiene acceso a la m�quina de estados y a Enemy.
 *              Se encarga de ejecutar la l�gica de la instancia espec�fica que contiene Enemy para el estado de Attack.
 * VERSI�N: 1.0. Script base que ejecuta la l�gica de Attack del enemigo
 */
public class EnemyAttack : EnemyStateTemplate
{
    /*
     * Constructor del estado de Attack del enemigo.
     * @param1 _stateMachine - Recibe una referencia de la m�quina de estados del enemigo para poder acceder a su informaci�n.
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
