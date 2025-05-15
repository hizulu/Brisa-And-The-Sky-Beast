using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: NPCTalkState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 15/05/2025
 * DESCRIPCI�N: Clase que hereda de NPCStateTemplate y define la l�gica del estado de hablar de los NPCs.
 * VERSI�N: 1.0.
 */

public class NPCTalkState : NPCStateTemplate
{
    public NPCTalkState(NPCStateMachine _npcStateMachine) : base(_npcStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("El NPC ha entrado en estado de HABLAR");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("El NPC ha salido del estado de HABLAR");
    }
}
