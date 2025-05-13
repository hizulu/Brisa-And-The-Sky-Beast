using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Jone Sainz Egea
// 14/04/2025
// Nodo que recibe una probabilidad y aleatoriamente en base a esa probabilidad devuelve éxito o fracaso
// Hace que el flujo de acciones de la bestia sea menos predecible
public class CheckRandomChance : Node
{
    private float _probability;

    public CheckRandomChance(float probabilityPercent)
    {
        _probability = Mathf.Clamp01(probabilityPercent / 100f); // Convertir de porcentaje a valor entre 0 y 1
    }

    public override NodeState Evaluate()
    {
        float roll = Random.value; // Valor aleatorio entre 0 y 1

        if (roll <= _probability)
            state = NodeState.SUCCESS;
        else
            state = NodeState.FAILURE;

        return state;
    }
}
