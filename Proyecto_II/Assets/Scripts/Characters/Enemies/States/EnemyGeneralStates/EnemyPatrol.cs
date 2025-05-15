/*
 * NOMBRE CLASE: EnemyPatrol
 * AUTOR: Sara Yue Madruga Martín, Jone Sainz Egea
 * FECHA: 11/03/2025
 * DESCRIPCIÓN: Clase que define el estado de Patrol del enemigo.
 *              Hereda de EnemyStateTemplete, por lo que tiene acceso a la máquina de estados y a Enemy.
 *              Se encarga de ejecutar la lógica de la instancia específica que contiene Enemy para el estado de Patrol.
 * VERSIÓN: 1.0. Script base que ejecuta la lógica de Patrol del enemigo
 */
public class EnemyPatrol : EnemyStateTemplate
{
    /*
     * Constructor del estado de Patrol del enemigo.
     * @param1 _stateMachine - Recibe una referencia de la máquina de estados del enemigo para poder acceder a su información.
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
