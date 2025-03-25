using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;
    public Animator anim { get; private set; }
    public Rigidbody rb;
    public Material matForDepuration;

    [SerializeField] float maxHealth = 100f;
    private float currentHealth;

    private EnemyStateMachine enemyStateMachine;

    #region States
    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;
    [SerializeField] private EnemyPatrolSOBase EnemyPatrolBase;
    [SerializeField] private EnemyChaseSOBase EnemyChaseBase;
    [SerializeField] private EnemyAttackSOBase EnemyAttackBase;
    [SerializeField] private EnemyRetreatSOBase EnemyRetreatBase;

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyPatrolSOBase EnemyPatrolBaseInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }
    public EnemyRetreatSOBase EnemyRetreatBaseInstance { get; set; }
    #endregion

    #region State change Checks
    public bool doIdle = true;
    public bool doPatrol = false;
    public bool doChase = false;
    public bool doAttack = false;
    public bool doRetreat = false;
    #endregion

    private void Awake()
    {
        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        EnemyPatrolBaseInstance = Instantiate(EnemyPatrolBase);
        EnemyChaseBaseInstance = Instantiate(EnemyChaseBase);
        EnemyAttackBaseInstance = Instantiate(EnemyAttackBase);
        EnemyRetreatBaseInstance = Instantiate(EnemyRetreatBase);

        enemyStateMachine = new EnemyStateMachine(this);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyPatrolBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);
        EnemyRetreatBaseInstance.Initialize(gameObject, this);

        enemyStateMachine.ChangeState(enemyStateMachine.EnemyIdleState);
    }

    private void Update()
    {
        enemyStateMachine.UpdateLogic();
    }

    private void FixedUpdate()
    {
        enemyStateMachine.UpdatePhysics();
    }

    public void MoveEnemy(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    #region DamageRelated Functions
    // Function called from Player script
    public void DamageEnemy (float damageAmount)
    {
        currentHealth -= damageAmount;
        // TODO: anim.SetTrigger("getDamaged");
        // TODO: play enemy damage sound depending on enemy
        matForDepuration.color = Color.red; // TEMP

        if (currentHealth <= Mathf.Epsilon)
        {
            Die();
        }
    }

    public void Die()
    {
        MoveEnemy(Vector3.zero);
        // TODO: anim.SetBool("isDead", true);
        // TODO: play enemy death sound depending on enemy
        // TODO: character deactivation (collider, script...)
        Destroy(this.gameObject, 1f); // TEMP
    }
    #endregion
}
