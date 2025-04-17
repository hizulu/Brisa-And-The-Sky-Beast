using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 17/04/2025
public class HealBeast : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public HealBeast(Blackboard blackboard, Beast beast)
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

            _beast.agent.ResetPath(); // TODO: si hay que reposicionar a la bestia se haría aquí

            _beast.anim.SetBool("isWalking", false);
            _beast.anim.SetBool("isHealing", true);

            Heal();// TODO: añadir funcionalidad de sanación

            Debug.Log("Starting to heal");
            _beast.StartNewCoroutine(Healing(2f), this);
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

    public void Heal()
    {
        // TODO: check if there are fruits
        // TODO: consume fruit
        // TODO: healing particle effect
        // TODO: increase health
    }

    private IEnumerator Healing(float duration)
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

        _beast.anim.SetBool("isHealing", false);
        _blackboard.SetValue("isCoroutineActive", false);
        _blackboard.SetValue("menuOpened", false);

        Debug.Log("Finished healing");

        _hasFinished = true;
    }
}
