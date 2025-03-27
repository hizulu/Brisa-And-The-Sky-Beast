using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagesBrisa : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetHealth(50);
        }
    }
}
