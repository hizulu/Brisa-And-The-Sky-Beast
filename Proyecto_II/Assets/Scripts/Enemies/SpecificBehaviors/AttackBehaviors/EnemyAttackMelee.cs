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
 */
[CreateAssetMenu(fileName = "Attack-Melee", menuName = "Enemy Logic/Attack Logic/Melee")]
public class EnemyAttackMelee : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float _timeBetweenHits = 2f;
    [SerializeField] private float _attackDamage = 20f;

    private float _timer;

    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float distanceToStopAttackState = 5f;
    private float distanceToStopAttackStateSQR = 0f; // Variable auxiliar para almacenar distancia evitando c�lculo de ra�z cuadrada cada frame.
    #endregion

    #region Sobreescritura de m�todos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.anim.SetBool("isAttacking", true);
        distanceToStopAttackStateSQR = distanceToStopAttackState * distanceToStopAttackState;
        Debug.Log("Has entrado en el estado de Attack Melee");
        enemy.agent.ResetPath();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.anim.SetBool("isAttacking", false);
        Debug.Log("Has salido del estado de Attack Melee");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        // Gesti�n del tiempo entre ataques
        if (_timer > _timeBetweenHits)
        {
            _timer = 0f;
            // TODO: l�gica de ataque
        }

        _timer += Time.deltaTime;

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        // Si el jugador se aleja demasiado, vuelve al estado de Chase
        if (distanceToPlayerSQR > distanceToStopAttackStateSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);
    }
    #endregion
}
