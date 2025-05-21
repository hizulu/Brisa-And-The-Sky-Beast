using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Jone Sainz Egea
// 19/05/2025
public class ShadowController : MonoBehaviour
{
    [SerializeField] float castDistance = 10f;
    [SerializeField] LayerMask groundMask;
    DecalProjector projector;

    void Start()
    {
        projector = GetComponent<DecalProjector>();
    }

    void Update()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.2f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, castDistance, groundMask))
        {
            float distance = hit.distance;

            // Ajustar profundidad (solo hacia abajo)
            var size = projector.size;
            size.z = distance;
            projector.size = size;

            // Mover el volumen de proyección completamente hacia abajo
            projector.pivot = new Vector3(0, 0, -distance / 2f);

            // Atenuar la sombra
            projector.fadeFactor = 0.7f - (distance *2/ castDistance);
            //float t = distance / castDistance;
            //projector.fadeFactor = 1 - t * t;
        }
        else
        {
            projector.fadeFactor = 0f;
        }
    }
}
