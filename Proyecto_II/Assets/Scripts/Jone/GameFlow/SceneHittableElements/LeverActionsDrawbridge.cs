using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 30/03/2025
[CreateAssetMenu(fileName = "DrawbridgeAction", menuName = "Lever Actions/Drawbridge Action")]
public class LeverActionsDrawbridge : LeverActionBase
{
    [SerializeField] private bool isLeverOne; // Porque necesita que se activen dos palancas diferentes
    private DrawbridgeMover drawbridgeMover;    

    public override void DoLeverAction()
    {
        // Obtención de la clase DrawbridgeMover
        if (drawbridgeMover == null)
        {
            drawbridgeMover = GameObject.FindObjectOfType<DrawbridgeMover>();
            if (drawbridgeMover == null)
            {
                Debug.LogWarning("No se encontró DrawbridgeMover en la escena.");
                return;
            }
        }

        // La activación se gestiona desde DrawbridgeMover para que guarde la información del estado de ambas palancas
        if (isLeverOne)
        {
            drawbridgeMover.SetLeverOneToActive();
        }
        else
        {
            drawbridgeMover.SetLeverTwoToActive();
        }

        drawbridgeMover.CheckDrawbridgeState();
    }   
}
