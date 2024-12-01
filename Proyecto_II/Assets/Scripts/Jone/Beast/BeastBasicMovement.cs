using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;


/* NOMBRE CLASE: BeastBasicMovement
 * AUTOR: Jone Sainz Egea
 * FECHA: 10/11/2024
 * DESCRIPCIÓN: Script base que se encarga del movimiento de la bestia con estados
 * VERSIÓN: 1.0 movimiento base de seguir al jugador
 *              1.1. añadir aleatoriedad al movimiento base de seguir al jugador
 *          2.0 cambio de estados a árbol jerárquico
 */
public class BeastBasicMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float walkDistance = 6f;
    [SerializeField] float runDistance = 4f;
    [SerializeField] float followDistance = 2f;
    [SerializeField] float waitDistance = 10f;

    //[SerializeField] float wanderCooldown = 5f;

    [SerializeField] float walkingRange = 30f;
    [SerializeField] float wanderingRange = 50f;
    [SerializeField] float freeCloseDistance = 10f;
    [SerializeField] float treesDetectionRange = 5f;
    [SerializeField] float treeDetectionProbabilityWalking = 0.3f;
    [SerializeField] float treeDetectionProbabilityWandering = 0.6f;

    private bool playerWalking = false;
    private bool playerRunning = false;

    private Vector3 randomMovement;
    private NavMeshAgent bestia;
    private GameObject[] trees;
    [SerializeField] private Animator animBestia;

    private enum BeastState { Free, Constrained}
    private enum BeastFreeState { Walk, Run, Wander, Sleep }
    private enum BeastConstrainedState { Approach, Wait, Sit }
    private BeastState currentState = BeastState.Free;
    private BeastFreeState currentFreeState = BeastFreeState.Walk;
    private BeastConstrainedState currentConstrainedState = BeastConstrainedState.Approach;

    private bool beastFree = true;

    private float idleTime = 0f;

    void Start()
    {
        bestia = GetComponent<NavMeshAgent>();
        trees = GameObject.FindGameObjectsWithTag("Arbol");
        animBestia = GetComponent<Animator>();
        Smell();
    }

    void Update()
    {
        UpdateBeastState();      
    }
    void UpdateBeastState()
    {        
        
        if (beastFree)
        {
            ChangeState(BeastState.Free); // Esto no debería estar aquí

            UpdateBeastFree(); // Esto sí
        }
        else
        {
            ChangeState(BeastState.Constrained);

            UpdateBeastConstrained();
        }      
    }

    void UpdateBeastFree()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
       
        switch (currentFreeState)
        {
            case BeastFreeState.Walk:
                if (distanceToPlayer > walkingRange)
                    ChangeState(BeastFreeState.Run);
                else if (!playerWalking)
                    ChangeState(BeastFreeState.Wander);
                else
                    Walk();
                break;

            case BeastFreeState.Run:
                if (playerRunning)
                {
                    ChangeState(BeastFreeState.Run);
                    Run();
                }
                else 
                    ChangeState(BeastFreeState.Walk);
                break;

            case BeastFreeState.Wander:
                if (distanceToPlayer > wanderingRange)
                    ChangeState(BeastFreeState.Run);
                else if (playerWalking)
                    ChangeState(BeastFreeState.Walk);
                else
                    Wander();
                break;

            case BeastFreeState.Sleep:
                if (distanceToPlayer > waitDistance)
                    ChangeState(BeastState.Run);
                else
                    idleTime += Time.deltaTime;
                if (idleTime >= wanderCooldown)
                {
                    ChangeState(BeastState.Wander);
                    idleTime = 0f;
                }
                break;
        }
    }

    void UpdateBeastConstrained()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentConstrainedState)
        {
            case BeastConstrainedState.Approach:
                if (distanceToPlayer > walkingRange)
                    ChangeState(BeastFreeState.Run);
                else if (!playerWalking)
                    ChangeState(BeastFreeState.Wander);
                else
                    Walk();
                break;

            case BeastConstrainedState.Wait:
                if (playerRunning)
                {
                    ChangeState(BeastFreeState.Run);
                    Run();
                }
                else
                    ChangeState(BeastFreeState.Walk);
                break;

            case BeastConstrainedState.Sit:
                if (distanceToPlayer > wanderingRange)
                    ChangeState(BeastFreeState.Run);
                else if (playerWalking)
                    ChangeState(BeastFreeState.Walk);
                else
                    Wander();
                break;
        }
    }

    // Sobrecarga de métodos
    void ChangeState(BeastState newState)
    {
        currentState = newState;
    }
    void ChangeState(BeastFreeState newState)
    {
        currentFreeState = newState;
    }
    void ChangeState(BeastConstrainedState newState)
    {
        currentConstrainedState = newState;
    }

    void FollowPlayer(float targetDistance, float randomness)
    {
        Vector3 followPosition = player.position - player.forward * targetDistance;
        followPosition += new Vector3(Random.Range(-randomness, randomness), 0, Random.Range(-randomness, randomness));
        MoveToPosition(followPosition);
        bestia.SetDestination(followPosition);
    }

    void MoveTowardsPlayer()
    {
        MoveToPosition(player.position);
    }

    void Walk()
    {
        //FollowPlayer(walkDistance, 2f); // Follow with random offset
    }

    void Run()
    {
        bestia.SetDestination(player.position);
    }

    void Wander()
    {
       // Vector3 wanderPosition = player.position + new Vector3(Random.Range(-wanderDistance, wanderDistance), 0, Random.Range(-wanderDistance, wanderDistance));
       // MoveToPosition(wanderPosition);
    }

    void MoveToPosition(Vector3 position)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * 3f); // Velocidad de movimiento ajustable
        transform.LookAt(position); // Mirar hacia la posición de destino
    }

    // Hecho por Sara (no sé cuando pero antes del 10/11/2024)
    void Smell()
    {
        if (trees.Length > 0)
        {
            GameObject randomTree = trees[Random.Range(0, trees.Length)];
            Vector3 treePosition = randomTree.transform.position;

            Vector3 randomOffset = Random.insideUnitSphere * 5f;
            randomOffset.y = 0;

            randomMovement = treePosition + randomOffset;
        }
        else
        {
            Vector3 randomOffset = Random.insideUnitSphere * followDistance;
            randomOffset.y = 0;

            randomMovement = player.position + randomOffset;
        }
    }


    #region CalledFromOtherScripts
    public void CallBeast() // Llamar a este desde el jugador
    {
        beastFree = false;
        //ChangeState(BeastConstrainedState.Approach);
    }

    public void PlayerOnIdle()
    {
        ChangeState(BeastFreeState.Wander);
    }

    public void PlayerOnSleepForLong()
    {
        ChangeState(BeastFreeState.Sleep);
    }
    #endregion
    /*
    // Dibujar círculos para visualizar las distancias en el editor
    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(player.position, walkDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, runDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.position, followDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, waitDistance);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(player.position, wanderDistance);
        }
    }
    */
    
}
