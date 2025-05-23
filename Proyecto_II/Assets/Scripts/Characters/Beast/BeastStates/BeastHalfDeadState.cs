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
        //beast.currentHalffDeadDuration = beast.maxHalfDeadDuration;
        EventsManager.TriggerNormalEvent("AskForHelpBrisa"); // Cuando Bestia entra en estado de medio - muerto, manda un evento para avisar a Brisa de que necesita que le reviva.
        beast.anim.SetBool("isWalking", false);
        beast.anim.SetBool("isHalfDead", true);
        beast.SfxBeast.PlayRandomSFX(BeastSFXType.Halfdead);
        beast.agent.ResetPath();
        Debug.Log("Beast is half dead");

        EventsManager.CallNormalEvents("ReviveBeast", ReviveBeast);
    }
    public override void OnUpdate(Beast beast)
    {
        if (_countdownStarted)
            return;

        beast.StartCoroutine(BeastHalfDeadCountdown(beast, beast.maxHalfDeadDuration));
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
        float elapsedTime = duration;

        while (elapsedTime > 0f)
        {
            if (_beastRevived)
            {
                HalfDeadScreen.Instance.HideHalfDeadScreenBestia();
                Debug.Log("Beast ha sido revivida, volviendo a estado natural");
                beast.currentHealth = beast.maxHealth / 2;
                beast.TransitionToState(new BeastFreeState());
                beast.StopAllCoroutines();
                break;
            }
            
            if (!HalfDeadScreen.Instance.IsReviving)
            {
                elapsedTime -= Time.deltaTime;
                HalfDeadScreen.Instance.ShowHalfDeadScreenBestia(elapsedTime, duration);
            }

            yield return null;
        }

        if (!_beastRevived)
        {
            Debug.Log("Tiempo de espera completado sin que player reviva a Beast. Llamando a condición de fin de juego");
            beast.anim.SetBool("isDead", true);
            beast.SfxBeast.PlayRandomSFX(BeastSFXType.Dead);
            // TODO: wait for animation before game end
            GameManager.Instance.GameOver();
        }
    }

    private void ReviveBeast()
    {
        // TODO: revive animation, sound effect, visual effect
        Debug.Log("La Bestia ha recibido bien la llamada de Revivir");
        _beastRevived = true;
    }
}
