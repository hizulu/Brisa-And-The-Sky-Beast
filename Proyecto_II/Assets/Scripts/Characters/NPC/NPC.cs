using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: NPC
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 15/05/2025
 * DESCRIPCIÓN: Clase que gestiona la lógica de los NPCs.
 * VERSIÓN: 1.0.
 */

public class NPC : MonoBehaviour
{
    #region Variables
    private NPCStateMachine npcStateMachine;

    public Animator AnimNPC {  get; private set; }
    #endregion

    private void Awake()
    {
        AnimNPC = GetComponent<Animator>();

        npcStateMachine = new NPCStateMachine(this);
    }

    void Start()
    {
        npcStateMachine.ChangeState(npcStateMachine.NPCIdleState);
    }

    void Update()
    {
        npcStateMachine.UpdateLogic();
    }

    private void FixedUpdate()
    {
        npcStateMachine.UpdatePhysics();
    }
}
