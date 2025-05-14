using UnityEngine;

/*
 * NOMBRE CLASE: EnemyComboAttacksSOBase
 * AUTOR: Sara Yue Madruga Martín, Jone Sainz Egea
 * FECHA: 07/05/2025
 * DESCRIPCIÓN: Clase abstracta que sirve de base para los Scriptable Object que definen los ataques de combo de cada enemigo.
 *              Hereda de ScriptableObject.
 *              Contiene la información de Enemy.
 *              Las clases que hereden de esta pueden sobreescribir sus métodos y tienen acceso a sus variables.
 * VERSIÓN: 1.0. Script que sirve de molde para los distintos ataques de combo de los enemigos.
 *              1.1. Hace daño también a bestia y comprueba distancia antes de hacer daño
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
    }

    public virtual void FrameLogic() { }

    public virtual void EnemyAttack() { }

    public virtual void FinishAnimation() { }

    public virtual bool IsFinished() {  return false; }

    public virtual void Exit() { }

    /*
     * Método que daña al objetivo del enemigo según la variable de enemy
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

            if (distanceToTargetSQR < distanceToHit * distanceToHit)
                EventsManager.TriggerSpecialEvent<float>("OnAttackBeast", damageAmount);
        }
    }
}