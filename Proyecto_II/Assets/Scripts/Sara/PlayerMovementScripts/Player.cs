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

    [SerializeField] public GameObject PaloBrisa;
    [SerializeField] public GameObject hitBox;
    [SerializeField] private float attackDuration = 0.2f;
    public Rigidbody RbPlayer { get; private set; }
    public BoxCollider GroundCheckCollider { get; private set; }
    public Animator AnimPlayer { get; private set; }
    public CinemachineVirtualCamera CamPlayer { get; private set; }

    private PlayerStateMachine playerStateMachine;

    public PlayerInput PlayerInput { get; private set; }



    private void Awake()
    {
        Debug.Log("Awake iniciado en " + gameObject.name);

        // Verificar PlayerAnimationData antes de inicializar
        if (PlayerAnimationData == null)
        {
            Debug.LogError("PlayerAnimationData es null en Awake()");
        }
        else
        {
            PlayerAnimationData.Initialize();
        }

        // Obtener Rigidbody y verificar
        RbPlayer = GetComponent<Rigidbody>();
        if (RbPlayer == null)
        {
            Debug.LogError("No se encontró Rigidbody en " + gameObject.name);
        }

        // Obtener BoxCollider (GroundCheckCollider) y verificar
        GroundCheckCollider = GetComponentInChildren<BoxCollider>();
        if (GroundCheckCollider == null)
        {
            Debug.LogError("No se encontró BoxCollider en los hijos de " + gameObject.name);
        }

        // Obtener Animator y verificar
        AnimPlayer = GetComponent<Animator>();
        if (AnimPlayer == null)
        {
            Debug.LogError("No se encontró Animator en " + gameObject.name);
        }

        // Obtener PlayerInput y verificar
        PlayerInput = GetComponent<PlayerInput>();
        if (PlayerInput == null)
        {
            Debug.LogError("No se encontró PlayerInput en " + gameObject.name);
        }

        // Crear PlayerStateMachine verificando que 'this' es válido
        if (this != null)
        {
            Debug.Log("Creando PlayerStateMachine para " + gameObject.name);
            playerStateMachine = new PlayerStateMachine(this);
        }
        else
        {
            Debug.LogError("El objeto Player es null en Awake()");
        }

        // Verificar PlayerActions antes de suscribirse a Inventory.performed
        if (PlayerInput != null) // && PlayerInput.PlayerActions != null
        {
            PlayerInput.PlayerActions.Inventory.performed += OpenCloseInventory;
        }
        else
        {
            Debug.LogError("PlayerActions es null en Awake()");
        }
    }

    private void OnDestroy()
    {
        PlayerInput.PlayerActions.Inventory.performed -= OpenCloseInventory;
    }

    void Start()
    {
        playerStateMachine.ChangeState(playerStateMachine.IdleState);
        PaloBrisa.SetActive(false);
        hitBox.SetActive(false);
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

    #region Métodos temporales (No se sabe si se quedarán en el script de Player o se moverán).

    public void GolpearPrueba()
    {
        StartCoroutine(EnableHitBox());
    }

    private IEnumerator EnableHitBox()
    {
      hitBox.SetActive(true); // Activa el hitbox
      yield return new WaitForSeconds(attackDuration);
      hitBox.SetActive(false); // Lo desactiva después de un tiempo
    }

    public void PaloRecogido()
    {
        PaloBrisa.SetActive(true);
    }
    #endregion
}
