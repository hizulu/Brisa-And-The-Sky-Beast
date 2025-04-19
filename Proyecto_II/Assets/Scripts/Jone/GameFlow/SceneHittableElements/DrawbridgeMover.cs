using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

// Jone Sainz Egea
// 30/03/2025
    // 19/04/2025 Nav Mesh Update fixed
public class DrawbridgeMover : MonoBehaviour, IMovableElement
{
    [SerializeField] private Transform oneActiveLever;
    [SerializeField] private Transform twoActiveLever;
    [SerializeField] private float movementDuration = 3f;
    [SerializeField] NavMeshSurface beastNavMeshSurface;

    private NavMeshModifierVolume navMeshModifier;
    private NavMeshData navMeshData;
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

        StartCoroutine(TemporarilyUpdateNavMesh());
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
            UpdateNavMesh();
        }
    }

    public void StartMoving(Vector3 target, float speed) { } // No usado aquí
    public bool IsMoving() => isMoving;

    private void UpdateNavMesh()
    {
        StartCoroutine(TemporarilyUpdateNavMesh());
    }

    private IEnumerator TemporarilyUpdateNavMesh()
    {
        // Obtener todos los renderers con los tags correspondientes
        MeshRenderer[] toAdd = GameObject.FindGameObjectsWithTag("AddForNavMesh")
            .SelectMany(go => go.GetComponentsInChildren<MeshRenderer>(true)).ToArray();

        MeshRenderer[] toRemove = GameObject.FindGameObjectsWithTag("RemoveForNavMesh")
            .SelectMany(go => go.GetComponentsInChildren<MeshRenderer>(true)).ToArray();

        // Guardar estado original
        Dictionary<MeshRenderer, bool> originalStates = new Dictionary<MeshRenderer, bool>();
        foreach (var rend in toAdd)
        {
            originalStates[rend] = rend.enabled;
            rend.enabled = true; // Asegurar que estén activos para ser considerados
        }
        foreach (var rend in toRemove)
        {
            originalStates[rend] = rend.enabled;
            rend.enabled = false; // Asegurar que no se incluyan en el bake
        }

        // Desactivar temporalmente el Modifier para que no interfiera
        navMeshModifier.enabled = false;

        // Espera 1 frame para asegurarse de que Unity registre los cambios
        yield return null;

        // Actualizar el NavMesh
        navMeshData = beastNavMeshSurface.navMeshData;
        beastNavMeshSurface.UpdateNavMesh(navMeshData);
        Debug.Log("NavMesh updated");

        // Restaurar el estado original
        foreach (var kvp in originalStates)
        {
            kvp.Key.enabled = kvp.Value;
        }
    }
}
