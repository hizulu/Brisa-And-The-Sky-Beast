using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

// Jone Sainz Egea
// 30/03/2025
    // 19/04/2025 Nav Mesh Update fixed
        //26/40/2025 chains added
public class DrawbridgeMover : MonoBehaviour, IMovableElement
{
    [SerializeField] private Transform oneActiveLeverDrawbridge;
    [SerializeField] private Transform twoActiveLeverDrawbridge;
    [SerializeField] private Transform chainTransform1;
    [SerializeField] private Transform oneActiveLeverChain1;
    [SerializeField] private Transform twoActiveLeverChain1;
    [SerializeField] private Transform chainTransform2;
    [SerializeField] private Transform oneActiveLeverChain2;
    [SerializeField] private Transform twoActiveLeverChain2;
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
        Transform targetForDrawbridge = (leverOneActive && leverTwoActive) ? twoActiveLeverDrawbridge : oneActiveLeverDrawbridge;
        Transform targetForChain1 = (leverOneActive && leverTwoActive) ? twoActiveLeverChain1 : oneActiveLeverChain1;
        Transform targetForChain2 = (leverOneActive && leverTwoActive) ? twoActiveLeverChain2 : oneActiveLeverChain2;

        // Si el puente aún se está moviendo, debe esperar a que termine la animación anterior
        moveCoroutine = StartCoroutine(WaitAndMove(targetForDrawbridge, targetForChain1, targetForChain2));
    }

    private IEnumerator WaitAndMove(Transform target, Transform targetForChain1, Transform targetForChain2)
    {
        while (isMoving) // Espera a que termine la animación anterior
        {
            yield return null;
        }

        yield return StartCoroutine(MoveDrawbridge(target, targetForChain1, targetForChain2));
    }

    private IEnumerator MoveDrawbridge(Transform target, Transform targetForChain1, Transform targetForChain2)
    {
        isMoving = true;
        Quaternion startRotationDrawbridge = transform.rotation;
        Quaternion targetRotationDrawbridge = target.rotation;

        Quaternion startRotationChain1 = chainTransform1.rotation;
        Quaternion targetRotationChain1 = targetForChain1.rotation;
        Vector3 startScaleChain1 = chainTransform1.localScale;
        Vector3 targetScaleChain1 = new Vector3 (chainTransform1.localScale.x, chainTransform1.localScale.y, chainTransform1.localScale.z + targetForChain1.localScale.z);

        Quaternion startRotationChain2 = chainTransform2.rotation;
        Quaternion targetRotationChain2 = targetForChain2.rotation;
        Vector3 startScaleChain2 = chainTransform2.localScale;
        Vector3 targetScaleChain2 = new Vector3(chainTransform2.localScale.x, chainTransform2.localScale.y, chainTransform2.localScale.z + targetForChain2.localScale.z);

        float elapsedTime = 0f;

        while (elapsedTime < movementDuration)
        {
            float t = elapsedTime / movementDuration; // Normaliza el tiempo para interpolación correcta
            transform.rotation = Quaternion.Slerp(startRotationDrawbridge, targetRotationDrawbridge, t);

            chainTransform1.rotation = Quaternion.Slerp(startRotationChain1, targetRotationChain1, t);
            chainTransform1.localScale = Vector3.Lerp(startScaleChain1, targetScaleChain1, t);

            chainTransform2.rotation = Quaternion.Slerp(startRotationChain2, targetRotationChain2, t);
            chainTransform2.localScale = Vector3.Lerp(startScaleChain2, targetScaleChain2, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotationDrawbridge;

        chainTransform1.rotation = targetRotationChain1;
        chainTransform1.localScale = targetScaleChain1;

        chainTransform2.rotation = targetRotationChain2;
        chainTransform2.localScale = targetScaleChain2;
        isMoving = false;

        // Gestión del NavMesh
        if (target == twoActiveLeverDrawbridge)
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
