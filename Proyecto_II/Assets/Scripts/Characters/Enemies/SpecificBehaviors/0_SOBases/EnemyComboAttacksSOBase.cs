using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComboAttacksSOBase : ScriptableObject
{
    protected Enemy enemy;

    public virtual void Initialize(Enemy _enemy)
    {
        enemy = _enemy;
        // Debug.Log("Leyendo SOBase de Combo Attack");
    }

    public virtual void FrameLogic() { }

    public virtual void EnemyAttack() { }

    public virtual void FinishAnimation() { }

    public virtual bool IsFinished() {  return false; }

    public virtual void Exit() { }
}