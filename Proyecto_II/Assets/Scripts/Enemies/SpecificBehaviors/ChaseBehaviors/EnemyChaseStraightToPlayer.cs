using UnityEngine;

/*
 * NOMBRE CLASE: EnemyChaseStraightToPlayer
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Chase en el que el enemigo se mueve directo hacia el jugador.
 *              Cambia a estado de Attack si se encuentra lo suficientemente cerca para atacar.
 *              Vuelve al estado de Idle si el jugador se aleja demasiado.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. Script base con el comportamiento de perseguir al jugador.
 */
[CreateAssetMenu(fileName = "Chase-Straight to Player", menuName = "Enemy Logic/Chase Logic/Straight to Player")]
public class EnemyChaseStraightToPlayer : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float chasingSpeed = 6f;
    [SerializeField] private float playerAttackRange = 7f; // Distancia a la que se tiene que encontrar el jugador para que el enemigo pase a atacar
    [SerializeField] private float playerLostRange = 20f; // Distancia a la que se tiene que encontrar el jugador para que el enemigo deje de perseguirlo

    // Variables auxiliares para almacenar distancias evitando cálculos de raíz cuadrada cada frame.
    private float playerAttackRangeSQR = 0f;
    private float playerLostRangeSQR = 0f;
    #endregion

    #region Sobreescriturta de métodos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        playerAttackRangeSQR = playerAttackRange * playerAttackRange;
        playerLostRangeSQR = playerLostRange * playerLostRange;

        enemy.anim.SetBool("isChasing", true);

        enemy.agent.speed = chasingSpeed;

        enemy.agent.SetDestination(playerTransform.position);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.anim.SetBool("isChasing,", false);
        enemy.agent.ResetPath();
    }    

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        enemy.agent.SetDestination(playerTransform.position); // Actualiza el destino para seguir al jugador

        // Si está en rango de ataque cambia a estado de ataque
        if (distanceToPlayerSQR < playerAttackRangeSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyAttackState);
        // Si el jugador se ha alejado mucho vuelve a estado de idle
        else if (distanceToPlayerSQR > playerLostRangeSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyIdleState);
    }
    #endregion
}

