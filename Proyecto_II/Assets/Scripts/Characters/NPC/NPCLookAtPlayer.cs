using UnityEngine;

/* NOMBRE CLASE: NPCLookAtPlayer
 * AUTOR: Lucía García López
 * FECHA: 20/05/2025
 * DESCRIPCIÓN: Script que gestiona el movimiento de un NPC para que mire al jugador cuando entra en su rango de diálogo.
 * VERSIÓN: 1.0
 */

[RequireComponent(typeof(NPCDialogRange))]
public class NPCLookAtPlayer : MonoBehaviour
{
    [Header("Rotation Settings")]
    private float rotationSpeed = 1f;
    private float returnSpeed = 1f;
    private bool rotateOnlyYAxis = true;

    private Transform playerTransform;
    private Quaternion initialRotation;
    private bool shouldLookAtPlayer = false;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (shouldLookAtPlayer && playerTransform != null)
        {
            RotateTowardsPlayer();
        }
        else
        {
            ReturnToInitialRotation();
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = playerTransform.position - transform.position;
        if (rotateOnlyYAxis) direction.y = 0; // Ignorar diferencia en altura

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void ReturnToInitialRotation()
    {
        //El slerp hace que la rotación vuelva a la inicial de forma suave
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            initialRotation,
            returnSpeed * Time.deltaTime
        );
    }

    // Llamado desde NPCDialogRange cuando el jugador entra/sale del rango
    public void SetLookAtTarget(Transform target, bool shouldLook)
    {
        playerTransform = target;
        shouldLookAtPlayer = shouldLook;
    }
}