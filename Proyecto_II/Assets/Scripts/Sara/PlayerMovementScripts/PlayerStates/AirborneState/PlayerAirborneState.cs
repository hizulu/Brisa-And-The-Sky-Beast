using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerAirborneState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 10/03/2025
 * DESCRIPCI�N: Clase que hereda de PlayerMovementState.
 *              Estado padre que contiene subestados espec�ficos de Player cuando est� en el aire.
 * VERSI�N: 1.0. 
 */
public class PlayerAirborneState : PlayerMovementState
{
    public PlayerAirborneState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Variables AirborneState Y Derivados
    protected bool jumpFinish;
    protected bool isJumping = false;

    protected float jumpTimeElapsed;
    protected float minTimeBeforeDoubleJump = 0.05f;
    protected int maxNumDoubleJump;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        base.Enter();
        jumpTimeElapsed = 0f;
        StartAnimation(stateMachine.Player.PlayerAnimationData.AirborneParameterHash);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        // Si quito "stateMachine.Player.PlayerInput.PlayerActions.Jump.triggered" puedo cortar el salto normal, pero se siente raro porque a veces va con retraso.
        // Si lo pongo, no se har� el doble salto hasta que se acabe el salto normal.
        if (jumpTimeElapsed > minTimeBeforeDoubleJump && stateMachine.Player.PlayerInput.PlayerActions.Jump.triggered && maxNumDoubleJump < 1)
        {
            maxNumDoubleJump++;
            stateMachine.ChangeState(stateMachine.DoubleJumpState);
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        jumpTimeElapsed += Time.deltaTime;
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        Move();
    }

    public override void Exit()
    {
        base .Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.AirborneParameterHash);
    }
    #endregion

    #region M�todos para Sobrescribir de Airborne
    protected virtual void Jump() { }
    protected override void ContactWithGround(Collider collider) { }
    #endregion

    #region M�todos Propios AirborneState
    /// <summary>
    /// M�todo para mover a Player mientras est� en el aire.
    /// </summary>
    protected override void Move()
    {
        Vector2 playerInput = stateMachine.MovementData.MovementInput;
        Vector3 currentVelocity = stateMachine.Player.RbPlayer.velocity;
        Vector3 cameraForward = Camera.main.transform.forward; // Obtenemos la direcci�n hacia delante de la c�mara.
        cameraForward.y = 0; // En el eje Y se ignora para que no apunte hacia el suelo o hacia el cielo.
        cameraForward.Normalize();
        Vector3 movementDirection = (cameraForward * playerInput.y + Camera.main.transform.right * playerInput.x).normalized; // Calcula la direcci�n del movimiento basado en la c�mara.

        float airSpeed = airborneData.AirSpeed; // Variable para la velocidad que se quiere de movimiento en el aire.
        float airControl = airborneData.AirControl; // Variable para determinar si la respuesta al input es m�s inmediata o menos. (Para tener m�s o menos control de movimiento en el aire).
        Vector3 newVelocityInAir;

        if (playerInput == Vector2.zero)
            newVelocityInAir = new Vector3(0f, currentVelocity.y, 0f); // Si no detecta teclas presionadas, solo se mantiene la velocidad en Y.
        else
        {
            newVelocityInAir = new Vector3(movementDirection.x * airSpeed, currentVelocity.y, movementDirection.z * airSpeed); // Si detecta teclas se calcula la nueva velocidad de direcci�n.

            if (IsTouchingWall(movementDirection)) // Si detecta una pared, frena la velocidad en X y en Z para que no se quede pegado.
            {
                newVelocityInAir.x = 0f;
                newVelocityInAir.z = 0f;
            }

            Rotate(movementDirection); // El personaje rota en la direcci�n establecida por el input.
        }

        stateMachine.Player.RbPlayer.velocity = Vector3.Lerp(currentVelocity, newVelocityInAir, airControl * Time.deltaTime);
    }

    /// <summary>
    /// M�todo que resetea a 0 el n�mero de doble saltos realizados.
    /// </summary>
    protected void ResetDoubleJump()
    {
        maxNumDoubleJump = 0;
    }

    /// <summary>
    /// M�todo que devuelve True/False dependiendo si detecta una pared pegada al Player o no.
    /// </summary>
    /// <param name="direction">Vector de direcci�n del Player para detectar si tiene una pared delante</param>
    /// <returns>
    /// Si no detecta pared o no detecta movimiento, de vuelve False.
    /// Si detecta una pared cercana a Player, devuelve True.
    /// </returns>
    private bool IsTouchingWall(Vector3 direction)
    {
        float wallDistance = 0.5f;
        Vector3 posOrigin = stateMachine.Player.transform.position;
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;

        if (horizontalDirection == Vector3.zero)
            return false;

        if (Physics.Raycast(posOrigin, horizontalDirection, out RaycastHit hit, wallDistance, LayerMask.GetMask("Enviroment")))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            return angle > 75f && angle < 105f;
        }

        return false;
    }
    #endregion

    #region M�todo Cancelar Entrada Input
    /// <summary>
    /// M�todo sobrescrito que se ejecuta cuando se cancela la entrada de movimiento.
    /// Si Player est� en el estado de salto o de ca�da, se resetea el input de movimiento a cero.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.CurrentState is PlayerJumpState || stateMachine.CurrentState is PlayerFallState)
            stateMachine.MovementData.MovementInput = Vector2.zero;
    }
    #endregion
}
