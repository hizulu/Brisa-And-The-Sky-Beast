using BehaviorTree;
using UnityEngine.AI;
using UnityEngine;
using System.Collections;

// Jone Sainz Egea
// 06/04/2025
// Nodo que realiza la acción de olfatear por un tiempo aleatorio
public class Smell : Node, ICoroutineNode
{
    private BeastBehaviorTree _beastBehaviorTree;
    private NavMeshAgent _agent;
    private Blackboard _blackboard;
    private float _minDuration;
    private float _maxDuration;

    private bool _isRunning = false;
    private bool _hasFinished = false;


    public Smell(Blackboard blackboard, BeastBehaviorTree beastBehaviorTree, NavMeshAgent agent, float minDuration, float maxDuration)
    {
        _blackboard = blackboard;
        _minDuration = minDuration;
        _maxDuration = maxDuration;
        _beastBehaviorTree = beastBehaviorTree;
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        PointOfInterest target = _blackboard.GetValue<PointOfInterest>("target");

        if (target == null)
        {
            Debug.Log("No se ha encontrado target, saliendo de Smell...");
            _isRunning = false;
            state = NodeState.FAILURE;
            return state;
        }

        bool hasArrived = _blackboard.GetValue<bool>("hasArrived");

        if (!hasArrived)
        {
            _isRunning = false;
            state = NodeState.RUNNING;
            return state;
        }

        if (!_isRunning)
        {
            _isRunning = true;
            _hasFinished = false;
            _beastBehaviorTree.StartNewCoroutine(Smelling(Random.Range(_minDuration, _maxDuration)), this);
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
        _agent.ResetPath();
        BeastBehaviorTree.anim.SetBool("isWalking", false);
        BeastBehaviorTree.anim.SetBool("isSmelling", true);

        Debug.Log("Iniciando animación de oler");
        yield return new WaitForSeconds(duration);

        OnCoroutineEnd();
    }

    public void OnCoroutineEnd()
    {
        if (!_hasFinished)
        {
            BeastBehaviorTree.anim.SetBool("isSmelling", false);

            Debug.Log("Finished smelling");

            _blackboard.ClearKey("target");
            _blackboard.SetValue("hasArrived", false); // reset
            _blackboard.SetValue("lookForTarget", false);

            _hasFinished = true;
        }
    }
}
