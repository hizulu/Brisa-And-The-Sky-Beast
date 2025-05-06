using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 03/04/2025

// Clase que maneja por separado la animación de la palanca
// Separado de la lógica de Lever para seguir el principio SOLID de responsabilidad única
public class LeverAnimator
{
    private readonly Transform leverStick;
    private readonly float leverActiveXRotation;
    private readonly float leverNotActiveXRotation;
    private readonly float rotationSpeed;
    private Coroutine rotationCoroutine;
    private MonoBehaviour owner;

    public LeverAnimator(MonoBehaviour owner, Transform stick, float activeRotation, float inactiveRotation, float speed)
    {
        this.owner = owner;
        leverStick = stick;
        leverActiveXRotation = activeRotation;
        leverNotActiveXRotation = inactiveRotation;
        rotationSpeed = speed;
    }

    public void AnimateLever(bool moveToActive)
    {
        float targetAngle = moveToActive ? leverActiveXRotation : leverNotActiveXRotation;
        if (rotationCoroutine != null) owner.StopCoroutine(rotationCoroutine);
        rotationCoroutine = owner.StartCoroutine(RotateLever(targetAngle));
    }


    private IEnumerator RotateLever(float targetXRotation)
    {       
        Quaternion startRotation = leverStick.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetXRotation, 0f, 0f);

        float elapsedTime = 0f;
        float duration = 1f / rotationSpeed;

        while (elapsedTime < duration)
        {
            leverStick.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        leverStick.rotation = targetRotation;
    }
}
