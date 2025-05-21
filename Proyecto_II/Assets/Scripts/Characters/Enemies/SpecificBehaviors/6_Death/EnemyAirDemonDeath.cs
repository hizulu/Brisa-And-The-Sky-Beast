using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Death AirDemon", menuName = "Enemy Logic/Death Logic/Death AirDemon")]
public class EnemyAirDemonDeath : EnemyStateSOBase
{
    [SerializeField] private float maxTimeToDeath = 2f;
    [SerializeField] private float currentTime = 0f;

    public override void DoEnterLogic()
    {
        enemy.anim.SetTrigger("Death");
        base.DoEnterLogic();
        currentTime = 0f;
        Debug.Log("Has entrado en el estado de Muerte del Demonio de Aire.");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        AirDemonDeath();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Debug.Log("Has salido del estado de Muerte del Demonio de Aire.");
    }

    public void AirDemonDeath()
    {
        currentTime += Time.deltaTime;

        if (currentTime > maxTimeToDeath)
        {
            //enemy.anim.enabled = false;
            enemy.beast?.OnEnemyExit(enemy.gameObject);
            Debug.Log("Destruye al enemigo");
            Destroy(enemy.gameObject);
            enemy.GetComponent<LootBox>()?.DropLoot();
            Debug.Log("Triggerea evento");
            EventsManager.TriggerNormalEvent("GameEndTrigger");
        }
    }
}
