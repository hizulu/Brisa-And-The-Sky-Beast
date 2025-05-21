using UnityEngine;

/*
 * NOMBRE CLASE: EnemyIdleStandStill
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCI�N: Clase que define el comportamiento espec�fico de Idle en el que el enemigo permanece quieto durante un tiempo aleatorio.
 *              Se define la duraci�n de este estado de forma aleatoria entre un valor m�nimo y un valor m�ximo.
 *              Vuelve al estado de patrulla despu�s de completar el tiempo de espera.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus m�todos y tiene acceso a sus variables.            
 * VERSI�N: 1.0. Script base con el comportamiento de esperar quieto durante un tiempo limitado.
 */
[CreateAssetMenu(fileName = "Idle-Stand Still", menuName = "Enemy Logic/Idle Logic/Stand Still")]
public class EnemyIdleStandStill : EnemyStateSOBase
{
    #region Variables
    [SerializeField] float minStillTime = 1f;
    [SerializeField] float maxStillTime = 5f;

    private float stillTime;
    #endregion

    #region Sobreescritura de m�todos de EnemyStateSOBase
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
