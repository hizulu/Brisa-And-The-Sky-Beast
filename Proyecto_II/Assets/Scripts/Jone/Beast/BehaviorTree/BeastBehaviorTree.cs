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
    private NavMeshAgent agent;
    public static bool isConstrained = false;
    public static Animator anim;
    private Coroutine waitCoroutine; // Para gestionar que solo haya una corrutina en marcha a la vez

    [SerializeField] private Blackboard blackboard = new Blackboard(); // Mostrar en inspector

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    protected override Node SetupTree()
    {
        Node root = new Sequence (new List<Node>
        {
            new GetInterestPoint(transform, searchRadius, blackboard),
            new GoToInterestPoint(transform, agent, arrivalThreshold, blackboard),
            new Sequence (new List<Node>
            {
                new CheckNotConstrained(),
                new Sequence (new List<Node>
                {
                    new CheckSmellable(blackboard),
                    new Smell(blackboard)
                })
            })
        });
        return root;
    }

    // Método que gestiona que solo haya una corrutina en marcha cada vez
    private void StartNewCoroutine(IEnumerator newCorroutine)
    {
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine); // Asegura que no haya otra corrutina en marcha
        }
        waitCoroutine = StartCoroutine(newCorroutine);
    }

    // Called from Brisa
    public void ConstrainBeast()
    {
        isConstrained = true;
    }
}
