/*
 * NOMBRE CLASE: EnemyRetreat
 * AUTOR: Sara Yue Madruga Martín, Jone Sainz Egea
 * FECHA: 11/03/2025
 * DESCRIPCIÓN: Clase que define el estado de Retreat del enemigo.
 *              Hereda de EnemyStateTemplete, por lo que tiene acceso a la máquina de estados y a Enemy.
 *              Se encarga de ejecutar la lógica de la instancia específica que contiene Enemy para el estado de Retreat.
 * VERSIÓN: 1.0. Script base que ejecuta la lógica de Retreat del enemigo
 */
public class EnemyRetreat : EnemyStateTemplate
{
    /*
     * Constructor del estado de Retreat del enemigo.
     * @param1 _stateMachine - Recibe una referencia de la máquina de estados del enemigo para poder acceder a su información.
     */
    public EnemyRetreat(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        enemyStateMachine.Enemy.anim.SetBool("isMoving", true);
        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoFrameUpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoPhysicsLogic();
    }
}
