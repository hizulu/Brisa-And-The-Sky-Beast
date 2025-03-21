using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player Player;
    public Animator AnimEnemy { get; private set; }
    public Material matForDepuration;

    private EnemyStateMachine enemyStateMachine;


    #region States
    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;
    [SerializeField] private EnemyPatrolSOBase EnemyPatrolBase;
    //[SerializeField] private EnemyChaseSOBase EnemyChaseBase;
    //[SerializeField] private EnemyAttackSOBase EnemyAttackBase;
    //[SerializeField] private EnemyRetreatSOBase EnemyRetreatBase;

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyPatrolSOBase EnemyPatrolBaseInstance { get; set; }
    //public EnemyChaseSOBase EnemyChaseBaseBaseInstance { get; set; }
    //public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }
    //public EnemyRetreatSOBase EnemyRetreatBaseInstance { get; set; }
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
        //EnemyChaseBaseBaseInstance = Instantiate(EnemyChaseBase);
        //EnemyAttackBaseInstance = Instantiate(EnemyAttackBase);
        //EnemyRetreatBaseInstance = Instantiate(EnemyRetreatBase);

        enemyStateMachine = new EnemyStateMachine(this);

        AnimEnemy = GetComponent<Animator>();
        //matForDepuration = GetComponent<Material>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        EnemyIdleBaseInstance.Initialize(gameObject, this);
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
}
