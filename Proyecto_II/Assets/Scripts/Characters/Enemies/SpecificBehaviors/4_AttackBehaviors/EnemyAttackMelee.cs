using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttackMelee
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Ataque del Soldado en el que ataca a melee.
 *              Mientras el jugador no salga del rango de ataque, ataca por intervalos.
 *              Vuelve al estado de Chase si el jugador se aleja demasiado.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. Script base con el comportamiento de ataque a melee.
 */
[CreateAssetMenu(fileName = "Attack-Melee", menuName = "Enemy Logic/Attack Logic/Melee")]
public class EnemyAttackMelee : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float _timeBetweenHits = 2f;
    [SerializeField] private float _attackDamage = 10f;
    private float _timer;
    [SerializeField] private float distanceToStopAttackState = 5f;
    private float distanceToStopAttackStateSQR = 0f; // Variable auxiliar para almacenar distancia evitando cálculo de raíz cuadrada cada frame.
    #endregion

    #region Sobreescritura de métodos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        _timer = _timeBetweenHits;
        
        distanceToStopAttackStateSQR = distanceToStopAttackState * distanceToStopAttackState;
        Debug.Log("Has entrado en el estado de Attack Melee");
        enemy.agent.ResetPath();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Debug.Log("Has salido del estado de Attack Melee");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        // Si el jugador se aleja demasiado, vuelve al estado de Chase
        if (distanceToPlayerSQR > distanceToStopAttackStateSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);

        // Gestión del tiempo entre ataques
        _timer += Time.deltaTime;

        if (_timer > _timeBetweenHits)
        {
            _timer = 0f;
            enemy.anim.SetTrigger("Attack");
            EventsManager.TriggerSpecialEvent<float>("OnAttackPlayer", _attackDamage);
            // TODO: lógica de ataque
        }
    }
    #endregion
}
