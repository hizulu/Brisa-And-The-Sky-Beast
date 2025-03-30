using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPZigZagTest : MonoBehaviour
{
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float lateralOffset = 2f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private Transform target;
    private bool isAttacking = false;

    private void Start()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(DoThreeZigZags());
        }
    }

    private IEnumerator DoThreeZigZags()
    {
        Vector3 startPosition = transform.position;
        Vector3 directionToTarget = (target.position - startPosition).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, directionToTarget).normalized;

        float[] distances = { 0.25f, 0.5f, 0.75f };
        float[] lateralOffsets = { lateralOffset, lateralOffset * 0.66f, lateralOffset * 0.33f };

        for (int i = 0; i < 3; i++)
        {
            Vector3 jumpTarget = startPosition + directionToTarget * distances[i] * Vector3.Distance(startPosition, target.position);
            jumpTarget += (i % 2 == 0 ? right : -right) * lateralOffsets[i];

            yield return MoveInArc(transform.position, jumpTarget, jumpHeight);
        }

        Vector3 finalJumpTarget = target.position - directionToTarget * stopDistance;
        yield return MoveInArc(transform.position, finalJumpTarget, jumpHeight);

        Attack();
    }

    private IEnumerator MoveInArc(Vector3 start, Vector3 end, float height)
    {
        float elapsedTime = 0;
        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            Vector3 position = Vector3.Lerp(start, end, t);
            position.y += Mathf.Sin(t * Mathf.PI) * height;
            transform.position = position;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = end;
    }

    private void Attack()
    {
        Debug.Log("El enemigo ataca al jugador");
        isAttacking = false;
    }
}
