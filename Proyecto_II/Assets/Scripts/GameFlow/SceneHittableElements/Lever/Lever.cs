using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 27/03/2025
// Script que gestiona las palancas, recibe la acción desde el inspector
public class Lever : HittableElement
{
    [SerializeField] private LeverActionBase leverAction;
    [SerializeField] private Transform leverStick;
    [SerializeField] private float leverActiveXRotation = -40f;
    [SerializeField] private float leverNotActiveXRotation = -140f;
    [SerializeField] private float rotationSpeed = 5f; // Velocidad de la animación

    private LeverAnimator animator;
    private bool isActivated = false;
    private bool moveToActive = true;

    private void Start()
    {
        animator = new LeverAnimator(this, leverStick, leverActiveXRotation, leverNotActiveXRotation, rotationSpeed);
        isActivated = false;
    }

    public override void OnHit()
    {
        SoundObjectsManager.Instance.PlaySFX(SoundType.Lever);

        animator.AnimateLever(moveToActive);

        if (leverAction != null)
        {
            if (leverAction.IsActionReversible())
            {
                if (isActivated)
                    leverAction.UndoLeverAction();
                else
                    leverAction.DoLeverAction();
                isActivated = !isActivated;  // Solo cambia si es reversible
            }
            else if (!isActivated) // Para acciones no reversibles
            {
                leverAction.DoLeverAction();
                isActivated = true;  // No cambiará de nuevo después del primer golpe
            }
        }

        moveToActive = !moveToActive; // La animación siempre alterna
    }
}
