using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerRunData
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase serializable que contiene el modificador de velocidad de correr.
 * VERSIÓN: 1.0
 */

[Serializable]
public class PlayerRunData
{
    [field: SerializeField][field: Range(0f, 1f)] public float RunSpeedModif { get; private set; } = 2f;
}
