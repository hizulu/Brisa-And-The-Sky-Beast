using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerStatsData
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 03/04/2025
 * DESCRIPCIÓN: Clase serializable que contiene las stats de Player.
 * VERSIÓN: 1.0
 */

[Serializable]
public class PlayerStatsData
{
    [field: SerializeField] public float MaxHealth { get; private set; } = 100f;
    [field: SerializeField] public float CurrentHealth { get; set; } // Sin el "private set" porque sino no puedo modificar el valor desde otros scripts.
    [field: SerializeField] public float AttackDamageBase { get; private set; } = 10f;

    [field: SerializeField] public float MaxTimeHalfDead { get; private set; } = 60f;
    [field: SerializeField] public float CurrentTimeHalfDead { get; set; }
}
