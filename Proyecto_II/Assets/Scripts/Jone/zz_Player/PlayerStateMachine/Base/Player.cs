//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class Player : MonoBehaviour
//{
//    #region Movement Variables
//    Rigidbody rb;
//    public Animator anim { get; set; } // Public? --> DUDA

//    [Header("Movement Settings")]
//    [SerializeField] float baseSpeed = 5f;
//    [SerializeField] float speedMultiplier = 1f;
//    [SerializeField] public static float rotationSpeed = 15f; // Public static¿? --> DUDA
//    [SerializeField]  float runMultiplier = 1.25f;
//    [SerializeField]  float crouchMultiplier = 0.75f;

//    [SerializeField] public Transform camTransform; // Public --> DUDA
//    #endregion

//    #region New Input System Variables
//    PlayerInput playerInput;
//    [Space(10)]
//    [Header("Inputs")]
//    [SerializeField] public InputActionReference walkAction; // DUDA (public)
//    [SerializeField] public InputActionReference runAction; // DUDA
//    [SerializeField] public InputActionReference crouchedAction; // DUDA
//    #endregion

//    #region State Machine Variables
//    public PlayerStateMachine StateMachine { get; set; }
//    public PlayerIdleState IdleState { get; set; }
//    //public PlayerWalkState WalkState { get; set; }
//    //public PlayerRunState RunState { get; set; }
//    //public PlayerCrouchState CrouchState { get; set; }
//    #endregion

//    private void Awake()
//    {
//        StateMachine = new PlayerStateMachine();

//        IdleState = new PlayerIdleState(this, StateMachine);
//        WalkState = new PlayerWalkState(this, StateMachine, baseSpeed, speedMultiplier);
//        RunState = new PlayerRunState(this, StateMachine, baseSpeed, runMultiplier);
//        CrouchState = new PlayerCrouchState(this, StateMachine, baseSpeed, crouchMultiplier);
//    }
//    // Start is called before the first frame update
//    void Start()
//    {
//        playerInput = GetComponent<PlayerInput>();
//        rb = GetComponent<Rigidbody>();
//        anim = GetComponent<Animator>();

//        StateMachine.Initialize(IdleState);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        StateMachine.CurrentPlayerState.FrameUpdate();
//    }
//}
