using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;


/* NOMBRE CLASE: BeastBasicMovement
 * AUTOR: Jone Sainz Egea
 * FECHA: 10/11/2024
 * DESCRIPCIÓN: Script base que se encarga del movimiento de la bestia con estados
 * VERSIÓN: 1.0 movimiento base de seguir al jugador
 *              1.1. añadir aleatoriedad al movimiento base de seguir al jugador
 */
public class BeastBasicMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float walkDistance = 6f;
    [SerializeField] float runDistance = 4f;
    [SerializeField] float followDistance = 2f;
    [SerializeField] float waitDistance = 10f;
    [SerializeField] float wanderDistance = 20f;
    [SerializeField] float wanderCooldown = 5f;

    private enum BeastState { Walk, Run, Approach, Wait, Wander }
    private BeastState currentState = BeastState.Walk;
    private float idleTime = 0f;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case BeastState.Walk:
                if (distanceToPlayer <= runDistance)
                    ChangeState(BeastState.Run);
                else if (distanceToPlayer <= followDistance)
                    ChangeState(BeastState.Wait);
                else
                    FollowPlayer(walkDistance, 2f); // Follow with random offset
            break;

            case BeastState.Run:
                if (distanceToPlayer > walkDistance)
                    ChangeState(BeastState.Walk);
                else if (distanceToPlayer <= followDistance)
                    ChangeState(BeastState.Wait);
                else
                    FollowPlayer(runDistance, 1f);// Follow closely with less offset
            break;

            case BeastState.Approach:
                if (distanceToPlayer <= followDistance)
                    ChangeState(BeastState.Wait);
                else
                    MoveTowardsPlayer();
            break;

            case BeastState.Wait:
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

            case BeastState.Wander:
                if (distanceToPlayer <= followDistance)
                    ChangeState(BeastState.Run);
                else
                    WanderAround();
            break;
        }
    }

    void ChangeState(BeastState newState)
    {
        currentState = newState;
    }

    void FollowPlayer(float targetDistance, float randomness)
    {
        Vector3 followPosition = player.position - player.forward * targetDistance;
        //followPosition += new Vector3(Random.Range(-randomness, randomness), 0, Random.Range(-randomness, randomness));
        MoveToPosition(followPosition);
    }

    void MoveTowardsPlayer()
    {
        MoveToPosition(player.position);
    }

    void WanderAround()
    {
        Vector3 wanderPosition = player.position + new Vector3(Random.Range(-wanderDistance, wanderDistance), 0, Random.Range(-wanderDistance, wanderDistance));
        MoveToPosition(wanderPosition);
    }

    void MoveToPosition(Vector3 position)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * 3f); // Velocidad de movimiento ajustable
        transform.LookAt(position); // Mirar hacia la posición de destino
    }

    public void CallDog() // Llamar a este desde el jugador
    {
        ChangeState(BeastState.Approach);
    }

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
}
