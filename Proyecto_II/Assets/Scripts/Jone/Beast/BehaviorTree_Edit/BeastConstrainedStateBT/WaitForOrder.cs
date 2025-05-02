using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

// Jone Sainz Egea
// 15/04/2025
// Nodo que espera a que el jugador abra el menú o sale de este "estado"
public class WaitForOrder : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;
    private Transform _transform;
    private Transform _playerTransform;

    private float _freeDistance;
    private float _duration;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public WaitForOrder(Blackboard blackboard, Beast beast, Transform playerTransform, float freeDistance, float duration)
    {
        _blackboard = blackboard;
        _beast = beast;
        _transform = _beast.transform;
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

        // Iniciar estado de espera al llegar
        if (!_isRunning)
        {
            _isRunning = true;
            _hasFinished = false;
            _beast.StartNewCoroutine(WaitingForOrder(_duration), this);
        }

        // Cuando termina el nodo
        if (_hasFinished)
        {
            Debug.Log("Ya ha terminado de esperar al menú");
            _isRunning = false;
            state = NodeState.SUCCESS;
        }
        else
            state = NodeState.RUNNING;


        return state;
    }

    private IEnumerator WaitingForOrder(float duration)
    {
        _beast.anim.SetBool("isWalking", false);
        _beast.anim.SetBool("isSitting", true);
        Debug.Log("Activo sentrarse");
        float elapsedTime = 0f;
        Debug.Log($"Empieza cuenta atrás de {duration} segundos");

        while (elapsedTime < duration)
        {
            if (Input.GetKeyDown(KeyCode.Tab)) //TODO: sustituirlo por NEW INPUT SYSTEM
            {
                _blackboard.SetValue("menuOpened", true);
                OnCoroutineEnd();
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
        if (_hasFinished)
            return;

        _beast.anim.SetBool("isSitting", false);
        Debug.Log("Desactivo sitting");

        _blackboard.SetValue("isCoroutineActive", false);

        _blackboard.SetValue("reachedPlayer", false);

        Debug.Log("Finished waiting and cleaned up flags");

        _hasFinished = true;
    }
}
