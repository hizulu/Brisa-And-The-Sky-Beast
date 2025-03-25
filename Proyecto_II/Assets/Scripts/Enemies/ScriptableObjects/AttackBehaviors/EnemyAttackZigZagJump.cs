using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Zig Zag Jump", menuName = "Enemy Logic/Attack Logic/Ziz Zag Jump")]
public class EnemyAttackZigZagJump : EnemyAttackSOBase
{
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float distanceToStopAttackState = 5f;

    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float lateralOffset = 3f;
    [SerializeField] private float timeBetweenJumps = 0.5f;

    private float distanceToStopAttackStateSQR = 0f;


    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        distanceToStopAttackStateSQR = distanceToStopAttackState * distanceToStopAttackState;

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
            jumpTarget += (i % 2 == 0 ? right : -right) * lateralOffset; // Alterna lado de zig-zag con el desplazamiento lateral deseado

            JumpTo(jumpTarget);

            yield return new WaitForSeconds(timeBetweenJumps);
        }

        // Después de los tres zig-zags
        JumpTo(playerTransform.position);
        yield return new WaitForSeconds(0.2f); // Tiempo de espera de salto final
        Attack();
    }

    private void JumpTo(Vector3 target)
    {
        enemy.rb.velocity = Vector3.zero; // Resetea velocidad antes del nuevo salto
        Vector3 jumpDirection = (target - enemy.transform.position).normalized;
        enemy.rb.AddForce(jumpDirection * jumpForce + Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Attack()
    {
        Debug.Log("El enemigo ataca al jugador");
        // Called after three zig zags done
        // TODO: enemy.anim.SetTrigger("ataca");
        // TODO: play enemy attack sound depending on enemy
        // TODO: que el jugador reciba daño, llamar a función de player
    }
}
