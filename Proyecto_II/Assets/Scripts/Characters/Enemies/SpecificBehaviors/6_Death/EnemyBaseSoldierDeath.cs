using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Death BaseSoldier", menuName = "Enemy Logic/Death Logic/Death BaseSoldier")]
public class EnemyBaseSoldierDeath : EnemyStateSOBase
{
    [SerializeField] private float maxTimeToDeath = 2f;
    [SerializeField] private float currentTime = 0f;

    public override void DoEnterLogic()
    {
        enemy.anim.SetTrigger("Death");
        base.DoEnterLogic();
        currentTime = 0f;
        Debug.Log("Has entrado en el estado de Muerte del Soldado Base.");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        BaseSoldierDeath();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Debug.Log("Has salido del estado de Muerte del Soldado Base.");
    }

    public void BaseSoldierDeath()
    {
        currentTime += Time.deltaTime;

        if (currentTime > maxTimeToDeath)
        {
            //enemy.anim.enabled = false;
            enemy.beast?.OnEnemyExit(enemy.gameObject);
            Destroy(enemy.gameObject);
            enemy.GetComponent<LootBox>()?.DropLoot();
        }
    }
}
