using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Jone Sainz Egea
// Clase abstracta de la que heredan las acciones específicas de las palancas
public abstract class LeverActionBase : ScriptableObject
{
    [SerializeField] private bool isReversible; // Definir en el inspector si la acción es reversible

    public bool IsActionReversible()
    {
        return isReversible;
    }

    public abstract void DoLeverAction(); // Acción principal

    public virtual void UndoLeverAction()
    {
        Debug.LogWarning("UndoLeverAction() no está implementado en esta acción.");
    }
}
