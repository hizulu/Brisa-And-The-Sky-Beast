using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player Player;
    public Animator AnimEnemy { get; private set; }
    public Material matForDepuration;

    private EnemyStateMachine enemyStateMachine;

    public Rigidbody rb;


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

        AnimEnemy = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
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
}
