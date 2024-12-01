using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
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



    [SerializeField] float walkingRange = 20f;
    [SerializeField] float waitingRange = 7f;
    [SerializeField] float wanderingRange = 40f;
    [SerializeField] float runningDistance = 10f;
    [SerializeField] float freeCloseDistance = 10f;
    [SerializeField] float treesDetectionRange = 5f;
    [SerializeField] float treeDetectionProbabilityWalking = 0.3f;
    [SerializeField] float treeDetectionProbabilityWandering = 0.6f;
    [SerializeField] float playerOutOfRange = 150f;

    private bool playerWalking = false;
    private static bool playerRunning = false;

    [SerializeField] InputActionReference openBeastMenu;

    private float stopTimer = 0f;
    [SerializeField] private float stopTime = 3f;
    [SerializeField] float followDistance = 10f;
    private Vector3 randomMovement;
    private NavMeshAgent bestia;
    private GameObject[] trees;
    Animator animBestia;

    private enum BeastState { Free, Constrained }
    private enum BeastFreeState { Walk, Run, Wander, Sleep }
    private enum BeastConstrainedState { Approach, Wait, Sit }
    private BeastState currentState = BeastState.Free;
    private BeastFreeState currentFreeState = BeastFreeState.Walk;
    private BeastConstrainedState currentConstrainedState = BeastConstrainedState.Approach;

    private static bool beastFree = true;

    private Vector3 lastPlayerPosition; // Última posición registrada.

    private float idleTime = 0f;

    private void OnEnable()
    {
        openBeastMenu.action.started += OpenBeastMenu;
    }

    private void OnDisable()
    {
        openBeastMenu.action.started -= OpenBeastMenu;
    }

    void Start()
    {
        bestia = GetComponent<NavMeshAgent>();
        trees = GameObject.FindGameObjectsWithTag("Arbol");
        animBestia = GetComponent<Animator>();

        lastPlayerPosition = player.position;
        Smell();
    }

    void Update()
    {
        UpdateBeastState();

        if (bestia.velocity.sqrMagnitude > 0.2f)
        {
            animBestia.SetBool("bestiaIsWalking", true);
        }
        else
        {
            animBestia.SetBool("bestiaIsWalking", false);
        }
    }
    void UpdateBeastState()
    {        
        
        if (beastFree)
        {
            Debug.Log("BEAST Free");
            ChangeState(BeastState.Free); // Esto no debería estar aquí

            UpdateBeastFree(); // Esto sí
        }
        else
        {
            Debug.Log("BEAST Constrained");
            ChangeState(BeastState.Constrained);

            UpdateBeastConstrained();
        }      
    }

    #region Beast Free States
    void UpdateBeastFree()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (playerRunning)
        {
            ChangeState(BeastFreeState.Run);
        }

        switch (currentFreeState)
        {
            case BeastFreeState.Walk:
                if (distanceToPlayer > walkingRange)
                    ChangeState(BeastFreeState.Run);
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
                else
                    Wander();
                break;

            case BeastFreeState.Sleep:
                /*
                if (distanceToPlayer > waitDistance)
                    ChangeState(BeastState.Run);
                else
                    idleTime += Time.deltaTime;
                if (idleTime >= wanderCooldown)
                {
                    ChangeState(BeastState.Wander);
                    idleTime = 0f;
                }*/
                break;

        }
    }


    void Walk()
    {
        Debug.Log("BEAST Walking");

        if (PlayerWalking())
        {
            Debug.Log("El jugador está caminando.");
        }
        else
        {
            Debug.Log("El jugador no está caminando.");

            ChangeState(BeastFreeState.Wander);
        }
    }

    void Run()
    {
        Debug.Log(" BEAST Running");
        Vector3 destination = new Vector3(player.position.x - 5f, player.position.y, player.position.z - 5f);
        bestia.SetDestination(destination);
        
    }

    void Wander()
    {
        Debug.Log("BEAST Wandering");

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
            bestia.SetDestination(randomMovement);
        }


        if (PlayerWalking())
        {
            Debug.Log("El jugador está caminando.");

            ChangeState(BeastFreeState.Walk);
        }
        else
        {
            Debug.Log("El jugador no está caminando.");
        }
    }

    #endregion

    #region Beast Constrained States
    void UpdateBeastConstrained()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentConstrainedState)
        {
            case BeastConstrainedState.Approach:
                if (distanceToPlayer <= waitingRange)
                    ChangeState(BeastConstrainedState.Wait);
                else
                    bestia.SetDestination(player.position);
                break;

            case BeastConstrainedState.Wait:
                // TODO: aleatoriedad en la que pueda pasar o a estado libre o a SIT
                Waiting();
                break;

            case BeastConstrainedState.Sit:
                if (distanceToPlayer > walkingRange)
                    beastFree = true;
                else
                    Debug.Log("BEAST Sitting");
                break;
        }
    }

    void Waiting()
    {
        Debug.Log("BEAST Waiting");
        /*if (!beastFree)
            StartCoroutine(BeastWaiting());
        else
            return;*/
        // Empezar a contar y si pasan 8 segundos sin ábrir el menú cambiar de estado
        // beastFree = true;
    }

    void Sitting()
    {
        // Mirar al jugador
    }

    IEnumerator BeastWaiting()
    {
        yield return new WaitForSecondsRealtime(8f);

        ChangeState(BeastConstrainedState.Sit);
    }

    void OpenBeastMenu(InputAction.CallbackContext context)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= waitingRange && currentConstrainedState == BeastConstrainedState.Wait)
        {
            // Abrir panel de opciones
            Debug.Log("Llama a acción de plataforma");
            PlatformAction();
        }
        Debug.Log("Ha terminado de abrir el menú)");
    }

    #endregion

    #region ChangeState Sobrecarga de métodos
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
    #endregion

    bool PlayerWalking()
    {
        // Compara la posición actual con la última posición registrada.
        if (player.position != lastPlayerPosition)
        {
            lastPlayerPosition = player.position; // Actualiza la posición.
            return true; // El jugador se ha movido.
        }

        return false; // El jugador no se ha movido.
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

    void PlatformAction()
    {
        BeastActionPlatform.LinkBeast();
    }

    #region CalledFromOtherScripts
    public static void CallBeast() // Llamar a este desde el jugador
    {
        beastFree = false;
    }

    public void PlayerOnSleepForLong()
    {
        ChangeState(BeastFreeState.Sleep);
    }

    public static void PlayerRunning()
    {
        playerRunning = !playerRunning;
    }

    #endregion

    // Dibujar círculos para visualizar las distancias en el editor
    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(player.position, walkingRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, waitingRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.position, wanderingRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, freeCloseDistance);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(player.position, treesDetectionRange);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.position, playerOutOfRange);
        }
    }  
    
}
