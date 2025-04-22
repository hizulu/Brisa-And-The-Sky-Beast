using UnityEngine;

/*
 * NOMBRE CLASE: PointTarget
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 20/04/2025
 * DESCRIPCI�N: Gestiona que el punto que se�ala al enemigo marcado se active y desactive.
 * VERSI�N: 1.0. 
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
