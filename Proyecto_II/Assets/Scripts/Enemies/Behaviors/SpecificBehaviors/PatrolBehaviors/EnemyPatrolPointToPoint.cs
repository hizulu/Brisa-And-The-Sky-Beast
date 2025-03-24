using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrol-Point to Point", menuName = "Enemy Logic/Patrol Logic/Point to Point")]
public class EnemyPatrolPointToPoint : EnemyPatrolSOBase
{
    [SerializeField] private float PointToPointMovementSpeed = 1f;

    private Transform _point1;
    private Transform _point2;

    private Vector3 _targetPos;
    private Vector3 _direction;

    private bool _point;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Debug.Log("Has entrado en estado de Patrol - Point To Point");
        Transform parent = transform.parent;

        // Encuentra los objetos hijo por su nombre
        GameObject _point1 = GameObject.Find("Point1");
        GameObject _point2 = GameObject.Find("Point2");

        if (_point1 == null)
            Debug.LogError("Point1 no encontrado como hijo de " + parent.name);
        if (_point2 == null)
            Debug.LogError("Point2 no encontrado como hijo de " + parent.name);

        //_targetPos = _point1.position;
        _point = false;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Debug.Log("Has salid del estado de Patrol - Point To Point");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        _direction = (_targetPos - enemy.transform.position).normalized;

        MoveEnemy();

        if ((enemy.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _targetPos = GetNewTarget();
            _point = !_point;
        }
    }

    public override void DoPhysiscsLogic()
    {
        base.DoPhysiscsLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }

    private Vector3 GetNewTarget()
    {
        if (_point)
            return _point1.position;
        else
            return _point2.position;
    }
}
