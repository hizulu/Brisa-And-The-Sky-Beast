using UnityEngine;
using UnityEngine.AI;

public class IABestia : MonoBehaviour
{
    [SerializeField] private Transform posPlayer;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float stopTime = 3f;
    [SerializeField] private float followRadius = 10f;
    private float stopTimer = 0f;
    private Vector3 randomMovement;
    private NavMeshAgent bestia;
    private GameObject[] trees;
    [SerializeField] private Animator animBestia;

    void Start()
    {
        bestia = GetComponent<NavMeshAgent>();
        trees = GameObject.FindGameObjectsWithTag("Arbol");
        animBestia = GetComponent<Animator>();
        Smell();
    }

    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        float playerDistance = Vector3.Distance(posPlayer.position, transform.position);

        if (bestia != null)
        {
            if (playerDistance > maxDistance)
            {
                bestia.SetDestination(posPlayer.position);
            }
            else
            {
                RandomBehaviour();
            }
        }

        if (bestia.velocity.sqrMagnitude > 0.2f)
        {
            animBestia.SetBool("bestiaIsWalking", true);
        }
        else
        {
            animBestia.SetBool("bestiaIsWalking", false);
        }
    }

    void RandomBehaviour()
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
            bestia.SetDestination(randomMovement);
        }
    }

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
            Vector3 randomOffset = Random.insideUnitSphere * followRadius;
            randomOffset.y = 0;

            randomMovement = posPlayer.position + randomOffset;
        }
    }
}
