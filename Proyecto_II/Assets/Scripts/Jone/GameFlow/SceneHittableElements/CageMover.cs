using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageMover : MonoBehaviour
{
    private Vector3 targetPosition;
    private float movementSpeed;
    private bool isMoving = false;

    public void StartMoving(Vector3 newTarget, float speed)
    {
        targetPosition = newTarget;
        movementSpeed = speed;
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition; // Asegurar posición final exacta
            isMoving = false;
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
