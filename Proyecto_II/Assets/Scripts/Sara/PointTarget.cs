using UnityEngine;

/*
 * NOMBRE CLASE: PointTarget
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 20/04/2025
 * DESCRIPCIÓN: Gestiona que el punto que señala al enemigo marcado se active y desactive.
 * VERSIÓN: 1.0. 
 */
public class PointTarget : MonoBehaviour
{
    public Transform enemyTargetPos;

    void Update()
    {
        if (enemyTargetPos != null)
            transform.position = enemyTargetPos.position + Vector3.up * 2f;
        else
            gameObject.SetActive(false);
    }

    public void SetTarget(Transform newTarget)
    {
        enemyTargetPos = newTarget;
        gameObject.SetActive(true);
    }

    public void ClearTarget()
    {
        enemyTargetPos = null;
    }
}
