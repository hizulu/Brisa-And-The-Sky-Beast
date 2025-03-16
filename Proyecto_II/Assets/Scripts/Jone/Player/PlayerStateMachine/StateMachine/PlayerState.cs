//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerState
//{
//    // Clase que define cómo serán los estados del jugador

//    #region Movement Variables
//    Rigidbody rb;
//    public Animator anim { get; set; } // Public? --> DUDA

//    protected float baseSpeed = 5f;
//    protected float speedMultiplier = 1f;
//    // protected float rotationSpeed = 15f;

//    protected private Transform camTransform;
//    #endregion

//    protected Player player;
//    protected PlayerStateMachine playerStateMachine;
//    public PlayerState(Player player, PlayerStateMachine playerStateMachine)
//    {
//        this.player = player;
//        this.playerStateMachine = playerStateMachine;
//    }

//    public PlayerState(Player player, PlayerStateMachine playerStateMachine, float baseSpeed, float speedMultiplier)
//    {
//        this.player = player;
//        this.playerStateMachine = playerStateMachine;
//        this.baseSpeed = baseSpeed;
//        this.speedMultiplier = speedMultiplier;
//    }


//    public virtual void EnterState() { }
//    public virtual void ExitState() { }
//    public virtual void FrameUpdate() { }
//    public virtual void FixedUpdate() { }

//}
