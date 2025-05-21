using UnityEngine;

/*
 * NOMBRE CLASE: EnemyIdleStandStill
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Idle en el que el enemigo permanece quieto durante un tiempo aleatorio.
 *              Se define la duración de este estado de forma aleatoria entre un valor mínimo y un valor máximo.
 *              Vuelve al estado de patrulla después de completar el tiempo de espera.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. Script base con el comportamiento de esperar quieto durante un tiempo limitado.
 */
[CreateAssetMenu(fileName = "Idle-Stand Still", menuName = "Enemy Logic/Idle Logic/Stand Still")]
public class EnemyIdleStandStill : EnemyStateSOBase
{
    #region Variables
    [SerializeField] float minStillTime = 1f;
    [SerializeField] float maxStillTime = 5f;

    private float stillTime;
    #endregion

    #region Sobreescritura de métodos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.anim.SetBool("isIdle", true);
        stillTime = Random.Range(minStillTime, maxStillTime);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        enemy.SfxEnemy.PlayRandomSFX(EnemySFXType.Idle);
        stillTime -= Time.deltaTime;

        // Cuando completa el tiempo de espera vuelve al estado de patrulla
        if (stillTime <= 0)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyPatrolState);
    }

    public override void DoExitLogic()
    {
        enemy.SfxEnemy.StopSound(EnemySFXType.Idle);
        base.DoExitLogic();
        enemy.anim.SetBool("isIdle", false);
    }
    #endregion
}
