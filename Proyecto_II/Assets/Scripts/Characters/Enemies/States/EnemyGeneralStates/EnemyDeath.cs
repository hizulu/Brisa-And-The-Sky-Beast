using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyDeath
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 19/05/2025
 * DESCRIPCI�N: Clase que define el estado de Muerte del enemigo.
 *              Hereda de EnemyStateTemplete, por lo que tiene acceso a la m�quina de estados y a Enemy.
 *              Se encarga de ejecutar la l�gica de la instancia espec�fica que contiene Enemy para el estado de Muerte.
 * VERSI�N: 1.0. Script base que ejecuta la l�gica de Muerte del enemigo.
 */

public class EnemyDeath : EnemyStateTemplate
{
    public EnemyDeath(EnemyStateMachine _enemyStateMachine) : base(_enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Has entrado en el script Base de Death");
        enemyStateMachine.Enemy.anim.SetBool("isMoving", false);
        enemyStateMachine.Enemy.EnemyDeathBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.EnemyDeathBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyDeathBaseInstance.DoFrameUpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyDeathBaseInstance.DoPhysicsLogic();
    }
}
