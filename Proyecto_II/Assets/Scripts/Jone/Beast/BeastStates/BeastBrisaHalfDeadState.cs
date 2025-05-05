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
    }
    public override void OnUpdate(Beast beast)
    {
        if (!isRevivingBrisa && Vector3.Distance(beast.agent.transform.position, beast.playerTransform.position) < 1.5f)
        {
            //reviveCoroutine = beast.StartCoroutine(ReviveBrisa(beast));
        }
    }
    public override void OnExit(Beast beast)
    {
        beast.agent.ResetPath();
        Debug.Log("Salde el estado de revivir a Brisa");
    }

    float maxTimeToRevive = 3f;
    public void ReviveBrisa(Beast beast)
    {
        PlayerStatsData playerStatsData = beast.player.Data.StatsData;

        beast.anim.SetBool("isWalking", false);
        beast.anim.SetTrigger("reviveBrisa");

        Debug.Log("La Bestia está reviviendo a Brisa");
        float healPerSecond = playerStatsData.MaxHealth / maxTimeToRevive;
        playerStatsData.CurrentHealth += healPerSecond * Time.deltaTime;

        if(playerStatsData.CurrentHealth > playerStatsData.MaxHealth)
        {
            playerStatsData.CurrentHealth = playerStatsData.MaxHealth;
            Debug.Log("Brisa ha sido revivida.");
            beast.TransitionToState(new BeastFreeState()); // TODO: Provisional, de momento he puesto esto para que deje de curar a Brisa.
        }
    }
}