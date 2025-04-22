using UnityEngine;

/*
 * NOMBRE CLASE: EnemyTargetDetectionSimpleRange
 * AUTOR: Jone Sainz Egea
 * FECHA: 22/04/2025
 * DESCRIPCI�N: Clase que define la detecci�n espec�fica de objetivo del enemigo dependiendo de un rango simple de detecci�n.
 *              Si ambos objetivos posibles est�n dentro del rango, prioriza la detecci�n del jugador.
 *              Hereda de EnemyTargetDetectionSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus m�todos y tiene acceso a sus variables.  
 * VERSI�N: 1.0. Script base con la detecci�n de objetivos dentro de un rango.
 */
[CreateAssetMenu(fileName = "Target-By Simple Range", menuName = "Enemy Logic/Detection Logic/Simple Range")]
public class EnemyTargetDetectionSimpleRange : EnemyTargetDetectionSOBase
{
    [SerializeField] private float targetDetectionRange = 12f;

    private float targetDetectionRangeSQR = 0f;

    public override void Initialize(Enemy enemy)
    {
        base.Initialize(enemy);

        targetDetectionRangeSQR = targetDetectionRange * targetDetectionRange;
    }

    /*
     * M�todo que busca si el jugador o la bestia est�n dentro del rango de detecci�n
     * Prioriza la detecci�n del jugador
     * Establece el booleano de enemy que se�ala cu�l es el enemigo
     * @return bool true si hay un objetivo dentro del rango, false si no
     */
    public override bool LookForTarget()
    {
        // Player is within detection range
        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;
        if (distanceToPlayerSQR < targetDetectionRangeSQR)
        {
            Debug.Log("Player is within detection range");
            enemy.targetIsPlayer = true;
            return true;
        }

        // Beast is within detection range
        float distanceToBeastSQR = (enemy.transform.position - beastTransform.position).sqrMagnitude;
        if (distanceToBeastSQR < targetDetectionRangeSQR)
        {
            Debug.Log("Beast is within detection range");
            enemy.targetIsPlayer = false;
            return true;
        }

        // No target detected within detection range
        Debug.Log("No target detected within detection range");
        return false;
    }
}
