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

        DoThreeZigZags();
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

    private void DoThreeZigZags()
    {
        // TODO: calcula el primer punto dependiendo de la posición del jugador
        // Cuando llega a ese punto calcula el segundo punto del zig zag teniendo en cuenta que el jugador se ha podido mover
        // Lo mismo con el tercer punto del zig zag
        // Después del último punto salta directo al jugador y llama a la función attack (cuando ya está "encima" del jugador)
    }

    private void JumpTo(Vector3 target)
    {
        Vector3 jumpDirection = (target - enemy.transform.position).normalized;
        enemy.rb.velocity = Vector3.zero; // Resetea velocidad antes del nuevo salto
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
