using UnityEngine;

/*
 * NOMBRE CLASE: PlayerJumpState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerAirborneState.
 *              Subestado que gestiona la acción del salto normal.
 * VERSIÓN: 1.0. 
 */
public class PlayerJumpState : PlayerAirborneState
{
    public PlayerJumpState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        jumpFinish = false;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.JumpParameterHash);
        //Debug.Log("Has entrado en el estado de SALTAR.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishAnimation();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        Jump();
    }

    public override void Exit()
    {
        jumpFinish = false;
        isJumping = false;
        //Debug.Log("Desde salto normal: " + maxNumDoubleJump);
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.JumpParameterHash);
        //Debug.Log("Has salido del estado de SALTAR.");
    }
    #endregion

    #region Métodos Propios JumpState
    /*
     * Método que gestiona la física del salto normal
     */
    protected override void Jump()
    {
        if (!isJumping)
        {
            float jumpForce = airborneData.BaseForceJump * (1 + airborneData.JumpData.NormalJumpModif);
            //jumpForce = Mathf.Clamp(jumpForce, 0f, 10f); // Por si queremos poner un tope a la fuerza de salto.
            stateMachine.Player.RbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    /*
     * Método para comprobar que la animación del salto se ha terminado para pasar al siguiente estado requerido.
     */
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("Jump") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            jumpFinish = true;

            if (stateMachine.Player.PlayerInput.PlayerActions.Jump.triggered && maxNumDoubleJump < 1)
            {
                maxNumDoubleJump++;
                stateMachine.ChangeState(stateMachine.DoubleJumpState);
            }
            else
                stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0f, 0f));
        SetFaceProperty(2, new Vector2(0.25f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }
    #endregion
}
