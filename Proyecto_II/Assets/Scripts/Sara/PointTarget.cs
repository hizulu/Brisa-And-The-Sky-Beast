using UnityEngine;

public class PointTarget : MonoBehaviour
{
    public Transform enemyTargetPos;

    public Vector3 posPointTarget = new Vector3(0, 0, 0);

    public void SetTarget(Transform newTarget)
    {
        enemyTargetPos = newTarget;
        transform.SetParent(enemyTargetPos);
        transform.localPosition = posPointTarget;
        gameObject.SetActive(true);
    }

    public void ClearTarget()
    {
        transform.SetParent(null);
        enemyTargetPos = null;
        gameObject.SetActive(false);
    }
}
