using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerRideBeastData
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 04/05/2025
 * DESCRIPCIÓN: Clase serializable que contiene el modificador de velocidad cuando está montado en la Bestia.
 * VERSIÓN: 1.0
 */

[Serializable]
public class PlayerRideBeastData
{
    [field: SerializeField][field: Range(0f, 3f)] public float RideBeastSpeedModif { get; private set; } = 3f;
}
