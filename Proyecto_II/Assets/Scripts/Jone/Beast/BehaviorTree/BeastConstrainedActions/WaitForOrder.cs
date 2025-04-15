using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class WaitForOrder : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private BeastBehaviorTree _beastBehaviorTree;
    private Transform _transform;
    private Transform _playerTransform;

    private float _freeDistance;
    private float _duration;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public WaitForOrder(Blackboard blackboard, BeastBehaviorTree beastBT, Transform transform, Transform playerTransform, float freeDistance, float duration)
    {
        _blackboard = blackboard;
        _beastBehaviorTree = beastBT;
        _transform = transform;
        _playerTransform = playerTransform;
        _freeDistance = freeDistance;
        _duration = duration;
    }

    public override NodeState Evaluate()
    {    
        float distance = Vector3.Distance(_transform.position, _playerTransform.position);

        // Si el jugador se aleja el nodo falla
        if (distance > _freeDistance)
        {
            state = NodeState.FAILURE;
            return state;
        }
        
        if (!_isRunning)
        {
            _isRunning = true;
            _hasFinished = false;
            _beastBehaviorTree.StartNewCoroutine(WaitingForOrder(_duration), this);
        }

        if (_hasFinished)
        {
            _isRunning = false;
            state = NodeState.SUCCESS;
        }
        else
            state = NodeState.RUNNING;


        return state;
    }

    private IEnumerator WaitingForOrder(float duration)
    {
        BeastBehaviorTree.beastWaitingOrder = true;
        BeastBehaviorTree.anim.SetBool("isWalking", false);
        BeastBehaviorTree.anim.SetBool("isSitting", true);
        float elapsedTime = 0f;
        Debug.Log($"Empieza cuenta atrás de {duration} segundos");
        while (elapsedTime < duration)
        {
            if (Input.GetKeyDown(KeyCode.Tab)) //TODO: sustituirlo por NEW INPUT SYSTEM
            {
                _blackboard.SetValue("menuOpened", true);
                yield break; // Termina la corrutina inmediatamente
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Tiempo de espera completado: Ejecutando función por timeout.");
        OnCoroutineEnd();       
    }

    public void OnCoroutineEnd()
    {
        if (!_hasFinished)
        {
            BeastBehaviorTree.anim.SetBool("isSitting", false);
            BeastBehaviorTree.isConstrained = false;
            BeastBehaviorTree.beastWaitingOrder = false;

            Debug.Log("Finished waiting");

            _hasFinished = true;
        }
    }
}
