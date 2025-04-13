using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase-Run to Player", menuName = "Enemy Logic/Chase Logic/Run to Player")]
public class EnemyChaseRunToPlayer : EnemyChaseSOBase
{
    [SerializeField] private float chasingSpeed = 4f; // Velocidad a la que persigue el enemigo.
    [SerializeField] private float playerAttackRange = 3f; // �rea donde el enemigo pasa a atacar si el jugador entra dentro de �l.
    [SerializeField] private float playerLostRange = 15f; // �rea donde el enemigo deja de perseguir al jugador si este sale de �l.

    private float playerAttackRangeSQR = 0f; // Variable auxiliar para almacenar el rango de ataque del jugador (para evitar la ra�z cuadrada).
    private float playerLostRangeSQR = 0f; // Variable auxiliar para almacenar el rango de dejar de perseguir al jugador (para evitar la ra�z cuadrada).

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
        enemy.anim.SetBool("isChasing", false);
        enemy.agent.ResetPath();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        enemy.agent.SetDestination(playerTransform.position); // Actualiza el destino para seguir al jugador

        if (distanceToPlayerSQR < playerAttackRangeSQR)
        {
            Debug.Log("Deber�a atacar a Brisa");
            enemy.doAttack = true;
            enemy.doChase = false;
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyAttackState);
        }
        else if (distanceToPlayerSQR > playerLostRangeSQR)
        {
            Debug.Log("Deber�a volver a idle");
            enemy.doIdle = true;
            enemy.doChase = false;
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyIdleState);
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
