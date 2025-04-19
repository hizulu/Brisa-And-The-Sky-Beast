/*
 * NOMBRE CLASE: PlayerStateMachine
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que hereda de StateMachine y se encarga de instanciar y dar acceso a los estados del jugador.
 *              Mantiene las referencias a los diferentes estados.
 * VERSIÓN: 1.0. Instanciación del jugador y de los estados.
 */
public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    #region Datas
    public PlayerMovementData MovementData { get; }
    public PlayerAirborneData AirborneData { get; }
    public PlayerStatsData StatsData { get; }
    #endregion

    #region Movement States
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerRunState RunState { get; }
    public PlayerCrouchState CrouchState { get; }
    public PlayerAttackState AttackState { get; }
    public PlayerAttack01 Attack01State { get; }
    public PlayerAttack02 Attack02State { get; }
    public PlayerAttack03 Attack03State { get; }
    public PlayerTakeDamageState TakeDamageState { get; }
    public PlayerHealState HealState { get; }
    public PlayerJumpState JumpState { get; }
    public PlayerDoubleJumpState DoubleJumpState { get; }
    public PlayerFallState FallState { get; }
    public PlayerLandState LandState { get; }
    public PlayerHalfDeadState HalfDeadState { get; }
    public PlayerFinalDeadState FinalDeadState { get; }
    public PlayerPetBeastState PetBeastState { get; }
    public PlayerPointedBeastState PointedBeastState { get; }
    public PlayerRideBeastState RideBeastState { get; }
    public PlayerPickUpState PickUpState { get; }
    #endregion

    /*
     * Constructor de la máquina de estados del Player.
     * Inicializa los estados para dejarlos preparados para los cambios de estado.
     * @param1 player - Recibe una referencia del Player para poder acceder a su información.
     */
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
        Attack01State = new PlayerAttack01(this);
        Attack02State = new PlayerAttack02(this);
        Attack03State = new PlayerAttack03(this);
        TakeDamageState = new PlayerTakeDamageState(this);
        HealState = new PlayerHealState(this);
        JumpState = new PlayerJumpState(this);
        DoubleJumpState = new PlayerDoubleJumpState(this);
        FallState = new PlayerFallState(this);
        LandState = new PlayerLandState(this);
        HalfDeadState = new PlayerHalfDeadState(this);
        FinalDeadState = new PlayerFinalDeadState(this);
        PetBeastState = new PlayerPetBeastState(this);
        PointedBeastState = new PlayerPointedBeastState(this);
        RideBeastState = new PlayerRideBeastState(this);
        PickUpState = new PlayerPickUpState(this);
    }
}