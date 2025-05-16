using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: NPCIdleState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 15/05/2025
 * DESCRIPCI�N: Clase que hereda de NPCStateTemplate y define la l�gica del estado de Idle de los NPCs.
 * VERSI�N: 1.0.
 */

public class NPCIdleState : NPCStateTemplate
{
    public NPCIdleState(NPCStateMachine _npcStateMachine) : base(_npcStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        npcStateMachine.NPC.AnimNPC.SetBool("isIdle", true);
        Debug.Log("El NPC ha entrado en estado de IDLE");
    }

    public override void Exit()
    {
        base.Exit();
        npcStateMachine.NPC.AnimNPC.SetBool("isIdle", false);
        Debug.Log("El NPC ha salido del estado de IDLE");
    }
}
