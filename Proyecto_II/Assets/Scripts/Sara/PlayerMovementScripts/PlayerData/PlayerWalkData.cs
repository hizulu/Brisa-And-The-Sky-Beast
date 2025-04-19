using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerWalkData
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 
 * DESCRIPCIÓN: Clase serializable que contiene modificadores de velocidad para los diferentes modos de movimiento de Player.
 * VERSIÓN: 1.0
 */
[Serializable]
public class PlayerWalkData
{
    [field: SerializeField][field: Range(0f, 1f)] public float WalkSpeedModif { get; private set; } = 0.5f;
    [field: SerializeField][field: Range(0f, 1f)] public float RunSpeedModif { get; private set; } = 2f;
    [field: SerializeField][field: Range(0f, 1f)] public float CrouchSpeedModif { get; private set; } = 0.1f;
}
