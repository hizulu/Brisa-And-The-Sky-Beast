using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 20/04/2025
public class BeastBrisaHalfDeadState : BeastState
{
    private Coroutine reviveCoroutine;
    private bool isRevivingBrisa = false;

    public override void OnEnter(Beast beast)
    {
        Debug.Log("Entra en el estado de revivir a Brisa");
        beast.agent.SetDestination(beast.playerTransform.position);
        beast.anim.SetBool("isWalking", true);
        isRevivingBrisa = false;
    }
    public override void OnUpdate(Beast beast)
    {
        if (!isRevivingBrisa && Vector3.Distance(beast.agent.transform.position, beast.playerTransform.position) < 3f)
        {
            reviveCoroutine = beast.StartCoroutine(ReviveBrisa(beast));
        }
    }
    public override void OnExit(Beast beast)
    {
        beast.agent.ResetPath();
        if (isRevivingBrisa)
        {
            beast.StopCoroutine(reviveCoroutine);
            reviveCoroutine = null;
            isRevivingBrisa = false;
            Debug.Log("Revivir cancelado al salir del estado.");
        }
        Debug.Log("Sale del estado de revivir a Brisa");
    }

    private IEnumerator ReviveBrisa(Beast beast)
    {
        beast.agent.ResetPath();
        isRevivingBrisa = true;
        beast.anim.SetBool("isWalking", false);
        beast.anim.SetTrigger("reviveBrisa");

        float duration = 3f;
        float elapsed = 0f;

        Debug.Log("La Bestia empieza a revivir a Brisa");

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            HalfDeadScreen.Instance.ShowHalfDeadScreenBrisaRevive(elapsed);
            yield return null;
        }

        EventsManager.TriggerNormalEvent("BrisaRevive");
        Debug.Log("Brisa ha sido revivida.");
        isRevivingBrisa = false;
        beast.TransitionToState(new BeastFreeState());
    }
}