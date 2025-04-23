using UnityEngine;

/*
 * NOMBRE CLASE: PlayerMovementData
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que almacena los datos de movimiento del jugador. La dirección de entrada y el modificador de velocidad.
 * VERSIÓN: 1.0
 */

public class PlayerMovementData
{
    public Vector2 MovementInput { get; set; }
    public float MovementSpeedModifier { get; set; } = 2f;
}