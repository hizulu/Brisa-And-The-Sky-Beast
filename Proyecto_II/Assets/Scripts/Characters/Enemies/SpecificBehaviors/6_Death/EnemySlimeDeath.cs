using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemySlimeDeath
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 19/05/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Muerte del enemigo (para el slime, concretamente).
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. 
 */

[CreateAssetMenu(fileName = "Death Slime", menuName = "Enemy Logic/Death Logic/Death Slime")]
public class EnemySlimeDeath : EnemyStateSOBase
{
    [SerializeField] private float maxTimeToDeath = 1f;
    //[SerializeField] private float currentTime = 0f;

    public override void DoEnterLogic()
    {
        enemy.SfxEnemy.PlayRandomSFX(EnemySFXType.Death, 1f);
        base.DoEnterLogic();
        enemy.StartCoroutine(SlimeDeath());
        //currentTime = 0f;
        Debug.Log("Has entrado en el estado de Muerte del Slime.");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();        
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Debug.Log("Has salido del estado de Muerte del Slime.");
    }

    private IEnumerator SlimeDeath()
    {
        yield return new WaitForSeconds(maxTimeToDeath);

        enemy.beast?.OnEnemyExit(enemy.gameObject);
        Destroy(enemy.gameObject);
        enemy.GetComponent<LootBox>()?.DropLoot();
    }

    //private void SlimeDeath()
    //{
    //    currentTime += Time.deltaTime;

    //    if(currentTime > maxTimeToDeath)
    //    {
    //        enemy.anim.enabled = false;
    //        enemy.beast?.OnEnemyExit(enemy.gameObject);
    //        Destroy(enemy.gameObject);
    //        enemy.GetComponent<LootBox>()?.DropLoot();
    //    }
    //}
}
