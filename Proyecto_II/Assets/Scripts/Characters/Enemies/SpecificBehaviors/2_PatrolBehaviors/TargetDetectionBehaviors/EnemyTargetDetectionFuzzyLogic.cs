using UnityEngine;

/*
 * NOMBRE CLASE: EnemyTargetDetectionFuzzyLogic
 * AUTOR: Jone Sainz Egea
 * FECHA: 22/04/2025
 * DESCRIPCIÓN: Clase que define la detección específica de objetivo del enemigo mediante lógica difusa.
 *              Tiene en cuenta la salud y la distancia a la que se encuentran el jugador y la Bestia.
 *              Prioriza al objetivo que se encuentre cerca y el que tenga menos salud.
 *              Hereda de EnemyTargetDetectionSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.  
 * VERSIÓN: 1.0. Script base con la detección de objetivos mediante lógica difusa.
 *          1.1. Sara Yue Madruga Martín - Disminución del tamaño de detección de Player cuando este está en modo sigilo. (17/05/2025).
 */
[CreateAssetMenu(fileName = "Target-By Fuzzy Logic", menuName = "Enemy Logic/Detection Logic/Fuzzy Logic")]
public class EnemyTargetDetectionFuzzyLogic : EnemyTargetDetectionSOBase
{
    [SerializeField] private float baseTargetDetectionRange = 12f;
    [Range(0.1f, 1f)] [SerializeField] private float targetDetectionModifier = 0.5f;
    private float targetDetectionRange;
    //private float originalTargetDetectionRange;

    private float targetDetectionRangeSQR = 0f;

    private float playerHealthPercentage;
    private float beastHealthPercentage;

    private FuzzyEvaluator fuzzy = new FuzzyEvaluator();

    public override void Initialize(Enemy enemy)
    {
        targetDetectionRange = baseTargetDetectionRange;
        

        base.Initialize(enemy);

        playerHealthPercentage = player.Data.StatsData.CurrentHealth/ player.Data.StatsData.MaxHealth * 100;
        beastHealthPercentage = beast.currentHealth/beast.maxHealth * 100;

        //targetDetectionRangeSQR = targetDetectionRange * targetDetectionRange;

        ChangeRangeDetection();
        //Debug.Log("Normal rango" + enemy.name + targetDetectionRange);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    /*
     * Método que busca un objetivo para el enemigo
     * Establece el booleano de enemy que señala cuál es el enemigo
     * @return bool true si ha encontrado un objetivo válido, false si no
     */
    public override bool LookForTarget()
    {
        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;
        float distanceToBeastSQR = (enemy.transform.position - beastTransform.position).sqrMagnitude;
        if (distanceToPlayerSQR < targetDetectionRangeSQR || distanceToBeastSQR < targetDetectionRangeSQR)
        {
            enemy.targetIsPlayer = GetTargetByFuzzyLogic(distanceToPlayerSQR, distanceToBeastSQR);
            if (enemy.targetIsPlayer && playerHealthPercentage <= 0f)
                return false; // Player is dead
            if (!enemy.targetIsPlayer && beastHealthPercentage <= 0f)
                return false; // Beast is dead
            return true;
        }

        // No target detected within detection range
        return false;
    }

    /*
     * Método que busca el objetivo mediante lógica difusa
     * Compara el valor difuso obtenido desde la clase FuzzyEvaluator
     * @return bool true si el objetivo es player, false si es bestia
     */
    private bool GetTargetByFuzzyLogic(float distanceToPlayerSQR, float distanceToBeastSQR)
    {
        float distPlayer = Mathf.Sqrt(distanceToPlayerSQR);
        float distBeast = Mathf.Sqrt(distanceToBeastSQR);

        float priorityPlayer = fuzzy.EvaluatePriority(distPlayer, playerHealthPercentage);
        float priorityBeast = fuzzy.EvaluatePriority(distBeast, beastHealthPercentage);

        return priorityPlayer >= priorityBeast;// True if target is player, false if target is Beast
    }

    

    protected void ChangeRangeDetection()
    {
        if (isCrouching)
        {
            targetDetectionRange = baseTargetDetectionRange * targetDetectionModifier;
            //Debug.Log("Reducción de rango" + enemy.name + targetDetectionRange);
        }
        else
        {
            targetDetectionRange = baseTargetDetectionRange;
            //Debug.Log("Normal rango" + enemy.name + targetDetectionRange);
        }

        targetDetectionRangeSQR = targetDetectionRange * targetDetectionRange;
    }
}
