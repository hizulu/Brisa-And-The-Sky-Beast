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

        // Si el menú se ha cerrado sin seleccionar
        //if (BeastBehaviorTree.beastMenuClosed)
        //{
        //    BeastBehaviorTree.beastMenuClosed = false;
        //    OnCoroutineEnd();
        //    state = NodeState.SUCCESS;
        //    return state;
        //}

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
            _isRunning = false;
            state = NodeState.SUCCESS;
        }
        else
            state = NodeState.RUNNING;


        return state;
    }

    private IEnumerator WaitingForOrder(float duration)
    {
        //_beast.beastWaitingOrder = true;
        _beast.anim.SetBool("isWalking", false);
        _beast.anim.SetBool("isSitting", true);

        float elapsedTime = 0f;
        Debug.Log($"Empieza cuenta atrás de {duration} segundos");

        while (elapsedTime < duration)
        {
            if (Input.GetKeyDown(KeyCode.Tab)) //TODO: sustituirlo por NEW INPUT SYSTEM
            {
                _blackboard.SetValue("menuOpened", true);
                //BeastBehaviorTree.OpenBeastMenu(); // Simula abrir menú
                yield break; // Termina la corrutina inmediatamente
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Tiempo de espera completado: Ejecutando función por timeout.");

        // Si no se ha abierto el menú, probabilidad de quedarse sentado unos segundos más
        //if (!BeastBehaviorTree.beastMenuOpened)
        //{
        //    float chance = Random.Range(0f, 100f);
        //    if (chance <= 40f) // 40% de que se quede sentado
        //    {
        //        Debug.Log("Se queda sentado unos segundos más tras no recibir orden");
        //        yield return new WaitForSeconds(3f);
        //    }
        //}

        OnCoroutineEnd();       
    }

    public void OnCoroutineEnd()
    {
        if (!_hasFinished)
        {
            _beast.anim.SetBool("isSitting", false);

            //BeastBehaviorTree.isConstrained = false;
            //BeastBehaviorTree.beastWaitingOrder = false;
            //BeastBehaviorTree.beastMenuOpened = false;
            //BeastBehaviorTree.beastMenuClosed = false;

            _blackboard.SetValue("reachedPlayer", false);

            _blackboard.SetValue("lookForTarget", true);

            Debug.Log("Finished waiting and cleaned up flags");

            _hasFinished = true;
        }
    }
}
