using UnityEngine;

/*
 * NOMBRE CLASE: EnemyTargetDetectionSimpleRange
 * AUTOR: Jone Sainz Egea
 * FECHA: 22/04/2025
 * DESCRIPCI�N: Clase que define la detecci�n espec�fica de objetivo del enemigo dependiendo de un rango simple de detecci�n.
 *              Si ambos objetivos posibles est�n dentro del rango, prioriza la detecci�n del jugador.
 *              Hereda de EnemyTargetDetectionSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus m�todos y tiene acceso a sus variables.  
 * VERSI�N: 1.0. Script base con la detecci�n de objetivos dentro de un rango.
 *          1.1. Sara Yue Madruga Mart�n - Disminuci�n del tama�o de detecci�n de Player cuando este est� en modo sigilo. (17/05/2025).
 */
[CreateAssetMenu(fileName = "Target-By Simple Range", menuName = "Enemy Logic/Detection Logic/Simple Range")]
public class EnemyTargetDetectionSimpleRange : EnemyTargetDetectionSOBase
{
    [SerializeField] private float baseTargetDetectionRange = 12f;
    [Range(0.1f, 1f)][SerializeField] private float targetDetectionModifier = 0.5f;
    private float targetDetectionRange;

    private float targetDetectionRangeSQR = 0f;

    private float playerHealthPercentage;
    private float beastHealthPercentage;

    public override void Initialize(Enemy enemy)
    {
        targetDetectionRange = baseTargetDetectionRange;

        base.Initialize(enemy);

        //targetDetectionRangeSQR = targetDetectionRange * targetDetectionRange;

        playerHealthPercentage = player.Data.StatsData.CurrentHealth / player.Data.StatsData.MaxHealth * 100;
        beastHealthPercentage = beast.currentHealth / beast.maxHealth * 100;

        ChangeRangeDetection();
        //Debug.Log("Normal rango" + enemy.name + targetDetectionRange);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
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
            // Debug.Log("Player is within detection range");
            if (playerHealthPercentage <= 0f)
                return false; // Player is dead
            enemy.targetIsPlayer = true;
            return true;
        }

        // Beast is within detection range
        float distanceToBeastSQR = (enemy.transform.position - beastTransform.position).sqrMagnitude;
        if (distanceToBeastSQR < targetDetectionRangeSQR)
        {
            // Debug.Log("Beast is within detection range");
            if (beastHealthPercentage <= 0f)
                return false; // Beast is dead
            enemy.targetIsPlayer = false;
            return true;
        }

        // No target detected within detection range
        // Debug.Log("No target detected within detection range");
        return false;
    }

   

    protected void ChangeRangeDetection()
    {
        if (isCrouching)
        {
            targetDetectionRange = baseTargetDetectionRange * targetDetectionModifier;
            //Debug.Log("Reducci�n de rango" + enemy.name + targetDetectionRange);
        }
        else
        {
            targetDetectionRange = baseTargetDetectionRange;
            //Debug.Log("Normal rango" + enemy.name + targetDetectionRange);
        }

        targetDetectionRangeSQR = targetDetectionRange * targetDetectionRange;
    }
}
