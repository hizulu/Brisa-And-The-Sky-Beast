using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 16/05/2025
// Nodo que comprueba la distancia a la que se encuentra el jugador
// Si está demasiado lejos devuelve éxito, si no, fracaso
public class CheckPlayerTooFar : Node
{
    private Beast _beast;
    private Transform _playerTransform;
    private float _distanceTooFarAway;
    private Node _child;
    private bool _expectedValue;

    public CheckPlayerTooFar(Beast beast, Transform playerTransform, float distanceTooFarAway, Node child, bool expectedValue = true)
    {
        _beast = beast;
        _playerTransform = playerTransform;
        _distanceTooFarAway = distanceTooFarAway;
        _child = child;
        _expectedValue = expectedValue;
    }

    public override NodeState Evaluate()
    {
        if(IsTooFarAway() == _expectedValue)
            return _child.Evaluate();
        return NodeState.FAILURE;
    }

    private bool IsTooFarAway()
    {
        return (_beast.transform.position - _playerTransform.position).sqrMagnitude > (_distanceTooFarAway * _distanceTooFarAway);
    }
}
