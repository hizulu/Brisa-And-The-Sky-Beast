using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR;


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


    [Header("Ranges and distances")]
    [SerializeField] float treesDetectionRange = 5f; // Cómo de cerca del árbol debe estar para que lo detecte
    [SerializeField] float waitingRange = 15f; // Distancia a la que se para del jugador
    [SerializeField] float runningDistance = 18f; // Distancia a la que corre del jugador
    [SerializeField] float freeCloseDistance = 20f; // Distancia a la que se tiene que acercar cuando se ha alejado
    [SerializeField] float walkingRange = 28f; // Rango de libertad de andar
    [SerializeField] float wanderingRange = 40f; // Rango de libertad de deambular
    [SerializeField] float playerOutOfRange = 150f; // Distancia a la que se considera fuera de rango
    private float smoothFactor = 5f;
    private float rotationSpeed = 8f;
    private float baseSpeed = 6f;
    private float baseAcceleration = 10f;

    // [SerializeField] float treeDetectionProbabilityWalking = 0.3f;
    // [SerializeField] float treeDetectionProbabilityWandering = 0.6f;

    // private bool playerWalking = false;
    private static bool playerRunning = false; // Se gestiona desde el script de player
    private static bool beastCalled = false;
    private bool beastMenuOpen = false;

    private Vector3 randomRunningDestination = Vector3.zero;

    [SerializeField] InputActionReference openBeastMenu;

    private float stopTimer = 0f;
    [SerializeField] private float stopTime = 3f;
    [SerializeField] float followDistance = 10f;
    private Vector3 randomMovement = Vector3.zero;
    private NavMeshAgent bestia;
    private GameObject[] trees;

    Animator animBestia;

    private enum BeastState { Free, Constrained }
    private enum BeastFreeState { Walk, Run, Wander, Sleep }
    private enum BeastConstrainedState { Approach, Wait, Sit }
    private BeastState currentState;
    private BeastFreeState currentFreeState = BeastFreeState.Walk;
    private BeastConstrainedState currentConstrainedState = BeastConstrainedState.Approach;

    private static bool beastFree = true;

    private Vector3 lastPlayerPosition; // Última posición registrada.

    //private float idleTime = 0f;

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
        
        // if (BeastOutOfRange() || BeastStuck()) {tp to player}

    }

    void MoveBeast(Vector3 destination, float desiredSpeed, float desiredAcceleration)
    {
        // Smooth rotation only if moving
        if (bestia.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(bestia.velocity.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Smooth acceleration and speed transitions
        bestia.acceleration = Mathf.Lerp(bestia.acceleration, desiredAcceleration, Time.deltaTime * smoothFactor);
        bestia.speed = Mathf.Lerp(bestia.speed, desiredSpeed, Time.deltaTime * smoothFactor);

        // Apply a slight random offset only when setting a new destination
        if (!bestia.hasPath || bestia.remainingDistance < 0.5f) // Check if the AI needs a new path
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            bestia.SetDestination(destination + randomOffset);
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
                if (playerRunning || distanceToPlayer > freeCloseDistance)
                {
                    Debug.Log("Player running es: " + playerRunning);
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
                TODO: lógica del estado de descanso
                }*/
                break;
        }
    }


    void Walk()
    {
        Debug.Log("BEAST Walking");

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

        if (!PlayerWalking())
        {
            ChangeState(BeastFreeState.Wander);
        }
    }

    void Run()
    {
        Debug.Log(" BEAST Running");
        Vector3 destination = player.position + randomRunningDestination;
        MoveBeast(destination, baseSpeed + 1, baseAcceleration + 1);
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
            MoveBeast(randomMovement, baseSpeed - 1, baseAcceleration - 1);
        }

        if (PlayerWalking())
        {
            ChangeState(BeastFreeState.Walk);
        }
    }

    void GetRandomDestinationWithinRunningRange()
    {       
        float angle = Random.Range(0f, Mathf.PI * 2);  // Ángulo aleatorio en radianes

        float xOffset = Mathf.Cos(angle) * runningDistance;
        float zOffset = Mathf.Sin(angle) * runningDistance;

        randomRunningDestination = new Vector3(xOffset, 0, zOffset);
    }

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

    #endregion

    #region Beast Constrained States
    void UpdateBeastConstrained()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (beastCalled)
        {
            ChangeState(BeastConstrainedState.Approach);
        }

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

        if (distanceToPlayer > playerOutOfRange)
        {
            beastFree = true;
        }
    }

    void Waiting()
    {
        Debug.Log("BEAST Waiting");
        StartCoroutine(BeastWaiting());
    }

    void Sitting()
    {
        // Mirar al jugador
    }

    
    IEnumerator BeastWaiting()
    {
        float elapsedTime = 0f;
        float waitTime = 8f;

        while (elapsedTime < waitTime)
        {
            if (beastMenuOpen)
                yield break; // Finaliza la corrutina

            elapsedTime += Time.unscaledDeltaTime; // Incrementa el tiempo teniendo en cuenta el tiempo real
            yield return null;
        }

        beastFree = true;
    }

    void OpenBeastMenu(InputAction.CallbackContext context)
    {
        beastMenuOpen = true;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= waitingRange && currentConstrainedState == BeastConstrainedState.Wait)
        {
            // Abrir panel de opciones
            Debug.Log("Llama a acción de plataforma");
            PlatformAction();
        }
        Debug.Log("Ha terminado de abrir el menú)");
        beastMenuOpen = false;
    }

    void PlatformAction()
    {
        BeastActionPlatform.LinkBeast();
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
        if (newState == BeastFreeState.Run)
        {
            GetRandomDestinationWithinRunningRange();
        }
    }
    void ChangeState(BeastConstrainedState newState)
    {
        currentConstrainedState = newState;
    }
    #endregion

    

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
    public static void CallBeast() // Llamar a este desde el jugador
    {
        beastFree = false;
        beastCalled = true;        
    }

    public void PlayerOnSleepForLong()
    {
        ChangeState(BeastFreeState.Sleep);
    }

    public static void PlayerRunning(bool playerIsRunning)
    {
        playerRunning = playerIsRunning;
    }

    public static void GiveBeastFreedom()
    {
        beastFree = true;
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
