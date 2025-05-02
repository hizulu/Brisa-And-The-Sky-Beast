using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Jone Sainz Egea
// 15/04/2025
public class Beast : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Animator anim;
    //[SerializeField] public Rigidbody rb;
    [SerializeField] public Transform playerTransform;
    [SerializeField] public Transform mountPoint;

    [Header("Parameters")]
    [SerializeField] public float arrivalThreshold = 5f;
    [SerializeField] public float freeRoamRadius = 30f;
    [SerializeField] public float interactionThreshold = 8f;

    [Header("Stats")]
    [SerializeField] public float maxHealth = 500f;
    [SerializeField] public float healingAmount = 50f; // Estoy gestionando la cura de la Bestia desde el script de Brisa.
    [SerializeField] public float halfDeadDuration = 30f;
    [SerializeField] public float swipeAttackDamage = 15f;
    [SerializeField] public float biteAttackDamage = 25f;

    public float currentHealth;

    float baseInterestInBrisa = 1f;
    float growthFactorInterestInBrisa = 0.05f;

    private BeastState currentState;

    public Blackboard blackboard { get; private set; }

    private Coroutine activeCoroutine; // Para gestionar que solo haya una corrutina en marcha a la vez
    private ICoroutineNode coroutineOwner;

    [SerializeField] public Player player;

    public HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();
    private bool isInCombat = false;
    private SphereCollider detectionCollider;

    private void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (anim == null) anim = GetComponent<Animator>();
        if (blackboard == null) blackboard = new Blackboard();

        currentHealth = maxHealth;

        AnimationDurationDatabase.Instance.RegisterAnimatorClips(anim.runtimeAnimatorController);

        detectionCollider = GetComponentInChildren<SphereCollider>();

        if (detectionCollider == null)
        {
            Debug.LogError("DetectionTrigger (SphereCollider) not found in beast children!");
        }

        // Comenzamos en estado de libertad
        TransitionToState(new BeastFreeState());

        #region Events
        EventsManager.CallSpecialEvents<float>("OnAttackBeast", DamageBeast);
        EventsManager.CallSpecialEvents<Vector3>("BeastDirected", BeastIsDirected);

        EventsManager.CallNormalEvents("CallBeast", CallBeast);
        EventsManager.CallNormalEvents("AcariciarBestia_Bestia", PetBeastSelected);
        EventsManager.CallNormalEvents("SanarBestia_Bestia", HealBeastSelected);
        EventsManager.CallNormalEvents("AtaqueBestia_Bestia", AttackBeastSelected);
        EventsManager.CallNormalEvents("MontarBestia_Bestia", MountBeastSelected);
        EventsManager.CallNormalEvents("AccionBestia_Bestia", ActionBeastSelected);

        EventsManager.CallNormalEvents("BrisaHalfDead", BrisaIsHalfDead);
    }

    private void OnDestroy()
    {
        EventsManager.StopCallSpecialEvents<float>("OnAttackBeast", DamageBeast);
        EventsManager.StopCallSpecialEvents<Vector3>("BeastDirected", BeastIsDirected);

        EventsManager.StopCallNormalEvents("CallBeast", CallBeast);
        EventsManager.StopCallNormalEvents("AcariciarBestia_Bestia", PetBeastSelected);
        EventsManager.StopCallNormalEvents("SanarBestia_Bestia", HealBeastSelected);
        EventsManager.StopCallNormalEvents("AtaqueBestia_Bestia", AttackBeastSelected);
        EventsManager.StopCallNormalEvents("MontarBestia_Bestia", MountBeastSelected);
        EventsManager.StopCallNormalEvents("AccionBestia_Bestia", ActionBeastSelected);

        EventsManager.StopCallNormalEvents("BrisaHalfDead", BrisaIsHalfDead);
        #endregion
    }
    private void Update()
    {
        currentState?.OnUpdate(this);
    }

    public void TransitionToState(BeastState newState)
    {
        currentState?.OnExit(this);

        // Para asegurar que se realizan las acciones de fin de corrutina al cambiar de estado:
        if (activeCoroutine != null)
            coroutineOwner?.OnCoroutineEnd();

        currentState = newState;

        currentState?.OnEnter(this);
    }

    // Gestión de corrutinas de la Bestia
    public void StartNewCoroutine(IEnumerator routine, ICoroutineNode owner)
    {
        if (activeCoroutine != null)
            coroutineOwner?.OnCoroutineEnd();

        coroutineOwner = owner;
        activeCoroutine = StartCoroutine(routine);
        blackboard.SetValue("isCoroutineActive", true);
    }

    #region Enemy detection logic
    public void OnEnemyEnter(GameObject enemy)
    {
        enemiesInRange.Add(enemy);
        Debug.Log($"Enemy {enemy.name} entered detection range.");

        CheckCombatState();
    }

    public void OnEnemyExit(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
        Debug.Log($"Enemy {enemy.name} exited detection range.");

        CheckCombatState();
    }

    private void CheckCombatState()
    {
        if (enemiesInRange.Count > 0 && !isInCombat)
        {
            EnterCombatState();
        }
        else if (enemiesInRange.Count == 0 && isInCombat)
        {
            ExitCombatState();
        }
    }

    private void EnterCombatState()
    {
        isInCombat = true;

        if (currentState is not BeastCombatState)
            TransitionToState(new BeastCombatState());
    }

    private void ExitCombatState()
    {
        isInCombat = false;
        ChangeEnemyDetectionRange(); // Reseteo del rango de combate

        TransitionToState(new BeastFreeState());
    }

    private void ChangeEnemyDetectionRange(float expectedRange = 20f)
    {
        detectionCollider.radius = expectedRange;
    }
    #endregion

    #region Event triggered functions
    private void CallBeast()
    {
        blackboard.SetValue("isConstrained", true);
        agent.ResetPath();

        TransitionToState(new BeastConstrainedState());

        Debug.Log("Bestia llamada por el jugador");
    }

    private void BrisaIsHalfDead()
    {
        blackboard.SetValue("brisaIsHalfDead", true);
        TransitionToState(new BeastBrisaHalfDeadState());
    }

    private void BeastIsDirected(Vector3 targetDestination)
    {
        agent.ResetPath();
        TransitionToState(new BeastToPointedState(targetDestination));
    }
    #endregion

    #region Beast Selection Menu
    public void OpenBeastMenu()
    {
        // Por si se abre el menú sin estar en estado de constrained
        agent.ResetPath();
        blackboard.SetValue("menuOpenedFromOtherState", true);
        blackboard.SetValue("isConstrained", true);       
    }

    public void ResetBeastSelection()
    {
        // Resetear todos los valores
        blackboard.SetValue("isOptionPet", false);
        blackboard.SetValue("isOptionHeal", false);
        blackboard.SetValue("isOptionAttack", false);
        blackboard.SetValue("isOptionMount", false);
        blackboard.SetValue("isOptionAction", false);
    }

    public void PetBeastSelected()
    {
        ResetBeastSelection();
        blackboard.SetValue("isOptionPet", true);
    }

    public void HealBeastSelected()
    {
        ResetBeastSelection();
        blackboard.SetValue("isOptionHeal", true);

        if (currentHealth == maxHealth) return;
    }

    public void AttackBeastSelected()
    {
        ResetBeastSelection();
        blackboard.SetValue("isOptionAttack", true);
        ChangeEnemyDetectionRange(30f); // Si el jugador le manda atacar, aumenta su rango de detección de enemigos
    }

    public void MountBeastSelected()
    {
        ResetBeastSelection();
        blackboard.SetValue("isOptionMount", true);
    }

    public void ActionBeastSelected()
    {
        ResetBeastSelection();
        blackboard.SetValue("isOptionAction", true);
    }
    #endregion

    #region Damage Related Functions
    public void DamageBeast(float damage)
    {
        if (currentState is not BeastCombatState)
            TransitionToState(new BeastCombatState());

        anim.SetTrigger("damageBeast");

        // TODO: beast gets damaged sound

        currentHealth -= damage;
        Debug.Log("Beast has been damaged");
        if (currentHealth < Mathf.Epsilon)
        {
            blackboard.SetValue("isHalfDead", true);
            TransitionToState(new BeastHalfDeadState());
        }
    }
    #endregion

    public bool IsPlayerWithinInteractionDistance()
    {
        return Vector3.Distance(transform.position, playerTransform.position) < interactionThreshold;
    }

    // TODO: borrar esto
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, freeRoamRadius);
        if (playerTransform == null) return;
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        float printInterestInBrisa = baseInterestInBrisa * Mathf.Exp(growthFactorInterestInBrisa * distance);
        // UnityEditor.Handles.Label(playerTransform.position + Vector3.up * 4, $"Interest: {printInterestInBrisa}");
    }
}
