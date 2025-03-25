using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dump : MonoBehaviour
{
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float distanceToStopAttackState = 5f;

    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float lateralOffset = 3f; // Cuánto se desvía a los lados en zig-zag
    [SerializeField] private float timeBetweenJumps = 0.5f;

    private float distanceToStopAttackStateSQR = 0f;
    private Rigidbody enemyRigidbody;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        distanceToStopAttackStateSQR = distanceToStopAttackState * distanceToStopAttackState;
        enemyRigidbody = enemy.GetComponent<Rigidbody>();

        enemy.StartCoroutine(DoThreeZigZags());
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        if (distanceToPlayerSQR > distanceToStopAttackStateSQR)
        {
            enemy.doAttack = false;
            enemy.doChase = true;
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
        Vector3 directionToPlayer = (playerTransform.position - startPosition).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, directionToPlayer).normalized; // Vector perpendicular para el zig-zag

        for (int i = 0; i < 3; i++)
        {
            Vector3 jumpTarget = startPosition + directionToPlayer * (i + 1) * 2f; // Avanza hacia el jugador
            jumpTarget += (i % 2 == 0 ? right : -right) * lateralOffset; // Alterna izquierda/derecha

            JumpTo(jumpTarget);

            yield return new WaitForSeconds(timeBetweenJumps); // Espera entre saltos
        }

        // Salto final hacia el jugador y ataque
        JumpTo(playerTransform.position);
        yield return new WaitForSeconds(timeBetweenJumps);
        Attack();
    }

    private void JumpTo(Vector3 target)
    {
        Vector3 jumpDirection = (target - enemy.transform.position).normalized;
        enemyRigidbody.velocity = Vector3.zero; // Resetea velocidad antes del nuevo salto
        enemyRigidbody.AddForce(jumpDirection * jumpForce + Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Attack()
    {
        Debug.Log("El enemigo ataca al jugador");
        // enemy.anim.SetTrigger("ataca");
        // enemy.PlayAttackSound();
        // player.ReceiveDamage(attackDamage);
    }
}
