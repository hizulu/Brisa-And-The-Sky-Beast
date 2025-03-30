using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

// Jone Sainz Egea
// 30/03/2025
public class DrawbridgeMover : MonoBehaviour
{
    [SerializeField] private Transform oneActiveLever;
    [SerializeField] private Transform twoActiveLever;
    [SerializeField] private float movementDuration = 3f;

    private NavMeshModifierVolume navMeshModifier;
    private Coroutine moveCoroutine;

    private bool leverOneActive;
    private bool leverTwoActive;
    private bool isMoving = false;
    private void Awake()
    {
        navMeshModifier = GetComponent<NavMeshModifierVolume>();
        if (navMeshModifier == null)
        {
            Debug.LogWarning("The drawbridge doesn't have a NavMeshModifierVolume assigned.");
        }
    }
    public void SetLeverOneToActive() => leverOneActive = true;
    public void SetLeverTwoToActive() => leverTwoActive = true;

    // Comprobación de si solo hay una palanca activa o dos
    // Dependiendo de ello rota el puente levadizo a una posición u otra
    public void CheckDrawbridgeState()
    {
        Transform target = (leverOneActive && leverTwoActive) ? twoActiveLever : oneActiveLever;

        // Si el puente aún se está moviendo, debe esperar a que termine la animación anterior
        moveCoroutine = StartCoroutine(WaitAndMove(target));
    }

    private IEnumerator WaitAndMove(Transform target)
    {
        while (isMoving) // Espera a que termine la animación anterior
        {
            yield return null;
        }

        yield return StartCoroutine(MoveDrawbridge(target));
    }

    private IEnumerator MoveDrawbridge(Transform target)
    {
        isMoving = true;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = target.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < movementDuration)
        {
            float t = elapsedTime / movementDuration; // Normaliza el tiempo para interpolación correcta
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        isMoving = false;

        // Gestión del NavMesh
        if (target == twoActiveLever)
        {
            navMeshModifier.enabled = false;
            Debug.Log("NavMeshModifier should not be activated");
        }
    }
}
