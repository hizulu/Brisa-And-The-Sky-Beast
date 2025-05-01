using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 22/04/2025
[CreateAssetMenu(fileName = "Target-By Fuzzy Logic", menuName = "Enemy Logic/Detection Logic/Fuzzy Logic")]
public class EnemyTargetDetectionFuzzyLogic : EnemyTargetDetectionSOBase
{
    [SerializeField] private float targetDetectionRange = 12f;

    private float targetDetectionRangeSQR = 0f;

    private float playerHealthPercentage;
    private float beastHealthPercentage;

    private FuzzyEvaluator fuzzy = new FuzzyEvaluator();

    public override void Initialize(Enemy enemy)
    {
        base.Initialize(enemy);

        playerHealthPercentage = player.Data.StatsData.CurrentHealth/ player.Data.StatsData.MaxHealth * 100;
        beastHealthPercentage = beast.currentHealth/beast.maxHealth * 100;

        targetDetectionRangeSQR = targetDetectionRange * targetDetectionRange;
    }

    public override bool LookForTarget()
    {
        // Player is within detection range
        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;
        float distanceToBeastSQR = (enemy.transform.position - beastTransform.position).sqrMagnitude;
        if (distanceToPlayerSQR < targetDetectionRangeSQR || distanceToBeastSQR < targetDetectionRangeSQR)
        {
            enemy.targetIsPlayer = GetTargetByFuzzyLogic(distanceToPlayerSQR, distanceToBeastSQR);
            return true;
        }

        // No target detected within detection range
        Debug.Log("No target detected within detection range");
        return false;
    }

    private bool GetTargetByFuzzyLogic(float distanceToPlayerSQR, float distanceToBeastSQR)
    {
        float distPlayer = Mathf.Sqrt(distanceToPlayerSQR);
        float distBeast = Mathf.Sqrt(distanceToBeastSQR);

        float priorityPlayer = fuzzy.EvaluatePriority(distPlayer, playerHealthPercentage);
        float priorityBeast = fuzzy.EvaluatePriority(distBeast, beastHealthPercentage);

        return priorityPlayer >= priorityBeast;// True if target is player, false if target is Beast
    }
}
