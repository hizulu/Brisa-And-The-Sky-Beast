using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepWalkState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/05/2025
 * DESCRIPCIÓN: Clase que hereda de SheepStateTemplate y define la lógica del estado de Caminar de las ovejas.
 * VERSIÓN: 1.0.
 */

public class SheepWalkState : SheepStateTemplate
{
    public SheepWalkState(SheepStateMachine _sheepStateMachine) : base(_sheepStateMachine) { }

    #region Variables
    private float walkSpeed = 1f;
    private Vector3 walkDirection;

    private Vector3 startPosition;
    private float walkedDistance;
    private float maxDistance;

    private float separationRadius = 2f;
    private float forceSeparation = 1f;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        base.Enter();
        sheepStateMachine.Sheep.AnimSheep.SetBool("isWalking", true);
        //Debug.Log("La oveja ha entrado en el estado de CAMINAR");
        walkDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        startPosition = sheepStateMachine.Sheep.transform.position;
        walkedDistance = 0f;
        maxDistance = Random.Range(2f, 4f);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        MoveSheep();
    }

    public override void Exit()
    {
        base.Exit();
        sheepStateMachine.Sheep.AnimSheep.SetBool("isWalking", false);
        //Debug.Log("La oveja ha salido del estado de CAMINAR.");
    }
    #endregion

    #region Métodos Propios WalkState
    /// <summary>
    /// Método para desplazar a las ovejas cuando están en el estado de caminar.
    /// Toman una dirección aleatoria hacia la que caminar, teniendo el cuenta la separación que debe tener con las demás ovejas.
    /// Se desplazan la distancia máxima definida (entre 2 y 4 metros).
    /// Cuando se desplazan dicha distancia, pasan al estado de pastar.
    /// </summary>
    private void MoveSheep()
    {
        Vector3 currentPosition = sheepStateMachine.Sheep.transform.position;
        Vector3 separationVector = CalculateSeparation();

        Vector3 finalDirection = (walkDirection + separationVector).normalized;

        Vector3 nextPosition = currentPosition + finalDirection * walkSpeed * Time.fixedDeltaTime;
        sheepStateMachine.Sheep.RbSheep.MovePosition(nextPosition);

        if (finalDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
            sheepStateMachine.Sheep.transform.rotation = Quaternion.Slerp(sheepStateMachine.Sheep.transform.rotation, targetRotation, 5f * Time.fixedDeltaTime);
        }

        walkedDistance = Vector3.Distance(startPosition, nextPosition);

        if (walkedDistance >= maxDistance)
            sheepStateMachine.ChangeState(sheepStateMachine.SheepGrazeState);
    }

    /// <summary>
    /// Método que gestiona que las ovejas se alejen unas de otras si se acercan demasiado.
    /// </summary>
    /// <returns>Vector de dirección para alejarse de las demás ovejas.</returns>
    private Vector3 CalculateSeparation()
    {
        Vector3 separation = Vector3.zero;
        int neighborSheeps = 0;

        Collider[] nearbySheep = Physics.OverlapSphere(sheepStateMachine.Sheep.transform.position, separationRadius);

        foreach (Collider colliderSheep in nearbySheep)
        {
            if (colliderSheep.gameObject == sheepStateMachine.Sheep.gameObject) continue; // Ignora su propio collider.

            if (colliderSheep.TryGetComponent<Sheep>(out Sheep otherSheep)) // Comprueba si el collider pertenece a otra oveja.
            {
                Vector3 distance = sheepStateMachine.Sheep.transform.position - colliderSheep.transform.position;
                float distanceBetweenSheeps = distance.magnitude;

                if (distanceBetweenSheeps > 0)
                {
                    separation += distance.normalized / distanceBetweenSheeps;
                    neighborSheeps++;
                }
            }
        }

        if (neighborSheeps > 0)
        {
            separation /= neighborSheeps;
            separation *= forceSeparation;
        }

        return separation;
    }
    #endregion
}
