using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 20/04/2025
public class BeastBrisaHalfDeadState : BeastState
{
    public override void OnEnter(Beast beast)
    {
        Debug.Log("Has entrado en el estado de REVIVIR A BRISA");
        beast.agent.SetDestination(beast.playerTransform.position);
    }
    public override void OnUpdate(Beast beast)
    {
        // Si golpean a la bestia, sale del estado, pero como la bandera de "brisaIsHalfDead" sigue activa volverá a este estado (después de golpear al enemigo)

        if(Vector3.Distance(beast.agent.transform.position, beast.playerTransform.position) < 1.5f) // La Bestia empieza a curar a Brisa solo cuando llega a su lado.
            HealBrisa(beast);
    }
    public override void OnExit(Beast beast)
    {
        // Vuelve a estado de libertad
    }

    float maxTimeToRevive = 3f;
    float currentTime = 0f;
    public void HealBrisa(Beast beast)
    {
        PlayerStatsData playerStatsData = beast.player.Data.StatsData;

        Debug.Log("La Bestia está curando a Brisa");
        float healPerSecond = playerStatsData.MaxHealth / maxTimeToRevive;
        playerStatsData.CurrentHealth += healPerSecond * Time.deltaTime;

        if(playerStatsData.CurrentHealth > playerStatsData.MaxHealth)
        {
            playerStatsData.CurrentHealth = playerStatsData.MaxHealth;
            beast.TransitionToState(new BeastFreeState()); // TODO: Provisional, de momento he puesto esto para que deje de curar a Brisa.
        }

        currentTime += Time.deltaTime;
    }
}