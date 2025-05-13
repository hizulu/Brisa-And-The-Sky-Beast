using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerRideBeastData
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 04/05/2025
 * DESCRIPCI�N: Clase serializable que contiene el modificador de velocidad cuando est� montado en la Bestia.
 * VERSI�N: 1.0
 */

[Serializable]
public class PlayerRideBeastData
{
    [field: SerializeField][field: Range(0f, 3f)] public float RideBeastSpeedModif { get; private set; } = 3f;
}
