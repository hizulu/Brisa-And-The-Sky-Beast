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
    private float playerCurrentVelocityInY;
    private float maxVelocity = 0f;
    private float maxVelocityHardLand = 10f;
    //private float timeInFall = 0f;
    //private float maxTime = 1f;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        //timeInFall = 0f;
        base.Enter();
        //Debug.Log("Desde entrada de caída: " + maxNumDoubleJump);
        StartAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        Debug.Log("Has entrado en el estado de CAYENDO");
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (maxNumDoubleJump == 0 && stateMachine.Player.PlayerInput.PlayerActions.Jump.triggered && jumpTimeElapsed > minTimeBeforeDoubleJump)
        {
            maxNumDoubleJump++; // Permite el doble salto solo una vez
            stateMachine.ChangeState(stateMachine.DoubleJumpState);
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        playerCurrentVelocityInY = GetYVelocity();

        GetFallVelocity(playerCurrentVelocityInY);

        CheckIfPlayerIsStuckInFallState();
        Debug.Log(playerCurrentVelocityInY);

        if (IsGrounded())
            LandInGround();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        //IncreaseFallSpeed();
    }

    public override void Exit()
    {
        base.Exit();
        fallSpeed = 0f;
        maxVelocity = 0f;
        //Debug.Log("Desde salida de caída: " + maxNumDoubleJump);
        StopAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        //Debug.Log("Has salido del estado de CAYENDO");
    }
    #endregion

    #region Métodos Propios LandState
    private float GetYVelocity()
    {
        return stateMachine.Player.RbPlayer.velocity.y;
    }

    /*
     * Método para obtener la velocidad de caída.
     */
    private void GetFallVelocity(float _velocityY)
    {
        float currentFallVelocity = Mathf.Abs(_velocityY); // Obtener el valor absoluto de la velocidad en Y.

        if (currentFallVelocity > maxVelocityHardLand)
            maxVelocity = currentFallVelocity;
    }

    /*
     * Método para comprobar si ha tocado el suelo después de estar en el aire para aterrizar.
     * Resetea la posibilidad de hacer un doble salto.
     */
    private void LandInGround()
    {
        ResetDoubleJump();

        if (maxVelocity < maxVelocityHardLand)
            stateMachine.ChangeState(stateMachine.LandState);
        else
            stateMachine.ChangeState(stateMachine.HardLandState);
    }

    /*
     * Método que incrementa la velocidad de caída si Player está cayendo.
     */
    private void IncreaseFallSpeed()
    {
        fallSpeed = Mathf.Min(fallSpeed + gravityAcceleration * Time.deltaTime, maxSpeed);
        stateMachine.Player.RbPlayer.velocity = new Vector3(stateMachine.Player.RbPlayer.velocity.x, -fallSpeed, stateMachine.Player.RbPlayer.velocity.z);
    }

    /*
     * Método para detectar si Player se ha quedado atrapado en FallState.
     * Si detecta que su velocidad en Y es 0 más de 2 segundos, fuerza un aterrizaje para poder salir de FallState.
     */
    private float timeWithoutVelocityInY = 0f;
    private float maxTimeStuck = 2f;
    private void CheckIfPlayerIsStuckInFallState()
    {
        if (Mathf.Approximately(playerCurrentVelocityInY, 0f))
        {
            timeWithoutVelocityInY += Time.deltaTime;

            if (timeWithoutVelocityInY >= maxTimeStuck)
            {
                Debug.Log("Player está atascado en FallState, se va a forzar que aterrice.");
                ForceLandPlayer();
            }
        }
        else
            timeWithoutVelocityInY = 0f;
    }

    /*
     * Método que fuerza el aterrizaje de Player.
     * Empuja al Player un poco hacia atrás y cambia al estado de LandState.
     */
    private void ForceLandPlayer()
    {
        Vector3 pushBackDirection = -stateMachine.Player.transform.forward;
        float pushBackForce = 5f;
        Vector3 currentVelocity = stateMachine.Player.RbPlayer.velocity;
        Vector3 newVelocity = new Vector3(pushBackDirection.x * pushBackForce, Mathf.Min(0f, currentVelocity.y), pushBackDirection.z * pushBackForce);
        stateMachine.Player.RbPlayer.velocity = newVelocity;
        ResetDoubleJump();
        stateMachine.ChangeState(stateMachine.LandState);
    }

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.11f, 0f));
        SetFaceProperty(2, new Vector2(0.875f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }
    #endregion
}
