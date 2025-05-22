using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class Seesaw : MonoBehaviour
{
    [SerializeField] private NavMeshSurface seesawSurface;

    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float maxRotation = 20f;

    private int leftWeight = 10;
    private int rightWeight = 10;

    private Coroutine rotationCoroutine;

    private void Awake()
    {
        seesawSurface.navMeshData = new NavMeshData();
        NavMesh.AddNavMeshData(seesawSurface.navMeshData, transform.position, transform.rotation);
        seesawSurface.BuildNavMesh(); // Bake inicial
    }

    private float GetTargetRotation()
    {
        if (leftWeight > rightWeight)
            return maxRotation;
        else if (rightWeight > leftWeight)
            return -maxRotation;
        else
            return 0f;
    }

    private IEnumerator RotateToTarget(float targetRotation)
    {
        while (true)
        {
            SoundObjectsManager.Instance.PlaySFX(SoundType.Seesaw, 0.2f);

            float current = transform.localRotation.eulerAngles.z;
            if (current > 180f) current -= 360f; // Asegura que esté en el rango -180 a 180

            float newRotation = Mathf.Lerp(current, targetRotation, Time.deltaTime * rotationSpeed);
            transform.localRotation = Quaternion.Euler(0f, 0f, newRotation);

            if (Mathf.Abs(newRotation - targetRotation) < 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0f, 0f, targetRotation);
                seesawSurface.UpdateNavMesh(seesawSurface.navMeshData);
                SoundObjectsManager.Instance.StopSFX(SoundType.Seesaw);
                yield break;
            }

            yield return null;
        }
    }

    private void StartRotation()
    {
        float target = GetTargetRotation();

        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(RotateToTarget(target));
    }

    public void AddWeight(bool isLeft, int addWeight)
    {
        if (isLeft)
            leftWeight += addWeight;
        else
            rightWeight += addWeight;

        StartRotation();
    }

    public void RemoveWeight(bool isLeft, int removeWeight)
    {
        if (isLeft)
            leftWeight -= removeWeight;
        else
            rightWeight -= removeWeight;

        StartRotation();
    }
}
