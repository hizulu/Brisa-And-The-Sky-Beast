using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: NPCStateTemplate
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 15/05/2025
 * DESCRIPCI�N: Clase abstracta que sirve como plantilla base para la m�quina de estados de los NPCs.
 *              Hereda de la interfaz IState, implementando sus m�todos.
 *              Al crealo recibe la referencia de la m�quina de estados.
 *              Se guarda la referencia a la m�quina de estados y se deja accesible para las clases que hereden de esta.
 * VERSI�N: 1.0. Script base para utilizar sus m�todos en los diferentes estados de los NPCs.
 */

public class NPCStateTemplate : IState
{
    protected NPCStateMachine npcStateMachine;

    public NPCStateTemplate(NPCStateMachine _npcStateMachine)
    {
        npcStateMachine = _npcStateMachine;
    }

    public virtual void Enter()
    {
        EventsManager.CallNormalEvents("NPCStartTalk", ChangeToTalkState);
        EventsManager.CallNormalEvents("NPCIdle", ChangeToIdleState);
    }

    public virtual void Exit()
    {
        EventsManager.StopCallNormalEvents("NPCStartTalk", ChangeToTalkState);
        EventsManager.StopCallNormalEvents("NPCIdle", ChangeToIdleState);
    }

    public virtual void HandleInput()
    {

    }

    public virtual void UpdateLogic()
    {

    }

    public virtual void UpdatePhysics()
    {

    }

    public virtual void OnTriggerEnter(Collider collider)
    {

    }

    public virtual void OnTriggerExit(Collider collider)
    {

    }

    #region M�todos Propios Generales
    private void ChangeToTalkState()
    {
        npcStateMachine.ChangeState(npcStateMachine.NPCTalkState);
    }

    private void ChangeToIdleState()
    {
        npcStateMachine.ChangeState(npcStateMachine.NPCIdleState);
    }
    #endregion
}
