using UnityEngine;

/*
 * NOMBRE CLASE: EnemyTargetDetectionSimpleRange
 * AUTOR: Jone Sainz Egea
 * FECHA: 22/04/2025
 * DESCRIPCIÓN: Clase que define la detección específica de objetivo del enemigo dependiendo de un rango simple de detección.
 *              Si ambos objetivos posibles están dentro del rango, prioriza la detección del jugador.
 *              Hereda de EnemyTargetDetectionSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.  
 * VERSIÓN: 1.0. Script base con la detección de objetivos dentro de un rango.
 */
[CreateAssetMenu(fileName = "Target-By Simple Range", menuName = "Enemy Logic/Detection Logic/Simple Range")]
public class EnemyTargetDetectionSimpleRange : EnemyTargetDetectionSOBase
{
    [SerializeField] private float targetDetectionRange = 12f;

    private float targetDetectionRangeSQR = 0f;

    private float playerHealthPercentage;
    private float beastHealthPercentage;

    public override void Initialize(Enemy enemy)
    {
        base.Initialize(enemy);

        targetDetectionRangeSQR = targetDetectionRange * targetDetectionRange;

        playerHealthPercentage = player.Data.StatsData.CurrentHealth / player.Data.StatsData.MaxHealth * 100;
        beastHealthPercentage = beast.currentHealth / beast.maxHealth * 100;
    }

    /*
     * Método que busca si el jugador o la bestia están dentro del rango de detección
     * Prioriza la detección del jugador
     * Establece el booleano de enemy que señala cuál es el enemigo
     * @return bool true si hay un objetivo dentro del rango, false si no
     */
    public override bool LookForTarget()
    {
        // Player is within detection range
        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;
        if (distanceToPlayerSQR < targetDetectionRangeSQR)
        {
            Debug.Log("Player is within detection range");
            if (playerHealthPercentage <= 0f)
                return false; // Player is dead
            enemy.targetIsPlayer = true;
            return true;
        }

        // Beast is within detection range
        float distanceToBeastSQR = (enemy.transform.position - beastTransform.position).sqrMagnitude;
        if (distanceToBeastSQR < targetDetectionRangeSQR)
        {
            Debug.Log("Beast is within detection range");
            if (beastHealthPercentage <= 0f)
                return false; // Beast is dead
            enemy.targetIsPlayer = false;
            return true;
        }

        // No target detected within detection range
        Debug.Log("No target detected within detection range");
        return false;
    }
}
