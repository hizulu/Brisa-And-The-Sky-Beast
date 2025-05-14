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
 */
public abstract class EnemyComboAttacksSOBase : ScriptableObject
{
    protected Enemy enemy;

    public virtual void Initialize(Enemy _enemy)
    {
        enemy = _enemy;
        // Debug.Log("Leyendo SOBase de Combo Attack");
    }

    public virtual void FrameLogic() { }

    public virtual void EnemyAttack() { }

    public virtual void FinishAnimation() { }

    public virtual bool IsFinished() {  return false; }

    public virtual void Exit() { }

    /*
     * M�todo que da�a al objetivo del enemigo seg�n la variable de enemy
     * El objetivo se ha establecido anteriormente en el estado de Patrol
     */
    public virtual void AttackTarget(float damageAmount)
    {
        if (enemy.targetIsPlayer)
        {
            EventsManager.TriggerSpecialEvent<float>("OnAttackPlayer", damageAmount);
        }
        else
        {
            EventsManager.TriggerSpecialEvent<float>("OnAttackBeast", damageAmount);
        }
    }
}