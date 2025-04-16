using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Jone Sainz Egea
// 15/04/2025
public class Beast : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Animator anim;
    [SerializeField] public Transform playerTransform;

    [Header("Parámetros")]
    [SerializeField] public float arrivalThreshold = 5f;
    [SerializeField] public float freeRoamRadius = 30f;
    [SerializeField] public float interactionThreshold = 8f;

    private BeastState currentState;

    public Blackboard blackboard { get; private set; }

    private Coroutine activeCoroutine; // Para gestionar que solo haya una corrutina en marcha a la vez
    private ICoroutineNode coroutineOwner;

    private void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (anim == null) anim = GetComponent<Animator>();
        if (blackboard == null) blackboard = new Blackboard();

        // Comenzamos en estado de libertad
        TransitionToState(new BeastFreeState());
    }

    private void Update()
    {
        currentState?.OnUpdate(this);
    }

    public void TransitionToState(BeastState newState)
    {
        currentState?.OnExit(this);
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

    // Called from Brisa script
    public void CallBeast()
    {
        blackboard.SetValue("isConstrained", true);
        agent.ResetPath();

        TransitionToState(new BeastConstrainedState());

        Debug.Log("Bestia llamada por el jugador");
    }
}
