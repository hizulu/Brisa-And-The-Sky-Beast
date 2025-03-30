using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Jone Sainz Egea
// Tercera versión de la bestia, sistema de prioridades
// 16/03/2025 funcionamiento básico de puntos de interés
    // 24/03/2025 Se añade interés en Brisa
public class Beast_V3 : MonoBehaviour
{
    [SerializeField] float searchRadius = 10f;
    private NavMeshAgent agent;
    public string interestTag = "InterestObject";
    private PointOfInterest currentTarget;
    List<PointOfInterest> interestPoints;

    private bool isWaiting = false;
    private Coroutine waitCoroutine;

    [Space(10)]
    [Header("InterestInBrisa")]
    [SerializeField] GameObject playerGO;
    [SerializeField] float baseInterestInBrisa = 5f;
    [SerializeField] float growthFactorInterestInBrisa = 0.05f;
    private float interestInBrisa = 0f;
    private bool interestedInBrisa = false;

    private bool beastIsTrapped = true;
    public static bool beastConstrained = false;

    private Animator anim;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();             
        anim = GetComponent<Animator>();
        anim.SetBool("bestiaIsWalking", false);
    }

    #region Lógica que será sustituida por el árbol de BeastTree
    private void Update()
    {
        if (!beastIsTrapped)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                CallBeast();
            if (!beastConstrained)
            {
                interestInBrisa = GetInterestInBrisa();

                if (interestedInBrisa)
                {
                    if (Vector3.Distance(transform.position, playerGO.transform.position) < 6f)
                        InteractWithBrisa();
                    else
                    {
                        anim.SetBool("bestiaIsWalking", true);
                        agent.SetDestination(playerGO.transform.position);
                    }
                }
                if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < 5f)
                {
                    InteractWithPoint();
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, playerGO.transform.position) > 6f)
                    agent.SetDestination(playerGO.transform.position);
                else
                {
                    if (!isWaiting)
                        StartNewCoroutine(WaitForOrderOrTimeout(10f));
                }
            }
        }
        else
        {
            anim.SetBool("bestiaIsWalking", false);
        }
    }
    #endregion

    #region Lógica que irá en el árbol de BeastFree
    private void FindBestInterestPoint()
    {
        interestInBrisa = GetInterestInBrisa();
        if (interestInBrisa > 50f)
        {
            Debug.Log("Brisa es el destino");
            interestedInBrisa = true;           
        }
        else
        {
            GetPointsOfInterest();

            if (interestPoints.Count > 0)
            {
                currentTarget = GetHighestInterestPoint(interestPoints);

                if (currentTarget != null)
                {
                    agent.SetDestination(currentTarget.transform.position);
                }
            }
            else if (!isWaiting)
            {
                StartNewCoroutine(WaitAndSearch(10f));
            }
        }        
    }

    private float GetInterestInBrisa()
    {
        float distance = Vector3.Distance(transform.position, playerGO.transform.position);
        return baseInterestInBrisa* Mathf.Exp(growthFactorInterestInBrisa * distance); // Aumento exponencial del interés en Brisa conforme se aleja    
    }

    private void GetPointsOfInterest()
    {
        interestPoints = new List<PointOfInterest>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(interestTag))
            {
                PointOfInterest poi = col.GetComponent<PointOfInterest>();
                if (poi != null)
                {
                    interestPoints.Add(poi);
                }
            }
        }
    }

    private PointOfInterest GetHighestInterestPoint(List<PointOfInterest> points)
    {
        PointOfInterest bestPoint = null;
        float highestInterest = 0f;

        foreach (var point in points)
        {
            float interestValue = point.GetInterestValue(transform);
            if (interestValue > highestInterest)
            {
                highestInterest = interestValue;
                bestPoint = point;
            }
        }
        return bestPoint;
    }
    #endregion

    #region Acciones BeastFree
    private void InteractWithPoint()
    {
        if (currentTarget != null)
        {
            anim.SetBool("bestiaIsWalking", false);
            Debug.Log($"Interacted with {currentTarget.name}, interest consumed.");
            currentTarget.ConsumeInterest();
            currentTarget = null;
            StartNewCoroutine(WaitAndSearch(2f));
        }
    }

    private void InteractWithBrisa()
    {
        // TODO: Sit and wait for 5 seconds (if Brisa goes move)
        StartNewCoroutine(WaitAndSearch(5f));
        interestedInBrisa = false;
    }

    private IEnumerator WaitAndSearch(float waitingTime)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitingTime);
        isWaiting = false;

        Debug.Log("Looking for new destination");
        FindBestInterestPoint();

        if (currentTarget == null)
        {
            Debug.Log("No interest points found, waiting again");
            StartNewCoroutine(WaitAndSearch(10f));
        }
    }
    #endregion

    #region BeastConstrained
    private IEnumerator WaitForOrderOrTimeout(float waitTime)
    {
        isWaiting = true;
        float elapsedTime = 0f;
        Debug.Log($"Empieza cuenta atrás de {waitTime} segundos");
        while (elapsedTime < waitTime)
        {
            if (Input.GetKeyDown(KeyCode.Tab)) //TODO: sustituirlo por NEW INPUT SYSTEM
            {
                Debug.Log("TAB presionado: Interrumpiendo espera."); 
                OnTabPressed();
                yield break; // Termina la corrutina inmediatamente
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Tiempo de espera completado: Ejecutando función por timeout.");
        OnTimeoutReached();
        isWaiting = false;
    }

    private void OnTabPressed()
    {
        Debug.Log("Ejecutando acción por TAB.");
        // TODO: sustituirlo por función de abrir menú y ¿pausa del juego?
        beastConstrained = false;
        StartNewCoroutine(WaitAndSearch(10f));
    }

    private void OnTimeoutReached()
    {
        Debug.Log("Ejecutando acción tras esperar 10 segundos.");
        beastConstrained = false;
        StartNewCoroutine(WaitAndSearch(4f));
    }

    #endregion
    
    // Método que gestiona que solo haya una corrutina en marcha cada vez
    private void StartNewCoroutine(IEnumerator newCorroutine)
    {
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine); // Asegura que no haya otra corrutina en marcha
        }
        waitCoroutine = StartCoroutine(newCorroutine);
    }

    // Se llama a este método desde el scritp de Brisa
    public static void CallBeast()
    {
        beastConstrained = true;
        Debug.Log("Beast has been called.");
    }

    public void SetBeastFreeFromCage()
    {
        beastIsTrapped = false;
        Invoke(nameof(FindBestInterestPoint), 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
        if (playerGO == null) return;
        float distance = Vector3.Distance(transform.position, playerGO.transform.position);
        float printInterestInBrisa = baseInterestInBrisa * Mathf.Exp(growthFactorInterestInBrisa * distance);
        UnityEditor.Handles.Label(playerGO.transform.position + Vector3.up * 4, $"Interest: {printInterestInBrisa}");
    }
}
