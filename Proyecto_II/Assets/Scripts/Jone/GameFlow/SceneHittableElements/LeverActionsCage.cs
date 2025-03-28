using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CageAction", menuName = "Lever Actions/Cage Action")]
public class LeverActionsCage : LeverActionBase
{
    private GameObject cage; // Asignar la jaula en tiempo de ejecución

    public override void DoLeverAction()
    {
        Debug.Log("La jaula se abre.");
        if (cage != null) cage.SetActive(false);
    }

    public override void UndoLeverAction()
    {
        Debug.Log("La jaula se cierra.");
        if (cage != null) cage.SetActive(true);
    }
}
