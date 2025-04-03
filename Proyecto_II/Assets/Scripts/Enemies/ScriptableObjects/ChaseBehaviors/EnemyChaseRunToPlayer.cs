using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase-Run to Player", menuName = "Enemy Logic/Chase Logic/Run to Player")]
public class EnemyChaseRunToPlayer : EnemyChaseSOBase
{
    [SerializeField] private float chasingSpeed = 4f; // Velocidad a la que persigue el enemigo.
    private Vector3 direction;
    [SerializeField] private float playerAttackRange = 1.5f; // Área donde el enemigo pasa a atacar si el jugador entra dentro de él.
    [SerializeField] private float playerLostRange = 10f; // Área donde el enemigo deja de perseguir al jugador si este sale de él.

    private float playerAttackRangeSQR = 0f; // Variable auxiliar para almacenar el rango de ataque del jugador (para evitar la raíz cuadrada).
    private float playerLostRangeSQR = 0f; // Variable auxiliar para almacenar el rango de dejar de perseguir al jugador (para evitar la raíz cuadrada).

    private Vector3 _direction;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.anim.SetBool("isChasing", true);
        playerAttackRangeSQR = playerAttackRange * playerAttackRange;
        playerLostRangeSQR = playerLostRange * playerLostRange;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.anim.SetBool("isChasing", false);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        if (distanceToPlayerSQR < playerAttackRangeSQR)
        {
            Debug.Log("Debería atacar a Brisa");
            enemy.doAttack = true;
            enemy.doChase = false;

            
        }
        else if (distanceToPlayerSQR > playerLostRangeSQR)
        {
            Debug.Log("Debería volver a idle");
            enemy.doIdle = true;
            enemy.doChase = false;
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        SetEnemyMovement();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }

    private void SetEnemyMovement()
    {
        _direction = (playerTransform.position - enemy.transform.position).normalized;

        enemy.MoveEnemy(_direction * chasingSpeed);

        if (_direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
