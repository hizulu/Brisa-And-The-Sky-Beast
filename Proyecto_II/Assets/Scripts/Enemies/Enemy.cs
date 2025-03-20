using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;
    public Animator anim { get; private set; }

    private EnemyStateMachine enemyStateMachine;

    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }

    private void Awake()
    {
        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        anim = GetComponent<Animator>();
        enemyStateMachine = new EnemyStateMachine(this);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        EnemyIdleBaseInstance.Initialize(gameObject, this);
        //Debug.Log("Este es el script del enemigo");
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
