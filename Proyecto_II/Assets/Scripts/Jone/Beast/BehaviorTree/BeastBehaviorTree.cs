using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Jone Sainz Egea
// Script que contiene la lógica del árbol de comportamiento de la Bestia
// 06/04/2025 pasar el funcionamiento de Beast_V3 a árbol de comportamiento
public class BeastBehaviorTree : BTree
{
    [SerializeField] private float searchRadius = 30f;
    [SerializeField] private float arrivalThreshold = 5.5f;
    
    public static bool isConstrained = false;
    public static bool beastWaitingOrder = false; // Cuando se va a abrir el menú de interacción hay que comp

    private NavMeshAgent agent;
    public static Animator anim;
    private Coroutine waitCoroutine; // Para gestionar que solo haya una corrutina en marcha a la vez

    [SerializeField] private Blackboard blackboard = new Blackboard(); // Mostrar en inspector

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        blackboard.SetValue("isConstrained", false);
    }

    // Árbol de comportamiento de la Bestia:
    protected override Node SetupTree()
    {
        Node root = new Selector (new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckFlag(blackboard, "lookForTarget", new GetInterestPoint(transform, searchRadius, blackboard)),
                new GoToInterestPoint(transform, agent, arrivalThreshold, blackboard),
                new Sequence (new List<Node>
                {
                    new CheckFlag(blackboard, "isConstrained", new Sequence(new List<Node>
                    {
                        new CheckSmellable(blackboard),
                        new Smell(blackboard, this, agent, 0.5f, 5f),
                        new SetRandomFlag(blackboard, "shouldSit", 50f),
                        new SetRandomFlag(blackboard, "shouldSleep", 30f),
                        new Selector(new List<Node>
                        {
                            new CheckFlag(blackboard, "shouldSit", new Sit(blackboard, this, 3f, 6f)),
                            new CheckFlag(blackboard, "shouldSleep", new Sleep(blackboard, this, 5f, 10f)),
                            new GoBackToLooking(blackboard)
                        })                       
                    }), false),
                    
                })
            }),
            new Sleep(blackboard, this, 2f, 10f) // Always true
        });
        return root;
    }

    // Método que gestiona que solo haya una corrutina en marcha cada vez
    public void StartNewCoroutine(IEnumerator newCoroutine)
    {
        if (waitCoroutine != null)
        {
            Debug.LogWarning("Ya hay una corrutina activa. Ignorando nueva solicitud.");
            return;
        }

        Debug.Log("Iniciando nueva corrutina");
        waitCoroutine = StartCoroutine(WrapCoroutine(newCoroutine));
    }

    private IEnumerator WrapCoroutine(IEnumerator coroutine)
    {
        yield return coroutine;
        waitCoroutine = null;
    }

    // Called from Brisa
    public void ConstrainBeast()
    {
        isConstrained = true;
        blackboard.SetValue("isConstrained", true);
    }
}
