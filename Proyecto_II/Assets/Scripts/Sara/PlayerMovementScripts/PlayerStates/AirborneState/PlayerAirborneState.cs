using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAirborneState : PlayerMovementState
{
    public PlayerAirborneState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    protected bool jumpFinish;
    protected bool isJumping = false;

    protected float jumpTimeElapsed;
    protected float minTimeBeforeDoubleJump = 0.05f;
    protected int maxNumDoubleJump;

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

    protected virtual void Jump()
    {

    }

    protected virtual bool IsGrounded()
    {
        float radius = groundedData.GroundCheckDistance;
        Vector3 groundCheckPosition = stateMachine.Player.GroundCheckCollider.transform.position;

        Collider[] colliders = Physics.OverlapSphere(groundCheckPosition, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enviroment") && !collider.isTrigger)
                return true;
        }
        return false;
    }

    protected override void ContactWithGround(Collider collider)
    {

    }

    protected virtual void FinishJump()
    {

    }

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

        stateMachine.Player.RbPlayer.velocity = Vector3.Lerp(stateMachine.Player.RbPlayer.velocity, newVelocity, airControl * Time.deltaTime);
        //stateMachine.Player.RbPlayer.velocity = newVelocity; // Línea que hay que poner si elimino la variable de "airControl".
        Rotate(movementDirection);
    }


    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.CurrentState is PlayerJumpState || stateMachine.CurrentState is PlayerFallState)
            stateMachine.MovementData.MovementInput = Vector2.zero;
    }

    protected void ResetDoubleJump()
    {
        maxNumDoubleJump = 0;
    }
}
