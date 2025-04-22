using UnityEngine;

/*
 * NOMBRE CLASE: EnemyChaseStraightToTarget
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Chase en el que el enemigo se mueve directo hacia el objetivo.
 *              Cambia a estado de Attack si se encuentra lo suficientemente cerca del objetivo para atacar.
 *              Vuelve al estado de Idle si el objetivo se aleja demasiado.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. Script base con el comportamiento de perseguir al jugador.
 *              1.1. Añadido persecución a Bestia (22/04/2025)
 */
[CreateAssetMenu(fileName = "Chase-Straight to Target", menuName = "Enemy Logic/Chase Logic/Straight to Target")]
public class EnemyChaseStraightToTarget : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float chasingSpeed = 6f;
    [SerializeField] private float playerAttackRange = 7f; // Distancia a la que se tiene que encontrar el jugador para que el enemigo pase a atacar
    [SerializeField] private float playerLostRange = 20f; // Distancia a la que se tiene que encontrar el jugador para que el enemigo deje de perseguirlo
    [SerializeField] private float beastAttackRange = 10f; // Distancia a la que se tiene que encontrar la bestia para que el enemigo pase a atacarla
    [SerializeField] private float beastLostRange = 15f; // Distancia a la que se tiene que encontrar la bestia para que el enemigo deje de perseguirla

    private Transform targetTransform; // Variable auxiliar para definir el objetivo

    // Variables auxiliares para almacenar distancias evitando cálculos de raíz cuadrada cada frame.
    private float targetAttackRangeSQR = 0f;
    private float targetLostRangeSQR = 0f;
    #endregion

    #region Sobreescriturta de métodos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();


        enemy.anim.SetBool("isChasing", true);

        enemy.agent.speed = chasingSpeed;

        SetTarget();

        enemy.agent.SetDestination(targetTransform.position);
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

        float distanceToTargetSQR = (enemy.transform.position - targetTransform.position).sqrMagnitude;

        enemy.agent.SetDestination(targetTransform.position); // Actualiza el destino para seguir al objetivo

        // Si está en rango de ataque cambia a estado de ataque
        if (distanceToTargetSQR < targetAttackRangeSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyAttackState);
        // Si el objetivo se ha alejado mucho vuelve a estado de idle
        else if (distanceToTargetSQR > targetLostRangeSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyIdleState);
    }
    #endregion

    #region Métodos Específicos de EnemyChaseStraightToTarget
    /*
     * Método que establece el objetivo del enemigo según la variable de enemy
     * El objetivo se ha establecido anteriormente en el estado de Patrol
     */
    private void SetTarget()
    {
        if (enemy.targetIsPlayer)
        {
            targetTransform = playerTransform;
            targetAttackRangeSQR = playerAttackRange * playerAttackRange;
            targetLostRangeSQR = playerLostRange * playerLostRange;
        }
        else
        {
            targetTransform = beastTransform;
            targetAttackRangeSQR = beastAttackRange * beastAttackRange;
            targetLostRangeSQR = beastLostRange * beastLostRange;
        }
    }
    #endregion
}

