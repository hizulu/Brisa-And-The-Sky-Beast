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

    [field: Header("Collisions")]
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

    [field: Header("Animaciones")]
    [field: SerializeField] public PlayerAnimationData PlayerAnimationData { get; private set; }

    public Rigidbody RbPlayer { get; private set; }
    public Animator AnimPlayer { get; private set; }
    public CinemachineVirtualCamera CamPlayer { get; private set; }

    private PlayerStateMachine playerStateMachine;

    public PlayerInput PlayerInput { get; private set; }



    private void Awake()
    {
        PlayerAnimationData.Initialize();

        RbPlayer = GetComponent<Rigidbody>();
        AnimPlayer = GetComponent<Animator>();

        PlayerInput = GetComponent<PlayerInput>();

        playerStateMachine = new PlayerStateMachine(this);

        PlayerInput.PlayerActions.Inventory.performed += OpenCloseInventory;
    }

    private void OnDestroy()
    {
        PlayerInput.PlayerActions.Inventory.performed -= OpenCloseInventory;
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

    private void OnTriggerEnter(Collider collider)
    {
        playerStateMachine.OnTriggerEnter(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        playerStateMachine.OnTriggerExit(collider);
    }

    public void OpenCloseInventory(InputAction.CallbackContext context)
    {
        InventoryManager.Instance.OpenCloseInventory(context);
    }

}
