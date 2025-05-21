using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttackMelee
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 
 * DESCRIPCI�N: Clase que define el comportamiento espec�fico de Ataque del Soldado en el que ataca a melee.
 *              Mientras el jugador no salga del rango de ataque, ataca por intervalos.
 *              Vuelve al estado de Chase si el jugador se aleja demasiado.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus m�todos y tiene acceso a sus variables.            
 * VERSI�N: 1.0. Script base con el comportamiento de ataque a melee.
 *              1.1. A�adido ataque a Bestia - Jone (20/05/2025)
 *              1.2. Rotaci�n para mirar al target mientras ataca - Jone (20/05/2025)
 */
[CreateAssetMenu(fileName = "Attack-Melee", menuName = "Enemy Logic/Attack Logic/Melee")]
public class EnemyAttackMelee : EnemyStateSOBase
{
    #region Variables   
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private float distanceToStopAttackState = 5f;
    private float distanceToStopAttackStateSQR = 0f; // Variable auxiliar para almacenar distancia evitando c�lculo de ra�z cuadrada cada frame.

    private Transform targetTransform;
    #endregion

    #region Sobreescritura de m�todos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Debug.Log("Entra en attack");

        distanceToStopAttackStateSQR = distanceToStopAttackState * distanceToStopAttackState;
        SetTarget();
        // Debug.Log("Has entrado en el estado de Attack Melee");
        enemy.agent.ResetPath();
        enemy.anim.SetTrigger("Attack");
        Attack();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        // Debug.Log("Has salido del estado de Attack Melee");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToTargetSQR = (enemy.transform.position - targetTransform.position).sqrMagnitude;

        LookAtTarget();

        // Si el jugador se aleja demasiado, vuelve al estado de Chase
        if (distanceToTargetSQR > distanceToStopAttackStateSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);
    }
    #endregion

    #region M�todos espec�ficos de EnemyAttackMelee
    private void SetTarget()
    {
        if (enemy.targetIsPlayer)
        {
            targetTransform = playerTransform;
        }
        else
        {
            targetTransform = beastTransform;
        }
    }

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

    private void Attack()
    {
        if (enemy.targetIsPlayer)
        {
            //Debug.Log("triggerea evento de hacer da�o a player");
            EventsManager.TriggerSpecialEvent<float>("OnAttackPlayer", _attackDamage);
        }
        else
            EventsManager.TriggerSpecialEvent<float>("OnAttackBeast", _attackDamage);

        // TODO: diferente da�o seg�n a qui�n golpee
        enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyRetreatState);
    }
    #endregion
}
