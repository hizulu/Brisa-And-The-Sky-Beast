using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Jone Sainz Egea
// Clase abstracta de la que heredan las acciones espec�ficas de las palancas
public abstract class LeverActionBase : ScriptableObject
{
    [SerializeField] private bool isReversible; // Definir en el inspector si la acci�n es reversible

    public bool IsActionReversible()
    {
        return isReversible;
    }

    public abstract void DoLeverAction(); // Acci�n principal

    public virtual void UndoLeverAction()
    {
        Debug.LogWarning("UndoLeverAction() no est� implementado en esta acci�n.");
    }
}
