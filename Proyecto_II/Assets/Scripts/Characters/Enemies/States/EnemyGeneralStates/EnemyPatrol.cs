/*
 * NOMBRE CLASE: EnemyPatrol
 * AUTOR: Sara Yue Madruga Mart�n, Jone Sainz Egea
 * FECHA: 11/03/2025
 * DESCRIPCI�N: Clase que define el estado de Patrol del enemigo.
 *              Hereda de EnemyStateTemplete, por lo que tiene acceso a la m�quina de estados y a Enemy.
 *              Se encarga de ejecutar la l�gica de la instancia espec�fica que contiene Enemy para el estado de Patrol.
 * VERSI�N: 1.0. Script base que ejecuta la l�gica de Patrol del enemigo
 */
public class EnemyPatrol : EnemyStateTemplate
{
    /*
     * Constructor del estado de Patrol del enemigo.
     * @param1 _stateMachine - Recibe una referencia de la m�quina de estados del enemigo para poder acceder a su informaci�n.
     */
    public EnemyPatrol(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        enemyStateMachine.Enemy.anim.SetBool("isMoving", true);
        enemyStateMachine.Enemy.EnemyPatrolBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.EnemyPatrolBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyPatrolBaseInstance.DoFrameUpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyPatrolBaseInstance.DoPhysicsLogic();
    }
}
