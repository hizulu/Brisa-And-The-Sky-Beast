using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStatsData
{
    [field: SerializeField] public float MaxHealth { get; private set; } = 100f;
    [field: SerializeField] public float CurrentHealth { get; set; } // Sin el "private set" porque sino no puedo modificar el valor desde otros scripts.
    [field: SerializeField] public float AttackDamageBase { get; private set; } = 100f;

    [field: SerializeField] public float MaxTimeHalfDead { get; private set; } = 60f;
    [field: SerializeField] public float CurrentTimeHalfDead { get; set; }
}
