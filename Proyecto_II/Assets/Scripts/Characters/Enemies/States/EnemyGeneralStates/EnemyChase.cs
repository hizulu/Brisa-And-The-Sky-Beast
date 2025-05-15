/*
 * NOMBRE CLASE: EnemyChase
 * AUTOR: Sara Yue Madruga Martín, Jone Sainz Egea
 * FECHA: 11/03/2025
 * DESCRIPCIÓN: Clase que define el estado de Chase del enemigo.
 *              Hereda de EnemyStateTemplete, por lo que tiene acceso a la máquina de estados y a Enemy.
 *              Se encarga de ejecutar la lógica de la instancia específica que contiene Enemy para el estado de Chase.
 * VERSIÓN: 1.0. Script base que ejecuta la lógica de Chase del enemigo
 */
public class EnemyChase : EnemyStateTemplate
{
    /*
     * Constructor del estado de Chase del enemigo.
     * @param1 _stateMachine - Recibe una referencia de la máquina de estados del enemigo para poder acceder a su información.
     */
    public EnemyChase(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        enemyStateMachine.Enemy.anim.SetBool("isMoving", true);
        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.anim.SetBool("isMoving", false);
        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoFrameUpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoPhysicsLogic();
    }
}
