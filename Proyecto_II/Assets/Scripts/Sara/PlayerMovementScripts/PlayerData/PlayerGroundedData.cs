using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerGroundedData
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 
 * DESCRIPCIÓN: Clase que contiene los datos necesarios para el estado del jugador en el suelo.
 * VERSIÓN: 1.0
 */
[Serializable]
public class PlayerGroundedData
{
    [field: SerializeField][field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
    [field: SerializeField] public PlayerWalkData WalkData { get; private set; }

    [field: SerializeField] public LayerMask ClickableLayers { get; private set; }
    [field: SerializeField] public ParticleSystem ClickEffect { get; private set; }
    //[field: SerializeField] public SpriteRenderer AreaMoveBeast { get; private set; }

    public float GroundCheckDistance = 0.1f;

    public bool IsPointedModeActived = false;
}
