using UnityEngine;

/*
 * NOMBRE CLASE: EnemyChaseStraightToTarget
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCI�N: Clase que define el comportamiento espec�fico de Chase en el que el enemigo se mueve directo hacia el objetivo.
 *              Cambia a estado de Attack si se encuentra lo suficientemente cerca del objetivo para atacar.
 *              Vuelve al estado de Idle si el objetivo se aleja demasiado.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus m�todos y tiene acceso a sus variables.            
 * VERSI�N: 1.0. Script base con el comportamiento de perseguir al jugador.
 *              1.1. A�adido persecuci�n a Bestia (22/04/2025)
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

    // Variables auxiliares para almacenar distancias evitando c�lculos de ra�z cuadrada cada frame.
    private float targetAttackRangeSQR = 0f;
    private float targetLostRangeSQR = 0f;
    #endregion

    #region Sobreescriturta de m�todos de EnemyStateSOBase
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

        // Si est� en rango de ataque cambia a estado de ataque
        if (distanceToTargetSQR < targetAttackRangeSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyAttackState);
        // Si el objetivo se ha alejado mucho vuelve a estado de idle
        else if (distanceToTargetSQR > targetLostRangeSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyIdleState);
    }
    #endregion

    #region M�todos Espec�ficos de EnemyChaseStraightToTarget
    /*
     * M�todo que establece el objetivo del enemigo seg�n la variable de enemy
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

