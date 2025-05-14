using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyRetreatJumpBack
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 07/05/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Retreat con un salto hacia atrás.
 * VERSIÓN: 1.0.
 */

[CreateAssetMenu(fileName = "Retreat-Jump Back", menuName = "Enemy Logic/Retreat Logic/Jump Back")]
public class EnemyRetreatJumpBack : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float jumpBackForce = 5f;
    [SerializeField] private float jumpUpForce = 3f;
    [SerializeField] private float targetChaseRange = 10f;
    [SerializeField] private float jumpDuration = 0.8f; // Duración del salto
    [SerializeField] private AnimationCurve jumpArc = AnimationCurve.EaseInOut(0, 0, 1, 1); // Para controlar la curva del salto

    private float targetChaseRangeSQR;

    private Transform targetTransform;
    // private bool retreatFinish = false;
    #endregion

    #region Sobreescriturta de métodos de EnemyStateSOBase
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

        if(!FinishAnimation()) // Si la animación de Retreat no ha acabado, no hacemos nada.
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

    #region Métodos Propios EnemyRetreatJumpBack
    /*
     * Método para obtener la posición de Player o de Beast (para saber en qué dirección debe saltar el enemigo).
     */
    private void SetTarget()
    {
        targetTransform = enemy.targetIsPlayer ? playerTransform : beastTransform;
    }

    /*
     * Gestión del salto hacia atrás.
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
            float arc = jumpArc.Evaluate(t); // Altura basada en la curva

            Vector3 currentPos = Vector3.Lerp(start, end, t);
            currentPos.y += arc * height;

            enemy.transform.position = currentPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Aseguramos que llega exactamente al final
        Vector3 finalPos = end;
        finalPos.y = start.y;
        enemy.transform.position = finalPos;
    }

    /*
     * Método booleano para saber cuándo ha acabado la animación de Retreat.
     */
    private bool FinishAnimation()
    {
        if (enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Retreat") && enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            return true;

        return false;
    }
    #endregion
}
