using System.Collections;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttackZigZagJump
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Ataque del Slime en el que ataca saltando en Zig-Zag.
 *              Simula la realización de tres saltos laterales acercándose al objetivo, luego realiza un último salto de ataque.
 *              Vuelve al estado de Chase si el objetivo se aleja demasiado. Cambia a estado de Retreat después de atacar.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. Script base con el comportamiento de ataque en zig-zag.
 *              1.1. Desactivación del agente del NavMesh para simular saltos
 *              1.2. Añadido función de hacer daño
 *              1.3. Añadido ataque a Bestia (22/04/2025)
 */
[CreateAssetMenu(fileName = "Attack-Zig Zag Jump", menuName = "Enemy Logic/Attack Logic/Ziz Zag Jump")]
public class EnemyAttackZigZagJump : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float attackDamage = 20f; // Daño que realiza el enemigo al objetivo con este ataque
    [SerializeField] private float distanceToStopAttackState = 8f; // Distancia a la que se tiene que encontrar el objetivo para salir del estado de ataque

    [SerializeField] private float jumpDuration = 0.8f; // Tiempo que debe durar cada salto
    [SerializeField] private float jumpHeight = 1f; // Desplazamiento vertical máximo en la simulación de salto
    [SerializeField] private float lateralOffset = 1.5f; // Desplazamiento lateral máximo de forma perpendicular a la dirección del objetivo
    [SerializeField] private float distanceToHit = 2f; // Distancia a la que se tiene que encontrar el objetivo para recibir el ataque

    private float stopDistance = 0.5f;
    private Transform targetTransform; // Variable auxiliar para definir el objetivo

    private bool isAttacking = false;

    private float distanceToStopAttackStateSQR = 0f; // Variable auxiliar para almacenar distancia evitando cálculo de raíz cuadrada cada frame.
    #endregion

    #region Sobreescriturta de métodos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        distanceToStopAttackStateSQR = distanceToStopAttackState * distanceToStopAttackState;

        SetTarget();

        if (!isAttacking)
        {
            isAttacking = true;
            enemy.agent.enabled = false; // Desactiva NavMeshAgent para saltos manuales
            enemy.StartCoroutine(DoThreeZigZags()); // Comienza a realizar los saltos
        }
    }

    public override void DoExitLogic()
    {
        enemy.SfxEnemy.StopSound(EnemySFXType.Attack);

        base.DoExitLogic();
        // Asegura reactivar el agente al salir del estado
        if (!enemy.agent.enabled)
            enemy.agent.enabled = true;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        enemy.SfxEnemy.PlayRandomSFX(EnemySFXType.Attack, 1f);

        float distanceToTargetSQR = (enemy.transform.position - targetTransform.position).sqrMagnitude;

        // Si el objetivo se aleja demasiado, vuelve al estado de Chase
        if (!isAttacking && distanceToTargetSQR > distanceToStopAttackStateSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);
    }
    #endregion

    #region Métodos específicos de EnemyAttackZigZagJump
    /*
     * Método que establece el objetivo del enemigo según la variable de enemy
     * El objetivo se ha establecido anteriormente en el estado de Patrol
     */
    private void SetTarget()
    {
        if (enemy.targetIsPlayer)
        {
            targetTransform = playerTransform;
            stopDistance = 0.5f;
        }
        else
        {
            targetTransform = beastTransform;
            stopDistance = 1.5f;
        }
    }
    /*
     * Corrutina que se encarga de hacer el Zig-Zag.
     * Realiza tres saltos en arco, con cada salto se acerca más al objetivo y cada salto tiene menos desplazamiento lateral.
     * Calcula la posición del salto final, el de ataque, con tiempo suficiente de antelación para que el jugador lo pueda evitar
     * Llama a la función de ataque tras realizar el salto final
     */
    private IEnumerator DoThreeZigZags()
    {
        Vector3 finalJumpTarget = Vector3.zero; // La posición del salto final se calcula antes de dar el tercer salto

        float[] distances = { 0.75f, 0.5f, 0.25f }; // Cada salto que hace se acerca más al objetivo, funciona como un porcentaje de la distancia total hacia el objetivo
        float[] lateralOffsets = { lateralOffset, lateralOffset * 0.66f, lateralOffset * 0.33f }; // Cada salto tiene menos desplazamiento lateral (zig-zag)

        for (int i = 0; i < 3; i++)
        {
            Vector3 directionToTarget = (targetTransform.position - enemy.transform.position).normalized; // Vector normalizado de la dirección hacia el objetivo
            Vector3 right = Vector3.Cross(Vector3.up, directionToTarget).normalized; // Vector perpendicular al de directionToTarget (para el desplazamiento lateral)

            Vector3 jumpTarget = enemy.transform.position + directionToTarget * distances[i] * Vector3.Distance(enemy.transform.position, targetTransform.position); // Calculo de la distancia del objetivo a la que debe saltar
            jumpTarget += (i % 2 == 0 ? right : -right) * lateralOffsets[i]; // Alterna el desplazamiento lateral del zig-zag para que sea en direcciones opuestas en la perpendicular de la dirección del objetivo

            // Justo antes del tercer salto, prepara el salto de ataque para que el jugador pueda evitarlo
            if (i == 2)
            {
                finalJumpTarget = targetTransform.position - directionToTarget * stopDistance; // Salta delante del objetivo
            }
            enemy.anim.SetTrigger("Jump");
            yield return MoveInArc(enemy.transform.position, jumpTarget, jumpHeight); // Simulación del salto
            enemy.transform.position = jumpTarget;
        }

        // Realiza el salto de ataque (con target ya calculado anteriormente)
        yield return MoveInArc(enemy.transform.position, finalJumpTarget, jumpHeight);
        enemy.transform.position = finalJumpTarget;

        Attack();
    }

    /*
     * Corrutina que se encarga de simular el salto
     * @param1 start - posición de la que parte, posición actual del enemigo
     * @param2 end - posición a la que debe saltar, calculada anteriormente
     * @param3 height - altura máxima a la que debe saltar en mitad del arco
     */
    private IEnumerator MoveInArc(Vector3 start, Vector3 end, float height)
    {
        float elapsedTime = 0;
        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            Vector3 position = Vector3.Lerp(start, end, t);
            position.y += Mathf.Sin(t * Mathf.PI) * height;

            // Apunta al target durante el salto (horizontalmente)
            Vector3 lookDirection = (targetTransform.position - enemy.transform.position);
            lookDirection.y = 0f; // Elimina componente vertical
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            enemy.transform.position = position;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura orientación final tras el salto
        Vector3 finalDirection = (targetTransform.position - end);
        finalDirection.y = 0f;
        if (finalDirection.sqrMagnitude > 0.01f)
        {
            enemy.transform.rotation = Quaternion.LookRotation(finalDirection);
        }

        enemy.transform.position = end;
    }

    /*
     * Función de ataque al objetivo
     */
    private void Attack()
    {
        float distanceToTargetSQR = (enemy.transform.position - targetTransform.position).sqrMagnitude;

        // Golpea al objetivo
        if (distanceToTargetSQR < distanceToHit * distanceToHit)
        {

            if (enemy.targetIsPlayer)
            {
                Debug.Log("triggerea evento de hacer daño a player");
                EventsManager.TriggerSpecialEvent<float>("OnAttackPlayer", attackDamage);
            }
            else
                EventsManager.TriggerSpecialEvent<float>("OnAttackBeast", attackDamage);
        }
        // TODO: diferente daño según a quién golpee

        isAttacking = false;
        enemy.agent.enabled = true; // Reactiva el agente

        // Cambia a estado de Retreat después de realizar el ataque
        enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyRetreatState);
    }
    #endregion
}
