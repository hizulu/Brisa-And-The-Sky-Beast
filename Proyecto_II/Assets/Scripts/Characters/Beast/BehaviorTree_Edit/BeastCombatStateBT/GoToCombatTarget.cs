using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

// Jone Sainz Egea
// 28/04/2025
public class GoToCombatTarget : Node
{
    private Blackboard _blackboard;
    private Beast _beast;
    private float _arrivalThreshold;

    private GameObject _target;
    private bool _wasWalking = false;

    public GoToCombatTarget(Blackboard blackboard, Beast beast, float arrivalThreshold)
    {
        _blackboard = blackboard;
        _beast = beast;
        _arrivalThreshold = arrivalThreshold;
    }

    public override NodeState Evaluate()
    {
        if (!_blackboard.HasKey("targetForCombat"))
        {
            state = NodeState.FAILURE;
            return state;
        }

        _target = _blackboard.GetValue<GameObject>("targetForCombat");

        if(_target == null)
        {
            Debug.LogWarning("Ha muerto slime mientrás iba a por él");
            _blackboard.ClearKey("targetForCombat");
            state = NodeState.FAILURE;
            return state;
        }

        float distance = Vector3.Distance(_beast.transform.position, _target.transform.position);

        if (distance < _arrivalThreshold + 1)
        {
            if (_wasWalking)
            {
                _beast.anim.SetBool("isWalking", false);
                _blackboard.SetValue("reachedCombatTarget", true);
                _beast.agent.ResetPath();

                _wasWalking = false;
            }
            Debug.Log("Reached combat target");
            state = NodeState.SUCCESS;
            return state;
        }

        if (!_wasWalking)
        {
            _beast.anim.SetBool("isWalking", true);
            _wasWalking = true;
        }

        if (_beast.agent.destination != _target.transform.position)
            _beast.agent.SetDestination(_target.transform.position);

        // Verificar si el destino es alcanzable
        if (_beast.agent.pathStatus == NavMeshPathStatus.PathInvalid || _beast.agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.LogWarning("Path to enemy is invalid or partial.");
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
