using UnityEngine;

/*
 * NOMBRE CLASE: PlayerDoubleJumpState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 
 * DESCRIPCIÓN: Clase que hereda de PlayerAirborneState
 * VERSIÓN: 1.0. 
 */
public class PlayerDoubleJumpState : PlayerAirborneState
{
    public PlayerDoubleJumpState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Has entrado en el estado de DOBLE-SALTO");
        StartAnimation(stateMachine.Player.PlayerAnimationData.DoubleJumpParameterHash);
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
        base.Exit();
        //Debug.Log("Has salido del estado de DOBLE-SALTO");
        StopAnimation(stateMachine.Player.PlayerAnimationData.DoubleJumpParameterHash);
    }
    #endregion

    #region Métodos Propios DoubleJumpState
    /*
     * Método que gestiona la física del doble salto.
     */
    protected override void Jump()
    {
        if(!isJumping)
        {
            // Este if es necesario para que, si está cayendo, la fuerza negativa en Y no contrarreste el impulso.
            if (stateMachine.PreviousState is PlayerFallState) // Asignamos la velocidad en Y en 0, para que se aplique bien la fuerza del doble salto.
                stateMachine.Player.RbPlayer.velocity = new Vector3(stateMachine.Player.RbPlayer.velocity.x, 0f, stateMachine.Player.RbPlayer.velocity.z);

            float jumpForce = airborneData.BaseForceJump * (1 + airborneData.JumpData.DoubleJumpModif);
            //jumpForce = Mathf.Clamp(jumpForce, 0f, 10f); // Por si queremos poner un tope a la fuerza de salto.
            stateMachine.Player.RbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    /*
     * Método para comprobar que la animación del doble salto se ha terminado para pasar al siguiente estado requerido.
     */
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("DoubleJump") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isJumping = false;
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }
    #endregion
}
