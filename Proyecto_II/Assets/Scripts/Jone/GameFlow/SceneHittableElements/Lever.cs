using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : HittableElement
{
    [SerializeField] private LeverActionBase leverAction;
    [SerializeField] float leverActiveXRotation = -25f;
    [SerializeField] float leverNotActiveXRotation = -150f;
    [SerializeField] private float rotationSpeed = 5f; // Velocidad de la animaci�n

    private Transform leverStick;
    private bool isActivated = true;
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
            //isActivated = !isActivated;
        }
        else if (!isActivated)
        {
            leverAction.DoLeverAction();
            isActivated = true;
        }
    }

    public void DoLeverAnimation()
    {
        float targetAngle = isActivated ? leverNotActiveXRotation : leverActiveXRotation;

        // Detiene una posible animaci�n previa antes de iniciar una nueva
        if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
        rotationCoroutine = StartCoroutine(RotateLever(targetAngle));

        isActivated = !isActivated;
    }

    private IEnumerator RotateLever(float targetXRotation)
    {
        Quaternion startRotation = leverStick.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetXRotation, leverStick.rotation.eulerAngles.y, leverStick.rotation.eulerAngles.z);

        float timeElapsed = 0f;
        while (timeElapsed < 1f)
        {
            leverStick.rotation = Quaternion.Lerp(startRotation, targetRotation, timeElapsed);
            timeElapsed += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        leverStick.rotation = targetRotation;
    }
}
