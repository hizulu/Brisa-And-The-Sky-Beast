using BehaviorTree;
using UnityEngine.AI;
using UnityEngine;
using System.Collections;

// Jone Sainz Egea
// 06/04/2025
// Nodo que realiza la acción de olfatear por un tiempo aleatorio
public class Smell : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;
    private float _minDuration;
    private float _maxDuration;

    private bool _isRunning = false;
    private bool _hasFinished = false;


    public Smell(Blackboard blackboard, Beast beast, float minDuration, float maxDuration)
    {
        _blackboard = blackboard;
        _beast = beast;
        _minDuration = minDuration;
        _maxDuration = maxDuration;
    }

    public override NodeState Evaluate()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            _hasFinished = false;

            _beast.anim.SetBool("isWalking", false);
            _beast.anim.SetTrigger("smellDown");

            _beast.agent.ResetPath();

            float duration = Random.Range(_minDuration, _maxDuration);
            Debug.Log("Starting to smell");
            _beast.StartNewCoroutine(Smelling(duration), this);
        }

        if (_hasFinished)
        {
            _isRunning = false;
            state = NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.RUNNING;
        }

        return state;
    }   

    private IEnumerator Smelling(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        OnCoroutineEnd();
    }

    public void OnCoroutineEnd()
    {
        if (_hasFinished) return;

        _blackboard.SetValue("isCoroutineActive", false);

        Debug.Log("Finished smelling");

        _blackboard.SetValue("reachedTarget", false); 

        _hasFinished = true;
    }
}
