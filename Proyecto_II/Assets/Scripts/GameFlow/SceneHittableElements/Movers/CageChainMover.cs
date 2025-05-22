using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageChainMover : MonoBehaviour, IMovableElement
{
    private Vector3 targetScale;
    private float movementSpeed;
    private bool isMoving = false;

    public void StartMoving(Vector3 newScale, float speed)
    {
        targetScale = newScale;
        movementSpeed = speed;
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;


        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
        {
            transform.localScale = targetScale; // Asegurar posición final exacta
            isMoving = false;
        }
    }

    public bool IsMoving() => isMoving;
}
