using UnityEngine;

/*
 * NOMBRE CLASE: EnemyComboAttacksSOBase
 * AUTOR: Sara Yue Madruga Mart�n, Jone Sainz Egea
 * FECHA: 07/05/2025
 * DESCRIPCI�N: Clase abstracta que sirve de base para los Scriptable Object que definen los ataques de combo de cada enemigo.
 *              Hereda de ScriptableObject.
 *              Contiene la informaci�n de Enemy.
 *              Las clases que hereden de esta pueden sobreescribir sus m�todos y tienen acceso a sus variables.
 * VERSI�N: 1.0. Script que sirve de molde para los distintos ataques de combo de los enemigos.
 *              1.1. Hace da�o tambi�n a bestia y comprueba distancia antes de hacer da�o.
 *              1.2. Mira hacia el objetivo mientras ataca.
 */
public abstract class EnemyComboAttacksSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform targetTransform;
    private Transform playerTransform;
    private Transform beastTransform;

    public virtual void Initialize(Enemy _enemy, Transform _playerTransform, Transform _beastTransform)
    {
        enemy = _enemy;
        playerTransform = _playerTransform;
        beastTransform = _beastTransform;
        // Debug.Log("Leyendo SOBase de Combo Attack");

        if (enemy.targetIsPlayer)
        {
            targetTransform = playerTransform;
        } else
            targetTransform = beastTransform;
    }
    public virtual void FrameLogic()
    {
        LookAtTarget();
    }

    public virtual void EnemyAttack() { }

    public virtual void FinishAnimation() { }

    public virtual bool IsFinished() {  return false; }

    public virtual void Exit() { }

    public virtual void LookAtTarget()
    {
        Vector3 direction = (targetTransform.position - enemy.transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = lookRotation;
        }
    }
    /*
     * M�todo que da�a al objetivo del enemigo seg�n la variable de enemy
     * El objetivo se ha establecido anteriormente en el estado de Patrol
     */
    public virtual void AttackTarget(float damageAmount, float distanceToHit)
    {
        if (enemy.targetIsPlayer)
        {
            targetTransform = playerTransform;

            float distanceToTargetSQR = (enemy.transform.position - targetTransform.position).sqrMagnitude;

            if (distanceToTargetSQR < distanceToHit * distanceToHit)
                EventsManager.TriggerSpecialEvent<float>("OnAttackPlayer", damageAmount);
        }
        else
        {
            targetTransform = beastTransform;

            float distanceToTargetSQR = (enemy.transform.position - targetTransform.position).sqrMagnitude;

            if (distanceToTargetSQR < ((distanceToHit + 2f) * (distanceToHit + 2f)))
                EventsManager.TriggerSpecialEvent<float>("OnAttackBeast", damageAmount);
        }
    }
}