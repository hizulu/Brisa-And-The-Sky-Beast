using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 20/04/2025
public class BeastHalfDeadState : BeastState
{
    private bool _countdownStarted = false;
    private bool _beastRevived = false;
    public override void OnEnter(Beast beast)
    {
        EventsManager.TriggerNormalEvent("AskForHelpBrisa"); // Cuando Bestia entra en estado de medio - muerto, manda un evento para avisar a Brisa de que necesita que le reviva.
        beast.anim.SetBool("isWalking", false);
        beast.anim.SetBool("isHalfDead", true);
        beast.agent.ResetPath();
        Debug.Log("Beast is half dead");

        EventsManager.CallNormalEvents("ReviveBeast", ReviveBeast);
    }
    public override void OnUpdate(Beast beast)
    {
        if (_countdownStarted)
            return;

        beast.StartCoroutine(BeastHalfDeadCountdown(beast, beast.halfDeadDuration));
    }
    public override void OnExit(Beast beast)
    {
        beast.anim.SetBool("isHalfDead", false);
        beast.blackboard.SetValue("beastIsHalfDead", false);

        EventsManager.StopCallNormalEvents("ReviveBeast", ReviveBeast);
    }

    private IEnumerator BeastHalfDeadCountdown(Beast beast, float duration)
    {
        _countdownStarted = true;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (_beastRevived)
            {
                Debug.Log("Beast ha sido revivida, volviendo a estado natural");
                beast.currentHealth = beast.maxHealth;
                beast.TransitionToState(new BeastFreeState());                
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Tiempo de espera completado sin que player reviva a Beast. Llamando a condición de fin de juego");
        beast.anim.SetBool("isDead", true);
        // TODO: wait for animation before game end
        GameManager.Instance.GameOver();
    }

    private void ReviveBeast()
    {
        // TODO: revive animation, sound effect, visual effect
        _beastRevived = true;
    }
}
