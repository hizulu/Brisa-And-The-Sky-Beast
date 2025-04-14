using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CageAction", menuName = "Lever Actions/Cage Action")]
public class LeverActionsCage : LeverActionBase
{
    private GameObject cage;
    private float movementSpeed = 2f;
    private Vector3 targetPosition;
    private BeastTrapped beast;

    private bool cageIsUp = false;

    private void EnsureCageIsAssigned()
    {
        if (cage == null)
        {
            cage = GameObject.FindGameObjectWithTag("Cage");
        }
    }
    private void EnsureBeastIsAssigned()
    {
        if (beast == null)
        {
            beast = GameObject.FindGameObjectWithTag("Beast").GetComponent<BeastTrapped>();
        }
    }

    public override void DoLeverAction()
    {
        if (!cageIsUp)
        {
            EnsureCageIsAssigned();
            if (cage == null) return;

            // Para no recalcular objetivo si se está moviendo
            if (cage.TryGetComponent(out CageMover mover) && mover.IsMoving())
            {
                Debug.Log("La jaula aún se está moviendo. No se recalcula la posición.");
                return;
            }

            Debug.Log("La jaula sube.");
            targetPosition = new Vector3(cage.transform.position.x, cage.transform.position.y + 5f, cage.transform.position.z);
            MoveCage();
        }
    }

    public override void UndoLeverAction()
    {
        if (cageIsUp)
        {
            EnsureCageIsAssigned();
            if (cage == null) return;

            // Para no recalcular objetivo si se está moviendo
            if (cage.TryGetComponent(out CageMover mover) && mover.IsMoving())
            {
                Debug.Log("La jaula aún se está moviendo. No se recalcula la posición.");
                return;
            }

            Debug.Log("La jaula baja.");
            targetPosition = new Vector3(cage.transform.position.x, cage.transform.position.y - 5f, cage.transform.position.z);
            MoveCage();
        }
    }

    private void MoveCage()
    {
        if (cage.TryGetComponent(out CageMover mover))
        {
            mover.StartMoving(targetPosition, movementSpeed);
            EnsureBeastIsAssigned();
            beast.SetBeastFreeFromCage();
        }
        else
        {
            Debug.LogWarning("El objeto jaula no tiene el script CageMover.");
        }
        cageIsUp = !cageIsUp;
    }
}

