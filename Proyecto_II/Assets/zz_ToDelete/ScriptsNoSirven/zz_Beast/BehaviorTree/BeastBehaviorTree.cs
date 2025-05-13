//using BehaviorTree;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//// Jone Sainz Egea
//// Script que contiene la lógica del árbol de comportamiento de la Bestia
//// 06/04/2025 pasar el funcionamiento de Beast_V3 a árbol de comportamiento
//public class BeastBehaviorTree : BTree
//{
//    [SerializeField] private float searchRadius = 30f;
//    [SerializeField] private float arrivalThreshold = 5.5f;
//    [SerializeField] private Transform playerTransform;
    
//    public static bool isConstrained = false;
//    public static bool beastWaitingOrder = false; // Cuando se va a abrir el menú de interacción hay que comprobar
//    public static bool beastMenuOpened = false;
//    public static bool beastMenuClosed = false;

//    public static NavMeshAgent agent;
//    public static Animator anim;

//    private Coroutine waitCoroutine; // Para gestionar que solo haya una corrutina en marcha a la vez
//    private ICoroutineNode coroutineOwner;

//    [SerializeField] private static Blackboard blackboard = new Blackboard(); // Mostrar en inspector

//    private void Awake()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        anim = GetComponent<Animator>();

//        blackboard.SetValue("isConstrained", false);
//        blackboard.SetValue("lookForTarget", true);
//    }

//    // Árbol de comportamiento de la Bestia:
//    protected override Node SetupTree()
//    {
//        Node root = new Selector (new List<Node>
//        {
//            // Beast Constrained Tree:
//            new Selector (new List<Node>
//            {
//                new Sequence(new List<Node>
//                {
//                    new CheckFlag(blackboard, "isConstrained", new GoToPlayer(blackboard, transform, playerTransform, agent, arrivalThreshold)),
//                    new WaitForOrder(blackboard, this, transform, playerTransform, 7f, 8f),
//                })
//            }),
//            // Beast Free tree:
//            new Sequence(new List<Node>
//            {
//                new CheckFlag(blackboard, "lookForTarget", new GetInterestPoint(transform, searchRadius, blackboard)),
//                new GoToInterestPoint(blackboard, transform, agent, arrivalThreshold),
//                new Sequence (new List<Node>
//                {
//                    new CheckFlag(blackboard, "isConstrained", new Sequence(new List<Node>
//                    {
//                        new CheckSmellable(blackboard),
//                        new Smell(blackboard, this, agent, 0.5f, 5f),
//                        new SetRandomFlag(blackboard, "shouldSit", 50f),
//                        new SetRandomFlag(blackboard, "shouldSleep", 30f),
//                        new Selector(new List<Node>
//                        {
//                            new CheckFlag(blackboard, "shouldSit", new Sit(blackboard, this, 3f, 6f)),
//                            new CheckFlag(blackboard, "shouldSleep", new Sleep(blackboard, this, 5f, 10f)),
//                            new GoBackToLooking(blackboard)
//                        })                       
//                    }), false),
                    
//                })
//            }),
//            new AlwaysTrue()
//            //new Sleep(blackboard, this, 2f, 10f) // Always true
//        });
//        return root;
//    }

//    // Método que gestiona que solo haya una corrutina en marcha cada vez
//    public void StartNewCoroutine(IEnumerator newCoroutine, ICoroutineNode owner)
//    {
//        if (waitCoroutine != null)
//        {
//            StopCoroutine(waitCoroutine);
//            if (coroutineOwner != null)
//            {
//                coroutineOwner.OnCoroutineEnd();
//            }
//        }

//        coroutineOwner = owner;
//        waitCoroutine = StartCoroutine(newCoroutine);
//    }

//    // Called from Brisa
//    public static void CallBeast()
//    {
//        if (blackboard.GetValue<bool>("isConstrained"))
//        {
//            Debug.LogWarning("CallBeast fue llamado, pero ya estaba en modo constrained");
//            return;
//        }

//        isConstrained = true;
//        blackboard.SetValue("isConstrained", true);
//        agent.ResetPath();
//        Debug.Log("Bestia llamada por el jugador");
//    }

//    public static void OpenBeastMenu()
//    {
//        beastMenuOpened = true;
//        blackboard.SetValue("isMenuOpen", true);
//    }

//    public static void CloseBeastMenu(bool optionSelected)
//    {
//        beastMenuOpened = false;
//        blackboard.SetValue("isMenuOpen", false);

//        if (!optionSelected)
//            beastMenuClosed = true;
//    }
//}
