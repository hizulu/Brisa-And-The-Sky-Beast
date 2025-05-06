using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 20/04/2025
public class BeastBrisaHalfDeadState : BeastState
{
    private Coroutine reviveCoroutine;
    private bool isRevivingBrisa = false;

    public override void OnEnter(Beast beast)
    {
        Debug.Log("Entra en el estado de revivir a Brisa");
        beast.agent.SetDestination(beast.playerTransform.position);
        beast.anim.SetBool("isWalking", true);
        isRevivingBrisa = false;
    }
    public override void OnUpdate(Beast beast)
    {
        if (!isRevivingBrisa && Vector3.Distance(beast.agent.transform.position, beast.playerTransform.position) < 2.5f)
        {
            reviveCoroutine = beast.StartCoroutine(ReviveBrisa(beast));
        }
    }
    public override void OnExit(Beast beast)
    {
        beast.agent.ResetPath();
        if (isRevivingBrisa)
        {
            beast.StopCoroutine(reviveCoroutine);
            reviveCoroutine = null;
            isRevivingBrisa = false;
            if (beast.player.Data.StatsData.CurrentHealth != beast.player.Data.StatsData.MaxHealth)
                beast.player.Data.StatsData.CurrentHealth = 0f; // No ha terminado de revivirla
            Debug.Log("Revivir cancelado al salir del estado.");
        }
        Debug.Log("Sale del estado de revivir a Brisa");
    }

    private IEnumerator ReviveBrisa(Beast beast)
    {
        isRevivingBrisa = true;
        beast.anim.SetBool("isWalking", false);
        beast.anim.SetTrigger("reviveBrisa");

        PlayerStatsData stats = beast.player.Data.StatsData;

        float duration = 3f;
        float elapsed = 0f;
        float startHealth = stats.CurrentHealth;
        float endHealth = stats.MaxHealth;

        Debug.Log("La Bestia empieza a revivir a Brisa");

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            stats.CurrentHealth = Mathf.Lerp(startHealth, endHealth, elapsed / duration);
            yield return null;
        }

        stats.CurrentHealth = endHealth;
        Debug.Log("Brisa ha sido revivida.");
        isRevivingBrisa = false;
        beast.TransitionToState(new BeastFreeState());
    }
}