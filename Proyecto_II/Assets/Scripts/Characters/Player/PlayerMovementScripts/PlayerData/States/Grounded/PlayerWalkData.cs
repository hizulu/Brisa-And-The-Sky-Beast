using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerWalkData
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase serializable que contiene el modificador de velocidad de caminar.
 * VERSI�N: 1.0
 */

[Serializable]
public class PlayerWalkData
{
    [field: SerializeField][field: Range(0f, 1f)] public float WalkSpeedModif { get; private set; } = 0.5f;
}
