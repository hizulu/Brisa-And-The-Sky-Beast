using UnityEngine;

/*
 * NOMBRE CLASE: PlayerFallState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerAirborneState.
 *              Subestado que gestiona la acción de caer.
 * VERSIÓN: 1.0. 
 */
public class PlayerFallState : PlayerAirborneState
{
    public PlayerFallState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Variables
    private float fallSpeed = 0f;
    private float gravityAcceleration = 9.8f;
    private float maxSpeed = 20f;

    private float timeInFall = 0f;
    private float maxTime = 1f;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        timeInFall = 0f;
        base.Enter();
        //Debug.Log("Desde entrada de caída: " + maxNumDoubleJump);
        StartAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        //Debug.Log("Has entrado en el estado de CAYENDO");
    }

    public override void HandleInput()
    {
        if (maxNumDoubleJump == 0 && stateMachine.Player.PlayerInput.PlayerActions.Jump.triggered && jumpTimeElapsed > minTimeBeforeDoubleJump)
        {
            maxNumDoubleJump++; // Permite el doble salto solo una vez
            stateMachine.ChangeState(stateMachine.DoubleJumpState);
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        timeInFall += Time.deltaTime;
        LandInGround();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        IncreaseFallSpeed();
    }

    public override void Exit()
    {
        base.Exit();
        fallSpeed = 0f;
        //Debug.Log("Desde salida de caída: " + maxNumDoubleJump);
        StopAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        //Debug.Log("Has salido del estado de CAYENDO");
    }
    #endregion

    #region Métodos Propios LandState
    /*
     * Método para comprobar si ha tocado el suelo después de estar en el aire para aterrizar.
     * Resetea la posibilidad de hacer un doble salto.
     */
    private void LandInGround()
    {
        if (IsGrounded())
        {
            //Debug.Log("Pasas a ATERRIZAR");
            ResetDoubleJump();

            if (timeInFall <  maxTime)
                stateMachine.ChangeState(stateMachine.LandState);
            else
                stateMachine.ChangeState(stateMachine.HardLandState);
        }
    }

    /*
     * Método que incrementa la velocidad de caída si Player está cayendo.
     */
    private void IncreaseFallSpeed()
    {
        fallSpeed = Mathf.Min(fallSpeed + gravityAcceleration * Time.deltaTime, maxSpeed);
        stateMachine.Player.RbPlayer.velocity = new Vector3(stateMachine.Player.RbPlayer.velocity.x, -fallSpeed, stateMachine.Player.RbPlayer.velocity.z);
    }
    #endregion
}
