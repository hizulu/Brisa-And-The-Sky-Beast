using System.Collections;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyRetreatJumpBack
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 07/05/2025
 * DESCRIPCI�N: Clase que define el comportamiento espec�fico de Retreat en el que el enemigo huye con un gran salto en la direcci�n contraria del objetivo.
 *              Despu�s de huir, cambia a estado de Chase si el objetivo se encuentra lo suficientemente cerca para seguirlo.
 *              Despu�s de huir, si el objetivo se encuentra lejos, vuelve a estado de Idle.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus m�todos y tiene acceso a sus variables.            
 * VERSI�N: 1.0.
 */

[CreateAssetMenu(fileName = "Retreat-Jump Back", menuName = "Enemy Logic/Retreat Logic/Jump Back")]
public class EnemyRetreatJumpBack : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float jumpBackForce = 7f;
    [SerializeField] private float jumpUpForce = 4f;
    [SerializeField] private float targetChaseRange = 10f;
    [SerializeField] private float jumpDuration = 1f; // Duraci�n del salto
    [SerializeField] private AnimationCurve jumpArc = AnimationCurve.EaseInOut(0, 0, 1, 1); // Para controlar la curva del salto

    private float targetChaseRangeSQR;

    private Transform targetTransform;
    // private bool retreatFinish = false;
    #endregion

    #region Sobreescriturta de m�todos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        // retreatFinish = false;
        base.DoEnterLogic();
        enemy.anim.SetBool("isRetreating", true);

        targetChaseRangeSQR = targetChaseRange * targetChaseRange;

        SetTarget();

        enemy.agent.enabled = false;

        JumpBack();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.anim.SetBool("isRetreating", false);
        enemy.agent.enabled = true;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if(!FinishAnimation()) // Si la animaci�n de Retreat no ha acabado, no hacemos nada.
            return;
        else // Si ha terminado, comprueba la distancia entre el enemigo y el objetivo.
        {
            float distanceToTargetSQR = (enemy.transform.position - targetTransform.position).sqrMagnitude;

            if (distanceToTargetSQR < targetChaseRangeSQR) // Si la distancia "enemy - target" es menor que la distancia de perseguir
                enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState); // Pasa a perseguir de nuevo.
            else
                enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyIdleState); // Sino, pasa a idle.
        }
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
    #endregion

    #region M�todos Propios EnemyRetreatJumpBack
    /*
     * M�todo para obtener la posici�n de Player o de Beast (para saber en qu� direcci�n debe saltar el enemigo).
     */
    private void SetTarget()
    {
        targetTransform = enemy.targetIsPlayer ? playerTransform : beastTransform;
    }

    /*
     * Gesti�n del salto hacia atr�s.
     */
    public void JumpBack()
    {
        Vector3 directionAway = (enemy.transform.position - targetTransform.position).normalized;
        Vector3 horizontalOffset = directionAway * jumpBackForce;
        float verticalOffset = jumpUpForce;

        Vector3 startPos = enemy.transform.position;
        Vector3 endPos = startPos + horizontalOffset;

        enemy.StartCoroutine(SimulateJumpArc(startPos, endPos, verticalOffset));
    }

    private IEnumerator SimulateJumpArc(Vector3 start, Vector3 end, float height)
    {
        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            float t = elapsed / jumpDuration;

            Vector3 horizontalPos = Vector3.Lerp(start, end, t);
            float arcHeight = jumpArc.Evaluate(t) * height;

            Vector3 currentPos = new Vector3(
                horizontalPos.x,
                Mathf.Lerp(start.y, end.y, t) + arcHeight,
                horizontalPos.z
            );

            enemy.transform.position = currentPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegura que termine exactamente en el suelo (end.y)
        enemy.transform.position = end;
    }

    /*
     * M�todo booleano para saber cu�ndo ha acabado la animaci�n de Retreat.
     */
    private bool FinishAnimation()
    {
        if (enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Retreat") && enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            return true;

        return false;
    }
    #endregion
}
