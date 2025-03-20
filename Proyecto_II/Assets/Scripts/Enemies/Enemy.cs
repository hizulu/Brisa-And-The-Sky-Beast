using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player Player;
    public Animator AnimEnemy { get; private set; }

    private EnemyStateMachine enemyStateMachine;

    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }

    private void Awake()
    {
        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        AnimEnemy = GetComponent<Animator>();
        enemyStateMachine = new EnemyStateMachine(this);
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        EnemyIdleBaseInstance.Initialize(gameObject, this);
        enemyStateMachine.ChangeState(enemyStateMachine.EnemeyIdleState);
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
