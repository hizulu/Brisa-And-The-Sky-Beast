using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Death AirDemon", menuName = "Enemy Logic/Death Logic/Death AirDemon")]
public class EnemyAirDemonDeath : EnemyStateSOBase
{
    [SerializeField] private float maxTimeToDeath = 2f;

    public override void DoEnterLogic()
    {
        enemy.anim.SetTrigger("Death");
        base.DoEnterLogic();
        Debug.Log("Has entrado en el estado de Muerte del Demonio de Aire.");
        enemy.SfxEnemy.PlayRandomSFX(EnemySFXType.Death);

        enemy.StartCoroutine(AirDemonDeath());
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Debug.Log("Has salido del estado de Muerte del Demonio de Aire.");
    }

    private IEnumerator AirDemonDeath()
    {
        yield return new WaitForSeconds(maxTimeToDeath);

        enemy.beast?.OnEnemyExit(enemy.gameObject);
        Debug.Log("Destruye al enemigo");
        Destroy(enemy.gameObject);
        enemy.GetComponent<LootBox>()?.DropLoot();
        Debug.Log("Triggerea evento");
        EventsManager.TriggerNormalEvent("GameEndTrigger");
    }

    //public void AirDemonDeath()
    //{
    //    currentTime += Time.deltaTime;

    //    if (currentTime > maxTimeToDeath)
    //    {
    //        //enemy.anim.enabled = false;
    //        enemy.beast?.OnEnemyExit(enemy.gameObject);
    //        Debug.Log("Destruye al enemigo");
    //        Destroy(enemy.gameObject);
    //        enemy.GetComponent<LootBox>()?.DropLoot();
    //        Debug.Log("Triggerea evento");
    //        EventsManager.TriggerNormalEvent("GameEndTrigger");
    //    }
    //}
}
