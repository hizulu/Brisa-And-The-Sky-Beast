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
    public PlayerAirborneData AirborneData { get; }
    public PlayerStatsData StatsData { get; }

    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerRunState RunState { get; }
    public PlayerCrouchState CrouchState { get; }
    public PlayerAttackState AttackState { get; }
    public PlayerComboAttack ComboAttack { get; }
    public PlayerJumpState JumpState { get; }
    public PlayerJumpState DoubleJumpState { get; }
    public PlayerFallState FallState { get; }
    public PlayerLandState LandState { get; }
    public PlayerHalfDeadState HalfDeadState { get; }
    public PlayerFinalDeadState FinalDeadState { get; }

    public PlayerStateMachine(Player player)
    {
        Player = player;

        MovementData = new PlayerMovementData();
        AirborneData = new PlayerAirborneData();
        StatsData = new PlayerStatsData();

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        CrouchState = new PlayerCrouchState(this);
        AttackState = new PlayerAttackState(this);
        ComboAttack = new PlayerComboAttack(this);
        JumpState = new PlayerJumpState(this);
        DoubleJumpState = new PlayerDoubleJumpState(this);
        FallState = new PlayerFallState(this);
        LandState = new PlayerLandState(this);
        HalfDeadState = new PlayerHalfDeadState(this);
        FinalDeadState = new PlayerFinalDeadState(this);
    }
}
