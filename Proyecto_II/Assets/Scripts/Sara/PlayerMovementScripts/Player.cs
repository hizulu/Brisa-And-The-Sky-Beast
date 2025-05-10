using Cinemachine;
using System.Collections;
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

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData PlayerAnimationData { get; private set; }

    [field: Header("Objetcts")]
    [SerializeField] public Beast Beast;
    [SerializeField] public CinemachineVirtualCamera playerCam;
    public CinemachinePOV CamComponents;
    [SerializeField] public Transform lookCamPlayer;
    [SerializeField] public GameObject PaloBrisa;
    [SerializeField] public GameObject hitBox;
    [SerializeField] public GameObject Shield;
    [SerializeField] private BeastSelectionPanel beastPanel;
    [SerializeField] private MapManager mapManager;
    [SerializeField] public GameObject AreaMoveBeast;
    [SerializeField] public GameObject CursorMarker;
    [SerializeField] public PointTarget pointTarget;
    [SerializeField] public GameObject Mango;
    [SerializeField] public GameObject Baya;
    [SerializeField] public BeastTrapped beastTrapped;
    [SerializeField] public SkinnedMeshRenderer RenderPlayer;

    public BoxCollider GroundCheckCollider { get; private set; }
    public PlayerInput PlayerInput { get; private set; }

    [field: Header("Components")]
    public Rigidbody RbPlayer { get; private set; }
    public Animator AnimPlayer { get; private set; }

    private float attackDuration = 0.2f;

    private PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        PlayerAnimationData.Initialize();

        RbPlayer = GetComponent<Rigidbody>();

        GroundCheckCollider = GetComponentInChildren<BoxCollider>();

        AnimPlayer = GetComponent<Animator>();

        PlayerInput = GetComponent<PlayerInput>();

        CamComponents = playerCam.GetCinemachineComponent<CinemachinePOV>();

        playerStateMachine = new PlayerStateMachine(this);

        PlayerInput.UIPanelActions.Inventory.performed += OpenCloseInventory;
        PlayerInput.UIPanelActions.ClosePanelGeneral.performed += OpenCloseInventory;
        PlayerInput.UIPanelActions.BeastPanel.performed += OpenCloseBeastPanel;
        PlayerInput.UIPanelActions.Map.performed += OpenCloseMapPanel;
    }

    void OnDisable()
    {
        if (!enabled)
            enabled = true;
    }

    private void OnDestroy()
    {
        PlayerInput.UIPanelActions.Inventory.performed -= OpenCloseInventory;
        PlayerInput.UIPanelActions.ClosePanelGeneral.performed -= OpenCloseInventory;
        PlayerInput.UIPanelActions.BeastPanel.performed -= OpenCloseBeastPanel;
        PlayerInput.UIPanelActions.Map.performed -= OpenCloseMapPanel;
    }

    void Start()
    {
        playerStateMachine.ChangeState(playerStateMachine.IdleState);
        //PaloBrisa.SetActive(false);
        hitBox.SetActive(false);
        ResetHealth();
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

    public void OpenCloseBeastPanel(InputAction.CallbackContext context)
    {
        beastPanel.OpenCloseBeastPanel(context);
    }

    public void OpenCloseMapPanel(InputAction.CallbackContext context)
    {
        mapManager.OpenCloseMapPanel(context);
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
        Debug.Log("Palo recogido");
        PaloBrisa.SetActive(true);
    }

    public void ResetHealth()
    {
        Data.StatsData.CurrentHealth = Data.StatsData.MaxHealth;
        //Debug.Log("Se ha reseteado la vida" + Data.StatsData.CurrentHealth);
    }
    #endregion

    [SerializeField] private float playerTargetEnemiesRange = 20f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerTargetEnemiesRange);
    }
}
