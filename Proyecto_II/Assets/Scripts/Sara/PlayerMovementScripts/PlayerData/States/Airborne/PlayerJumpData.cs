using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerJumpData
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/03/2025
 * DESCRIPCIÓN: Clase que almacena los modificadores de salto del jugador, tanto para el salto normal como para el doble salto.
 * VERSIÓN: 1.0
 */
[Serializable]
public class PlayerJumpData
{
    [field: SerializeField][field: Range(0f, 1f)] public float NormalJumpModif { get; private set; } = 0f;
    [field: SerializeField][field: Range(0f, 1f)] public float DoubleJumpModif { get; private set; } = 0.2f;
}
