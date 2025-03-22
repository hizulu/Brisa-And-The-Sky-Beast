using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "Chase-Straight to Player", menuName = "Enemy Logic/Chase Logic/Straight to Player")]
public class EnemyChaseStraightToPlayer : EnemyChaseSOBase
{
    //[SerializeField] private float _movementSpeed = 1.75f;
    //[SerializeField] private float _stuckTimeThreshold = 2f;
    //[SerializeField] private float _stuckDuration = 3f;

    //private Vector3 _lastPosition;
    //private float _stuckTimer;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        //_lastPosition = enemy.transform.position;
        //_stuckTimer = 0f;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        //Vector2 moveDirection = (playerTransform.position - enemy.transform.position).normalized;
        //enemy.MoveEnemy(moveDirection * _movementSpeed);

        //// Comprueba si la posición del enemigo ha cambiado
        //if (Vector3.Distance(enemy.transform.position, _lastPosition) < 0.02f)
        //{
        //    _stuckTimer += Time.deltaTime;
        //}
        //else
        //{
        //    _stuckTimer = 0f;
        //}

        //// Actualiza la última posición para la comprobación de movimiento
        //_lastPosition = enemy.transform.position;

        //// Si el enemigo ha estado atascado más del tiempo permitido, vuelve al estado de idle
        //if (_stuckTimer >= _stuckTimeThreshold)
        //{
        //    EnemyStuck();
        //}

        //if (enemy.IsAggroed)
        //    enemy.PlaySonidosEnem(0); // Sonido idle
        //else
        //    SFXManager.instance.StopSFXLoop();
    }

    public override void DoPhysiscsLogic()
    {
        base.DoPhysiscsLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }

    private void EnemyStuck()
    {
        //enemy.Coroutine(HandleStuck());
    }

    //private IEnumerator HandleStuck()
    //{
    //    // Retroceder
    //    Vector2 backwardDirection = -(playerTransform.position - enemy.transform.position).normalized;
    //    float timer = 0f;

    //    while (timer < _stuckDuration / 2)
    //    {
    //        enemy.MoveEnemy(backwardDirection * _movementSpeed);
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    // Moverse en una dirección aleatoria
    //    Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    //    timer = 0f;

    //    while (timer < _stuckDuration / 2)
    //    {
    //        enemy.MoveEnemy(randomDirection * _movementSpeed);
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //}
}
