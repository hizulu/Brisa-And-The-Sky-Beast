using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * NOMBRE CLASE: Enemy
 * AUTOR: Sara Yue Madruga Martín, Jone Sainz Egea
 * FECHA: 
 * DESCRIPCIÓN: Script que gestiona toda la lógica del enemigo, así como sus estadísticas.
 *              Instancia e inicializa los comportamientos de cada estado.
 *              Funcionamiento modular de los diferentes estados.
 *              Crea una EnemyStateMachine y efecuta sus funciones.
 * VERSIÓN: 1.0. Script base para la gestión de la FSM con comportamientos en SO
 *              1.1. Se añade lógica para dañar al enemigo
 */
public class Enemy : HittableElement
{
    #region Main Enemy Variables
    public Player player;
    public Animator anim { get; private set; }
    public NavMeshAgent agent;


    [SerializeField] public float maxHealth = 100f;
    [field:SerializeField] public float currentHealth;
    [SerializeField] float enemySpeed = 1f; // TODO: speed affects movement speed
    [SerializeField] float attackDamage = 10f; // TODO: attackDamage is taken into account
    #endregion

    #region FSM Variables
    public EnemyStateMachine enemyStateMachine {  get; private set; }

    [SerializeField] private EnemyStateSOBase EnemyIdleBase;
    [SerializeField] private EnemyStateSOBase EnemyPatrolBase;
    [SerializeField] private EnemyStateSOBase EnemyChaseBase;
    [SerializeField] private EnemyStateSOBase EnemyAttackBase;
    [SerializeField] private EnemyStateSOBase EnemyRetreatBase;

    public EnemyStateSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyStateSOBase EnemyPatrolBaseInstance { get; set; }
    public EnemyStateSOBase EnemyChaseBaseInstance { get; set; }
    public EnemyStateSOBase EnemyAttackBaseInstance { get; set; }
    public EnemyStateSOBase EnemyRetreatBaseInstance { get; set; }
    #endregion

    #region Variables temporales para visualizar las áreas: Gizmos
    [Header("Variables Gizmos")]
    [SerializeField] private float playerAttackRange = 1f;
    [SerializeField] private float playerLostRange = 15f;
    [SerializeField] private float playerDetectionRange = 12f;
    #endregion

    #region Suscripciones y desuspripciones a eventos
    #endregion

    private void Awake()
    {
        // Instanciar el comportamiento específico asociado al enemigo
        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        EnemyPatrolBaseInstance = Instantiate(EnemyPatrolBase);
        EnemyChaseBaseInstance = Instantiate(EnemyChaseBase);
        EnemyAttackBaseInstance = Instantiate(EnemyAttackBase);
        EnemyRetreatBaseInstance = Instantiate(EnemyRetreatBase);

        enemyStateMachine = new EnemyStateMachine(this);

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        EventsManager.CallSpecialEvents<float>("OnAttack01Enemy", SetDamageEnemy);
        EventsManager.CallSpecialEvents<float>("OnAttack02Enemy", SetDamageEnemy);
        EventsManager.CallSpecialEvents<float>("OnAttack03Enemy", SetDamageEnemy);
    }

    private void Start()
    {
        // Inicializar los comportamientos específicos asociados al enemigo
        EnemyIdleBaseInstance.Initialize(this);
        EnemyPatrolBaseInstance.Initialize(this);
        EnemyChaseBaseInstance.Initialize(this);
        EnemyAttackBaseInstance.Initialize(this);
        EnemyRetreatBaseInstance.Initialize(this);

        enemyStateMachine.ChangeState(enemyStateMachine.EnemyIdleState);

        currentHealth = maxHealth;
    }

    private void OnDestroy()
    {
        EventsManager.StopCallSpecialEvents<float>("OnAttack01Enemy", SetDamageEnemy);
        EventsManager.StopCallSpecialEvents<float>("OnAttack02Enemy", SetDamageEnemy);
        EventsManager.StopCallSpecialEvents<float>("OnAttack03Enemy", SetDamageEnemy);
    }

    private void Update()
    {
        enemyStateMachine.UpdateLogic();
    }

    private void FixedUpdate()
    {
        enemyStateMachine.UpdatePhysics();
    }

    /*
     * Método que se llama desde los estados del enemigo y se encarga de moverlo al destino indicado.
     * @param1 destination - Recibe la posición objetivo a la que debe ir.
     */
    public void MoveEnemy(Vector3 destination)
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(destination);
        }
    }

    #region DamageRelated Functions
    public static float damageAmount = 0f; // Debe ser estática para que todos los enemigos puedan acceder al cambio de parámetro de daño del evento.
    public override void OnHit()
    {
        ApplyDamageToEnemy();
    }

    // Function called from Player script
    public void SetDamageEnemy (float _damageAmount)
    {
        damageAmount = _damageAmount;        
    }

    public void ApplyDamageToEnemy()
    {
        currentHealth -= damageAmount;

        if (currentHealth <= Mathf.Epsilon)
        {
            Debug.Log("Vida del enemigo: " + " " + currentHealth);
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemigo muerto");
        MoveEnemy(Vector3.zero);
        // TODO: anim.SetBool("isDead", true);
        // TODO: play enemy death sound depending on enemy
        // TODO: character deactivation (collider, script...)
        Destroy(this.gameObject, 1f); // TEMP
    }
    #endregion

    // TEMP Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerAttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerLostRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
    }
}
