using System.Collections;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttackZigZagJump
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Ataque del Slime en el que ataca saltando en Zig-Zag.
 *              Simula la realización de tres saltos laterales acercándose al jugador, luego realiza un último salto de ataque.
 *              Vuelve al estado de Chase si el jugador se aleja demasiado. Cambia a estado de Retreat después de atacar.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. Script base con el comportamiento de ataque en zig-zag.
 *              1.1. Desactivación del agente del NavMesh para simular saltos
 */
[CreateAssetMenu(fileName = "Attack-Zig Zag Jump", menuName = "Enemy Logic/Attack Logic/Ziz Zag Jump")]
public class EnemyAttackZigZagJump : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float attackDamage = 20f; // Daño que realiza el enemigo al jugador con este ataque
    [SerializeField] private float distanceToStopAttackState = 8f; // Distancia a la que se tiene que encontrar el jugador para salir del estado de ataque

    [SerializeField] private float jumpDuration = 0.6f; // Tiempo que debe durar cada salto
    [SerializeField] private float jumpHeight = 1.5f; // Desplazamiento vertical máximo en la simulación de salto
    [SerializeField] private float lateralOffset = 2f; // Desplazamiento lateral máximo de forma perpendicular a la dirección del jugador
    [SerializeField] private float stopDistance = 0.5f; // Distancia del jugador a la que debe parar el salto final
    [SerializeField] private float distanceToHit = 2f; // Distancia a la que se tiene que encontrar el jugador para recibir el ataque

    private bool isAttacking = false;

    private float distanceToStopAttackStateSQR = 0f; // Variable auxiliar para almacenar distancia evitando cálculo de raíz cuadrada cada frame.
    #endregion

    #region Sobreescriturta de métodos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        distanceToStopAttackStateSQR = distanceToStopAttackState * distanceToStopAttackState;

        if (!isAttacking)
        {
            isAttacking = true;
            enemy.agent.enabled = false; // Desactiva NavMeshAgent para saltos manuales
            enemy.StartCoroutine(DoThreeZigZags()); // Comienza a realizar los saltos
        }
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        // Asegura reactivar el agente al salir del estado
        if (!enemy.agent.enabled)
            enemy.agent.enabled = true;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        // Si el jugador se aleja demasiado, vuelve al estado de Chase
        if (!isAttacking && distanceToPlayerSQR > distanceToStopAttackStateSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);
    }
    #endregion

    #region Métodos específicos de EnemyAttackZigZagJump
    /*
     * Corrutina que se encarga de hacer el Zig-Zag.
     * Realiza tres saltos en arco, con cada salto se acerca más al jugador y cada salto tiene menos desplazamiento lateral.
     * Calcula la posición del salto final, el de ataque, con tiempo suficiente de antelación para que el jugador lo evite
     * Llama a la función de ataque tras realizar el salto final
     */
    private IEnumerator DoThreeZigZags()
    {
        Vector3 finalJumpTarget = Vector3.zero; // La posición del salto final se calcula antes de dar el tercer salto

        float[] distances = { 0.75f, 0.5f, 0.25f }; // Cada salto que hace se acerca más al jugador
        float[] lateralOffsets = { lateralOffset, lateralOffset * 0.66f, lateralOffset * 0.33f }; // Cada salto tiene menos desplazamiento lateral (zig-zag)

        for (int i = 0; i < 3; i++)
        {
            Vector3 directionToTarget = (playerTransform.position - enemy.transform.position).normalized; // Vector normalizado de la dirección hacia el jugador
            Vector3 right = Vector3.Cross(Vector3.up, directionToTarget).normalized; // Vector perpendicular al de directionToTarget (para el desplazamiento lateral)

            Vector3 jumpTarget = enemy.transform.position + directionToTarget * distances[i] * Vector3.Distance(enemy.transform.position, playerTransform.position); // Calculo de la distancia del jugador a la que debe saltar
            jumpTarget += (i % 2 == 0 ? right : -right) * lateralOffsets[i]; // Alterna el desplazamiento lateral del zig-zag para que sea en direcciones opuestas en la perpendicular de la dirección del jugador

            // Justo antes del tercer salto, prepara el salto de ataque para que el jugador pueda evitarlo
            if (i == 2)
            {
                finalJumpTarget = playerTransform.position - directionToTarget * stopDistance; // Salta delante del jugador
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
            Vector3 position = Vector3.Lerp(start, end, t); // Movimiento interpolado linealmente entre ambos puntos
            position.y += Mathf.Sin(t * Mathf.PI) * height; // Simulación del cambio de altura en arco
            enemy.transform.position = position; // Mueve al enemigo
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        enemy.transform.position = end; // Asegurar la posición final del enemigo
    }

    /*
     * Función de ataque al jugador
     */
    private void Attack()
    {
        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        // Golpea al jugador
        if (distanceToPlayerSQR < distanceToHit * distanceToHit)
            EventsManager.TriggerSpecialEvent<float>("OnAttackPlayer", attackDamage);

        isAttacking = false;
        enemy.agent.enabled = true; // Reactiva el agente

        // Cambia a estado de Retreat después de realizar el ataque
        enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyRetreatState);
    }
    #endregion
}
