using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 29/04/2025
public class AttackCombatTarget : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;
    private bool _isRunning = false;
    private bool _hasFinished = false;

    private bool _isAttackNormal = true;
    private float _distanceToHit = 6f;
    private float _attackDamage = 0f;

    public AttackCombatTarget(Blackboard blackboard, Beast beast)
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

            Attack();

            _beast.StartNewCoroutine(Attacking(1f), this);
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

    private IEnumerator Attacking(float duration)
    {
        if (!_blackboard.TryGetValue("targetForCombat", out GameObject enemy))
        {
            Debug.LogWarning("No targetForCombat en blackboard");
            yield break;
        }

        Transform targetTransform = enemy.transform;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (targetTransform == null)
            {
                Debug.LogWarning("Ha muerto target mientrás bestia le atacaba");
                _blackboard.ClearKey("targetForCombat");
                yield break;
            }

            // Dirección sin componente vertical
            Vector3 directionToTarget = targetTransform.position - _beast.transform.position;
            directionToTarget.y = 0f;

            if (directionToTarget.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                _beast.transform.rotation = Quaternion.Slerp(
                    _beast.transform.rotation,
                    targetRotation,
                    Time.deltaTime * 10f
                );
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        OnCoroutineEnd();
    }

    private void Attack()
    {
        if (!_blackboard.TryGetValue("targetForCombat", out GameObject enemy))
            return;

        SetAttackRandom();

        Transform targetTransform = enemy.transform;
        float distanceToTargetSQR = (_beast.transform.position - targetTransform.position).sqrMagnitude;

        // Golpea al objetivo
        if (distanceToTargetSQR < _distanceToHit * _distanceToHit)
        {
            Debug.Log("Hits enemy");
            EventsManager.TriggerSpecialEvent<float>("OnBeastAttackEnemy", _attackDamage);
            enemy.GetComponent<Enemy>().OnHit();
        }
    }

    private void SetAttackRandom()
    {
        _isAttackNormal = Random.Range(0, 5) > 1; // 40% de probabilidad de realizar ataque especial
        if (_isAttackNormal)
        {
            _beast.anim.SetBool("attackSwipe", true);
            _beast.SfxBeast.PlayRandomSFX(BeastSFXType.AttackClaw);
            _attackDamage = _beast.swipeAttackDamage;
        }
        else
        {
            _beast.anim.SetBool("attackBite", true);
            _beast.SfxBeast.PlayRandomSFX(BeastSFXType.AttackBite);
            _attackDamage = _beast.biteAttackDamage;
        }
    }

    public void OnCoroutineEnd()
    {
        if (_hasFinished) return;

        _blackboard.SetValue("reachedCombatTarget", false);
        _blackboard.SetValue("isCoroutineActive", false);
        _blackboard.SetValue("attacked", true);
        _blackboard.SetValue("menuOpened", false);
        _blackboard.ClearKey("targetForCombat");

        Debug.Log("Finished attacking");

        _hasFinished = true;
    }
}
