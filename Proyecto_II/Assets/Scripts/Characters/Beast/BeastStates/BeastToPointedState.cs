using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Jone Sainz Egea
// 27/04/2025
// Estado en el que la Bestia se dirige al punto indicado por el jugador, cuando llega vuelve al estado de libertad
public class BeastToPointedState : BeastState
{
    private Vector3 _target;
    private bool _wasWalking = false;
    public BeastToPointedState(Vector3 target)
    {
        _target = target;
        _wasWalking = false;
    }
    public override void OnEnter(Beast beast)
    {
        beast.agent.ResetPath();
        Debug.Log("Beast is being directed");
    }
    public override void OnUpdate(Beast beast)
    {
        float distance = Vector3.Distance(beast.transform.position, _target);

        if (distance < 0.5f)
        {
            if (_wasWalking)
            {
                beast.anim.SetBool("isWalking", false);
                // TODO: Cambiaría a wait for order
                beast.agent.ResetPath();

                Debug.Log("Reached target.");
                _wasWalking = false;
            }
            beast.TransitionToState(new BeastFreeState());
            return;
        }

        if (!_wasWalking)
        {
            beast.anim.SetBool("isWalking", true);
            _wasWalking = true;
        }

        if (beast.agent.destination != _target) //&& !_beast.beastWaitingOrder)
            beast.agent.SetDestination(_target);

        // Verificar si el destino es alcanzable
        if (beast.agent.pathStatus == NavMeshPathStatus.PathInvalid || beast.agent.pathStatus == NavMeshPathStatus.PathPartial)
            beast.TransitionToState(new BeastFreeState());
    }
    public override void OnExit(Beast beast)
    {
        beast.agent.ResetPath();
        Debug.Log("Beast stops being directed");
    }
}
