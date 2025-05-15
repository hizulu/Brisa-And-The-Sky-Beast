using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: NPCStateMachine
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 15/05/2025
 * DESCRIPCI�N: Clase que hereda de StateMachine y se encarga de instanciar y dar acceso a los estados de los NPCs.
 *              Mantiene las referencias a los diferentes estados.
 * VERSI�N: 1.0. Instanciaci�n de todos los estados de los NPCs.
 */

public class NPCStateMachine : StateMachine
{
    public NPC NPC { get; }
    public NPCIdleState NPCIdleState { get; }
    public NPCTalkState NPCTalkState { get; }

    public NPCStateMachine(NPC _NPC)
    {
        NPC = _NPC;

        NPCIdleState = new NPCIdleState(this);
        NPCTalkState = new NPCTalkState(this);
    }
}
