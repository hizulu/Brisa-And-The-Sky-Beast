using UnityEngine;

/*
 * NOMBRE CLASE: PlayerFallState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 10/03/2025
 * DESCRIPCI�N: Clase que hereda de PlayerAirborneState.
 *              Subestado que gestiona la acci�n de caer.
 * VERSI�N: 1.0. 
 */
public class PlayerFallState : PlayerAirborneState
{
    public PlayerFallState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Variables
    #region Variables Obtener Velocidad Eje Y
    private float playerCurrentVelocityInY;
    private float maxVelocity = 0f;
    private float maxVelocityHardLand = 10f;
    #endregion

    #region Variables Incrementar Velocidad Ca�da
    private float fallSpeed = 0f;
    private float gravityAcceleration = 9.8f;
    private float maxSpeed = 20f;
    #endregion

    #region Variables Forzar Aterrizaje
    private float timeWithoutVelocityInY = 0f;
    private float maxTimeStuck = 2f;
    #endregion
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        //timeInFall = 0f;
        base.Enter();
        //Debug.Log("Desde entrada de ca�da: " + maxNumDoubleJump);
        StartAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.Fall);
        // Debug.Log("Has entrado en el estado de CAYENDO");
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

        if (CheckIfPlayerIsStuckInFallState())
            ForceLandPlayer();

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
        //Debug.Log("Desde salida de ca�da: " + maxNumDoubleJump);
        StopAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        //Debug.Log("Has salido del estado de CAYENDO");
    }
    #endregion

    #region M�todos Propios LandState
    /// <summary>
    /// Obtener la velocidad en Y (para poder trabjar con ello en otros m�todos).
    /// </summary>
    /// <returns>Velocidad del rigidbody de Player en el eje Y.</returns>
    private float GetYVelocity()
    {
        return stateMachine.Player.RbPlayer.velocity.y;
    }

    /// <summary>
    /// Obtiene la velocidad de ca�da de Player (eje Y) y lo convierte en valor absoluto.
    /// Registra la velocidad de ca�da m�s alta alcanzada de Player y se asigna a la variable "maxVelocity" para compararla posteriormente.
    /// </summary>
    /// <param name="_velocityY">Velocidad en el eje Y de Player cuando cae.</param>
    private void GetFallVelocity(float _velocityY)
    {
        float currentFallVelocity = Mathf.Abs(_velocityY);

        if (currentFallVelocity > maxVelocityHardLand)
            maxVelocity = currentFallVelocity;
    }

    /// <summary>
    /// L�gica de transiciones cuando Player aterriza en el suelo.
    /// Resetea la posibilidad de hacer un doble salto.
    /// Si la velocidad m�xima alcanzada es mayor que el l�mite establecido, pasa a un aterrizaje duro, sino, hace un aterrizaje normal.
    /// </summary>
    private void LandInGround()
    {
        ResetDoubleJump();

        if (maxVelocity < maxVelocityHardLand)
            stateMachine.ChangeState(stateMachine.LandState);
        else
            stateMachine.ChangeState(stateMachine.HardLandState);
    }

    /// <summary>
    /// M�todo que incrementa la velocidad de ca�da si Player est� cayendo.
    /// </summary>
    private void IncreaseFallSpeed()
    {
        fallSpeed = Mathf.Min(fallSpeed + gravityAcceleration * Time.deltaTime, maxSpeed);
        stateMachine.Player.RbPlayer.velocity = new Vector3(stateMachine.Player.RbPlayer.velocity.x, -fallSpeed, stateMachine.Player.RbPlayer.velocity.z);
    }

    /// <summary>
    /// M�todo para detectar si Player se ha quedado atrapado en FallState.
    /// Si detecta que su velocidad en Y es 0 m�s de 2 segundos, fuerza un aterrizaje para poder salir de FallState.
    /// </summary>
    private bool CheckIfPlayerIsStuckInFallState()
    {
        if (Mathf.Approximately(playerCurrentVelocityInY, 0f))
        {
            timeWithoutVelocityInY += Time.deltaTime;

            if (timeWithoutVelocityInY >= maxTimeStuck)
            {
                Debug.Log("Player est� atascado en FallState, se va a forzar que aterrice.");
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// M�todo que fuerza el aterrizaje de Player.
    /// Empuja al Player un poco hacia atr�s y cambia al estado de LandState.
    /// </summary>
    private void ForceLandPlayer()
    {
        timeWithoutVelocityInY = 0f;
        Vector3 pushBackDirection = -stateMachine.Player.transform.forward;
        float pushBackForce = 10f;
        Vector3 currentVelocity = stateMachine.Player.RbPlayer.velocity;
        Vector3 newVelocity = new Vector3(pushBackDirection.x * pushBackForce, Mathf.Min(0f, currentVelocity.y), pushBackDirection.z * pushBackForce);
        stateMachine.Player.RbPlayer.velocity = newVelocity;
        ResetDoubleJump();
        stateMachine.ChangeState(stateMachine.LandState);
    }

    /// <summary>
    /// M�todo sobreescrito para cambiar la expresi�n de Brisa cuando est� cayendo.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.11f, 0f));
        SetFaceProperty(2, new Vector2(0.875f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }
    #endregion
}
