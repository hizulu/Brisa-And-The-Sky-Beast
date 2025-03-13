using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE SCRIPT: PlayerStateMachine
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que hereda de StateMachine y se encarga de instanciar y dar acceso a los estados del jugador.
 *              Mantiene las referencias a los diferentes estados.
 * VERSIÓN: 1.0. Instanciación del jugador y de los estados.
 */
public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public PlayerMovementData MovementData { get; }

    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }

    public PlayerStateMachine(Player player)
    {
        Player = player;
        MovementData = new PlayerMovementData();

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
    }
}
