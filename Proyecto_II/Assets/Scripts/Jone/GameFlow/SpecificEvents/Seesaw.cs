using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 2f;

    [SerializeField] private float maxRotation = 20f;

    private int leftWeight = 0;
    private int rightWeight = 0;

    private Coroutine rotationCoroutine;

    private float GetTargetRotation()
    {
        if (leftWeight > 0 && rightWeight == 0)
            return maxRotation;
        else if (rightWeight > 0 && leftWeight == 0)
            return -maxRotation;
        else
            return 0f;
    }

    private IEnumerator RotateToTarget(float targetRotation)
    {
        while (true)
        {
            float current = transform.localRotation.eulerAngles.z;
            if (current > 180f) current -= 360f; // Asegura que esté en el rango -180 a 180

            float newRotation = Mathf.Lerp(current, targetRotation, Time.deltaTime * rotationSpeed);
            transform.localRotation = Quaternion.Euler(0f, 0f, newRotation);

            if (Mathf.Abs(newRotation - targetRotation) < 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0f, 0f, targetRotation);
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

    public void AddWeight(bool isLeft)
    {
        if (isLeft)
            leftWeight++;
        else
            rightWeight++;

        StartRotation();
    }

    public void RemoveWeight(bool isLeft)
    {
        if (isLeft)
            leftWeight = Mathf.Max(0, leftWeight - 1);
        else
            rightWeight = Mathf.Max(0, rightWeight - 1);

        StartRotation();
    }
}
