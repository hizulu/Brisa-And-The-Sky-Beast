using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItemAnimation : MonoBehaviour
{
    [Range(0.1f, 2f)][SerializeField] private float movementRange = 1f;
    [Range(0.5f, 5f)][SerializeField] private float movementVelocity = 1f;
    [Range(0.1f, 360f)][SerializeField] private float rotationVelocity = 30f;

    private Vector3 initialPosition;
    private float timeOffset;

    void Start()
    {
        initialPosition = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2f); // Desfase aleatorio para evitar sincronización exacta
    }

    void LateUpdate()
    {
        float yOffset = Mathf.Sin(Time.time * movementVelocity + timeOffset) * movementRange;
        transform.position = initialPosition + new Vector3(0, yOffset, 0);

        float rotationAngle = Time.time * rotationVelocity;
        transform.rotation = Quaternion.Euler(0f, (float)rotationAngle, 0f);
    }
}
