using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : HittableElement
{
    [SerializeField] private LeverActionBase leverAction;
    [SerializeField] float leverActiveXRotation = -40f;
    [SerializeField] float leverNotActiveXRotation = -140f;
    [SerializeField] private float rotationSpeed = 5f; // Velocidad de la animación

    private Transform leverStick;
    private bool isActivated = false;
    private bool moveToActive = true;
    private Coroutine rotationCoroutine;

    private void Start()
    {
        leverStick = transform.Find("Palo");
    }

    public override void OnHit()
    {
        DoLeverAnimation();

        if (leverAction == null) return;

        Debug.Log($"Action is reversible is: {leverAction.IsActionReversible()}");
        if (leverAction.IsActionReversible())
        {
            if (isActivated)
            {
                Debug.Log("It's going to undo action");
                leverAction.UndoLeverAction();
            }
            else
            {
                Debug.Log("It's going to do action");
                leverAction.DoLeverAction();
            }
            isActivated = !isActivated;
        }
        else if (!isActivated)
        {
            leverAction.DoLeverAction();
            isActivated = true;
        }
    }

    public void DoLeverAnimation()
    {
        float targetAngle = moveToActive ? leverActiveXRotation : leverNotActiveXRotation;

        // Detiene una posible animación previa antes de iniciar una nueva
        if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
        rotationCoroutine = StartCoroutine(RotateLever(targetAngle));

        moveToActive = !moveToActive;
    }

    private IEnumerator RotateLever(float targetXRotation)
    {
        Quaternion startRotation = leverStick.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetXRotation, leverStick.rotation.eulerAngles.y, leverStick.rotation.eulerAngles.z);

        float elapsedTime = 0f;
        float duration = 1f / rotationSpeed; // Ajusta la duración según la velocidad

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            leverStick.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        leverStick.rotation = targetRotation;
    }
}
