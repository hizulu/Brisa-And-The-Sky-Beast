using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 20/04/2025
public class BeastBrisaHalfDeadState : BeastState
{
    public override void OnEnter(Beast beast)
    {
        beast.agent.SetDestination(beast.playerTransform.position);
    }
    public override void OnUpdate(Beast beast)
    {
        // Si golpean a la bestia, sale del estado, pero como la bandera de "brisaIsHalfDead" sigue activa volverá a este estado (después de golpear al enemigo)
    }
    public override void OnExit(Beast beast)
    {
        // Vuelve a estado de libertad
    }
}
