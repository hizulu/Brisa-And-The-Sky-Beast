using UnityEngine;

/*
 * NOMBRE CLASE: PlayerMovementData
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 
 * DESCRIPCI�N: Clase que almacena los datos de movimiento del jugador. La direcci�n de entrada y el modificador de velocidad.
 * VERSI�N: 1.0
 */

public class PlayerMovementData
{
    public Vector2 MovementInput { get; set; }
    public float MovementSpeedModifier { get; set; } = 2f;
}
