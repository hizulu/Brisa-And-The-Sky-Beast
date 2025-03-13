using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE SCRIPT: Player
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Script que gestiona toda la lógica de la máquina de estado con el modelo 3D del juego.
 * VERSIÓN: 1.0. 
 */
public class Player : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Animaciones")]
    [field: SerializeField] public PlayerAnimationData PlayerAnimationData { get; private set; }

    public Rigidbody rbPlayer { get; private set; }
    public Animator animPlayer { get; private set; }
    public CinemachineVirtualCamera camPlayer { get; private set; }

    private PlayerStateMachine playerStateMachine;

    public PlayerInput playerInput { get; private set; }

    private void Awake()
    {
        PlayerAnimationData.Initialize();

        rbPlayer = GetComponent<Rigidbody>();
        animPlayer = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();

        playerStateMachine = new PlayerStateMachine(this);
    }

    void Start()
    {
        playerStateMachine.ChangeState(playerStateMachine.IdleState);
    }
    private void FixedUpdate()
    {
        playerStateMachine.UpdatePhysics();
    }

    private void Update()
    {
        playerStateMachine.HandleInput();
        playerStateMachine.UpdateLogic();
    }

}
