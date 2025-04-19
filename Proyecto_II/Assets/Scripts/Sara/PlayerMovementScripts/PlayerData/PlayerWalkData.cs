using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerWalkData
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 
 * DESCRIPCI�N: Clase serializable que contiene modificadores de velocidad para los diferentes modos de movimiento de Player.
 * VERSI�N: 1.0
 */
[Serializable]
public class PlayerWalkData
{
    [field: SerializeField][field: Range(0f, 1f)] public float WalkSpeedModif { get; private set; } = 0.5f;
    [field: SerializeField][field: Range(0f, 1f)] public float RunSpeedModif { get; private set; } = 2f;
    [field: SerializeField][field: Range(0f, 1f)] public float CrouchSpeedModif { get; private set; } = 0.1f;
}
