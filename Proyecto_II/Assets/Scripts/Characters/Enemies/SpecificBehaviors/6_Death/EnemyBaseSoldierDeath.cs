using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Death BaseSoldier", menuName = "Enemy Logic/Death Logic/Death BaseSoldier")]
public class EnemyBaseSoldierDeath : EnemyStateSOBase
{
    [SerializeField] private float maxTimeToDeath = 2f;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        //Debug.Log("Has entrado en el estado de Muerte del Soldado Base.");
        enemy.anim.SetTrigger("Death");
        enemy.StartCoroutine(BaseSoldierDeath());
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        //Debug.Log("Has salido del estado de Muerte del Soldado Base.");
    }

    private IEnumerator BaseSoldierDeath()
    {
        yield return new WaitForSeconds(maxTimeToDeath);

        enemy.beast?.OnEnemyExit(enemy.gameObject);
        Destroy(enemy.gameObject);
        //Debug.Log("Soldier desaparece");
        enemy.GetComponent<LootBox>()?.DropLoot();
    }

    //public void BaseSoldierDeath()
    //{
    //    currentTime += Time.deltaTime;

    //    if (currentTime > maxTimeToDeath)
    //    {
    //        //enemy.anim.enabled = false;
    //        enemy.beast?.OnEnemyExit(enemy.gameObject);
    //        Destroy(enemy.gameObject);
    //        Debug.Log("Soldier desaparece");
    //        enemy.GetComponent<LootBox>()?.DropLoot();
    //    }
    //}
}
