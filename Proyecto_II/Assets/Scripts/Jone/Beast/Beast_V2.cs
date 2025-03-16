using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

// Jone Sainz Egea
// Segunda versión de la bestia, cambios para optimizar el código
// 16/03/2025
public class Beast_V2 : MonoBehaviour
{
    [SerializeField] Transform player;

    [Header("Ranges and distances")]
    [SerializeField] float treesDetectionRange = 5f;
    [SerializeField] float waitingRange = 15f;
    [SerializeField] float runningDistance = 18f;
    [SerializeField] float freeCloseDistance = 20f;
    [SerializeField] float walkingRange = 28f;
    [SerializeField] float wanderingRange = 40f;
    [SerializeField] float playerOutOfRange = 150f;

    private float rotationSpeed = 6f;
    private float baseSpeed = 5f;
    private float baseAcceleration = 8f;
    private float stopTimer = 0f;
    private float stopTime = 3f;

    private bool playerWalking = false;
    private static bool playerRunning = false;
    private static bool beastCalled = false;
    private bool beastMenuOpen = false;

    private Vector3 randomRunningDestination = Vector3.zero;
    private Vector3 randomMovement = Vector3.zero;

    private NavMeshAgent bestia;
    private Animator animBestia;

    private enum BeastState { Free, Constrained }
    private enum BeastFreeState { Walk, Run, Wander, Sleep }
    private enum BeastConstrainedState { Approach, Wait, Sit }

    private BeastState currentState = BeastState.Free;
    private BeastFreeState currentFreeState = BeastFreeState.Walk;
    private BeastConstrainedState currentConstrainedState = BeastConstrainedState.Approach;

    private static bool beastFree = true;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        bestia = GetComponent<NavMeshAgent>();
        animBestia = GetComponent<Animator>();
        lastPlayerPosition = player.position;
        Smell();
    }

    void Update()
    {
        UpdateBeastState();
        animBestia.SetBool("bestiaIsWalking", bestia.velocity.sqrMagnitude > 0.2f);
    }

    void UpdateBeastState()
    {
        if (beastFree)
        {
            ChangeState(BeastState.Free);
            UpdateBeastFree();
        }
        else
        {
            ChangeState(BeastState.Constrained);
            UpdateBeastConstrained();
        }
    }

    void MoveBeast(Vector3 destination, float desiredSpeed, float desiredAcceleration)
    {
        if (bestia.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(bestia.velocity.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        bestia.acceleration = desiredAcceleration;
        bestia.speed = desiredSpeed;

        if (!bestia.hasPath || bestia.remainingDistance < 0.5f)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(destination, out hit, 2.0f, NavMesh.AllAreas))
            {
                bestia.SetDestination(hit.position);
            }
        }
    }

    void UpdateBeastFree()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (playerRunning || distanceToPlayer > freeCloseDistance)
            ChangeState(BeastFreeState.Run);

        switch (currentFreeState)
        {
            case BeastFreeState.Walk:
                if (distanceToPlayer > walkingRange)
                    ChangeState(BeastFreeState.Run);
                else
                    Walk();
                break;

            case BeastFreeState.Run:
                Run();
                break;

            case BeastFreeState.Wander:
                if (distanceToPlayer > wanderingRange)
                    ChangeState(BeastFreeState.Run);
                else
                    Wander();
                break;
        }
    }

    void Walk()
    {
        if (Vector3.Distance(transform.position, randomMovement) < 1f)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopTime)
            {
                stopTimer = 0f;
                Smell();
            }
        }
        else
        {
            MoveBeast(randomMovement, baseSpeed, baseAcceleration);
        }
    }

    void Run()
    {
        MoveBeast(player.position + randomRunningDestination, baseSpeed + 1, baseAcceleration + 1);
    }

    void Wander()
    {
        if (Vector3.Distance(transform.position, randomMovement) < 1f)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopTime)
            {
                stopTimer = 0f;
                Smell();
            }
        }
        else
        {
            MoveBeast(randomMovement, baseSpeed - 1, baseAcceleration - 1);
        }
    }

    void UpdateBeastConstrained()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (beastCalled)
            ChangeState(BeastConstrainedState.Approach);

        switch (currentConstrainedState)
        {
            case BeastConstrainedState.Approach:
                if (distanceToPlayer < waitingRange)
                {
                    beastCalled = false;
                    ChangeState(BeastConstrainedState.Wait);
                }
                else
                    MoveBeast(player.position, baseSpeed + 1, baseAcceleration + 2);
                break;
        }
    }

    void Smell()
    {
        Collider[] treesInRange = Physics.OverlapSphere(transform.position, treesDetectionRange, LayerMask.GetMask("Arbol"));
        if (treesInRange.Length > 0)
        {
            Transform randomTree = treesInRange[Random.Range(0, treesInRange.Length)].transform;
            randomMovement = randomTree.position + Random.insideUnitSphere * 5f;
        }
        else
        {
            randomMovement = player.position + Random.insideUnitSphere * 5f;
        }
    }

    void ChangeState(BeastState newState) => currentState = newState;
    void ChangeState(BeastFreeState newState) => currentFreeState = newState;
    void ChangeState(BeastConstrainedState newState) => currentConstrainedState = newState;
}

