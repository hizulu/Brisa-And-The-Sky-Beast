using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerAirborneState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerMovementState.
 *              Estado padre que contiene subestados específicos de Player cuando está en el aire.
 * VERSIÓN: 1.0. 
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

    #region Métodos Base de la Máquina de Estados
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
        // Si lo pongo, no se hará el doble salto hasta que se acabe el salto normal.
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

    #region Métodos para Sobrescribir de Airborne
    protected virtual void Jump() { }
    protected override void ContactWithGround(Collider collider) { }
    #endregion

    #region Métodos Propios AirborneState
    /*
     * Método para mover a Player mientras está en el aire.
     */
    protected override void Move()
    {
        Vector2 input = stateMachine.MovementData.MovementInput;

        if (input == Vector2.zero)
        {
            stateMachine.Player.RbPlayer.velocity = new Vector3(0, stateMachine.Player.RbPlayer.velocity.y, 0);
            return;
        }

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 movementDirection = (cameraForward * input.y + Camera.main.transform.right * input.x).normalized;

        float airSpeed = airborneData.AirSpeed;
        float airControl = airborneData.AirControl;

        Vector3 newVelocity = new Vector3(movementDirection.x * airSpeed, stateMachine.Player.RbPlayer.velocity.y, movementDirection.z * airSpeed);

        if (IsTouchingWall(movementDirection))
        {
            newVelocity.x = 0;
            newVelocity.z = 0;
        }

        stateMachine.Player.RbPlayer.velocity = Vector3.Lerp(stateMachine.Player.RbPlayer.velocity, newVelocity, airControl * Time.deltaTime);
        //stateMachine.Player.RbPlayer.velocity = newVelocity; // Línea que hay que poner si elimino la variable de "airControl".
        Rotate(movementDirection);
    }

    /*
     * Método que resetea a 0 el número de doble saltos realizados.
     */
    protected void ResetDoubleJump()
    {
        maxNumDoubleJump = 0;
    }

    /*
     * Método que devuelve True/False dependiendo si detecta una pared pegada al Player o no.
     */
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

    #region Método Comprobar si Player Toca Suelo
    /*
     * Método que devuelve True/False para comprobar si Player ha tocado suelo o no.
     */
    protected virtual bool IsGrounded()
    {
        Vector3 boxCenter = stateMachine.Player.GroundCheckCollider.transform.position;
        Vector3 boxHalfExtents = new Vector3(0.25f, 0.05f, 0.25f); // Ancho, altura pequeñita, profundidad
        Quaternion boxOrientation = Quaternion.identity; // No rotado, si quieres rotarlo puedes poner la rotación de tu jugador
        LayerMask groundMask = LayerMask.GetMask("Enviroment");

        bool isGrounded = Physics.CheckBox(boxCenter, boxHalfExtents, boxOrientation, groundMask, QueryTriggerInteraction.Ignore);

        return isGrounded;
    }
    #endregion

    #region Método Cancelar Entrada Input
    /*
     * Método sobrescrito que se ejecuta cuando se cancela la entrada de movimiento.
     * Si Player está en el estado de salto o de caída, se resetea el input de movimiento a cero.
     */
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.CurrentState is PlayerJumpState || stateMachine.CurrentState is PlayerFallState)
            stateMachine.MovementData.MovementInput = Vector2.zero;
    }
    #endregion
}
