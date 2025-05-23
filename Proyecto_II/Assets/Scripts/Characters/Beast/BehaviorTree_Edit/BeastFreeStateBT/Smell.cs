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
    private float _duration;

    private bool _isRunning = false;
    private bool _hasFinished = false;


    public Smell(Blackboard blackboard, Beast beast)
    {
        _blackboard = blackboard;
        _beast = beast;
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

            _duration = AnimationDurationDatabase.Instance.GetClipDuration("Beast_SmellDown");

            Debug.Log("Starting to smell");
            _beast.StartNewCoroutine(Smelling(_duration), this);
            _beast.SfxBeast.PlayRandomSFX(BeastSFXType.Smell);
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
        _blackboard.ClearKey("shouldSmell");

        Debug.Log("Finished smelling");

        _blackboard.SetValue("reachedTarget", false); 

        _hasFinished = true;
    }
}
