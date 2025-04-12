using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Attack-Zig Zag Jump", menuName = "Enemy Logic/Attack Logic/Ziz Zag Jump")]
public class EnemyAttackZigZagJump : EnemyAttackSOBase
{
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float distanceToStopAttackState = 8f;

    [SerializeField] private float jumpDuration = 0.6f;
    [SerializeField] private float lateralOffset = 2f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float distanceToHit = 2f;

    private bool isAttacking = false;

    private float distanceToStopAttackStateSQR = 0f;

    public static event Action<float> OnAttackPlayer;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        distanceToStopAttackStateSQR = distanceToStopAttackState * distanceToStopAttackState;

        if (!isAttacking)
        {
            isAttacking = true;
            enemy.agent.enabled = false; // Desactiva NavMeshAgent para saltos manuales
            enemy.StartCoroutine(DoThreeZigZags());
        }
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        // Si se sale del estado, asegurarse de reactivar el agent si se interrumpe el ataque
        if (!enemy.agent.enabled)
            enemy.agent.enabled = true;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        if (!isAttacking && distanceToPlayerSQR > distanceToStopAttackStateSQR)
        {
            enemy.doAttack = false;
            enemy.doChase = true;
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);
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

    private IEnumerator DoThreeZigZags()
    {
        Vector3 startPosition = enemy.transform.position;
        Vector3 finalJumpTarget = Vector3.zero;

        float[] distances = { 0.75f, 0.5f, 0.25f };
        float[] lateralOffsets = { lateralOffset, lateralOffset * 0.66f, lateralOffset * 0.33f };

        for (int i = 0; i < 3; i++)
        {
            Vector3 directionToTarget = (playerTransform.position - enemy.transform.position).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, directionToTarget).normalized;

            Vector3 jumpTarget = enemy.transform.position + directionToTarget * distances[i] * Vector3.Distance(enemy.transform.position, playerTransform.position);
            jumpTarget += (i % 2 == 0 ? right : -right) * lateralOffsets[i];

            // Justo antes del tercer salto, prepara el salto de ataque para que el jugador pueda evitarlo
            if (i == 2)
            {
                finalJumpTarget = playerTransform.position - directionToTarget * stopDistance;
            }
            enemy.anim.SetTrigger("Jump");
            yield return MoveInArc(enemy.transform.position, jumpTarget, jumpHeight);
            enemy.transform.position = jumpTarget;
        }

        // Realiza el salto de ataque (con target ya calculado anteriormente)
        yield return MoveInArc(enemy.transform.position, finalJumpTarget, jumpHeight);
        enemy.transform.position = finalJumpTarget;

        Attack();
    }

    private IEnumerator MoveInArc(Vector3 start, Vector3 end, float height)
    {
        float elapsedTime = 0;
        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            Vector3 position = Vector3.Lerp(start, end, t);
            position.y += Mathf.Sin(t * Mathf.PI) * height;
            enemy.transform.position = position;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        enemy.transform.position = end;
    }

    private void Attack()
    {
        Debug.Log("El enemigo ataca al jugador");

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        if (distanceToPlayerSQR < distanceToHit * distanceToHit)
        {
            Debug.Log("En distancia para atacar");
            OnAttackPlayer?.Invoke(attackDamage);
        }
        else
            Debug.Log("Fuera de rango de ataque");

        isAttacking = false;
        enemy.agent.enabled = true; // Reactiva el agente

        enemy.doRetreat = true;
        enemy.doAttack = false;

        enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyRetreatState);

        Debug.Log("Debería salir del estado de ataque zig-zag");
    }
}
