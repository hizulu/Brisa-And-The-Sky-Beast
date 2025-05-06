using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagesBrisa : MonoBehaviour
{
    private Player player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            player.Data.StatsData.CurrentHealth = 10f;
        }
    }
}
