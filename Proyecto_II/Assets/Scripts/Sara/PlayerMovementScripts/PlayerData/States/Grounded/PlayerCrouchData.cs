using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerCrouchData
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase serializable que contiene el modificador de velocidad de caminar en sigilo.
 * VERSI�N: 1.0
 */

[Serializable]

public class PlayerCrouchData
{
    [field: SerializeField][field: Range(0f, 1f)] public float CrouchSpeedModif { get; private set; } = 0.1f;
}
