using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrol-Point to Point", menuName = "Enemy Logic/Patrol Logic/Point to Point")]
public class EnemyPatrolPointToPoint : EnemyPatrolSOBase
{
    [SerializeField] private float PointToPointMovementSpeed = 1f;

    //[Header("Puntos Recorrido Patrullaje")]
    //[SerializeField] private List<Transform> patrolPoints = new List<Transform>();

    //private int currentPoint = 0;

    private Transform _point1;
    private Transform _point2;

    private Vector3 _targetPos;
    private Vector3 _direction;

    private bool _point;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Debug.Log("Has entrado en estado de PatrolPointToPoint");
        Transform parent = transform.parent;

        // Encuentra los objetos hijo por su nombre
        _point1 = parent.Find("Punto1");
        _point2 = parent.Find("Punto2");

        _targetPos = _point1.position;
        _point = false;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Debug.Log("Has salido del estado Patrol - Point To Point");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if ((enemy.transform.position - _targetPos).sqrMagnitude < 2f)
        {
            _targetPos = GetNewTarget();
            _point = !_point;
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        _direction = (_targetPos - enemy.transform.position).normalized;
        enemy.MoveEnemy(_direction * PointToPointMovementSpeed);
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
