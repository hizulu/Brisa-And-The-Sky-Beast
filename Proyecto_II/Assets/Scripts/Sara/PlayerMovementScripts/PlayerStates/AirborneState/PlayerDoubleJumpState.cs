using UnityEngine;

/*
 * NOMBRE CLASE: PlayerDoubleJumpState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 03/04/2025
 * DESCRIPCI�N: Clase que hereda de PlayerAirborneState.
 *              Subestado que gestiona la acci�n del doble salto.
 * VERSI�N: 1.0. 
 */
public class PlayerDoubleJumpState : PlayerAirborneState
{
    public PlayerDoubleJumpState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Has entrado en el estado de DOBLE-SALTO");
        StartAnimation(stateMachine.Player.PlayerAnimationData.DoubleJumpParameterHash);
    }

    public override void HandleInput()
    {
        base.HandleInput();
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

    #region M�todos Propios DoubleJumpState
    /// <summary>
    /// M�todo sobreescrito que gestiona la f�sica para realizar un doble salto.
    /// </summary>
    protected override void Jump()
    {
        if(!isJumping)
        {
            // Este if es necesario para que, si est� cayendo, la fuerza negativa en Y no contrarreste el impulso.
            if (stateMachine.PreviousState is PlayerFallState) // Asignamos la velocidad en Y en 0, para que se aplique bien la fuerza del doble salto.
                stateMachine.Player.RbPlayer.velocity = new Vector3(stateMachine.Player.RbPlayer.velocity.x, 0f, stateMachine.Player.RbPlayer.velocity.z);

            float jumpForce = airborneData.BaseForceJump * (1 + airborneData.JumpData.DoubleJumpModif);
            //jumpForce = Mathf.Clamp(jumpForce, 0f, 10f); // Por si queremos poner un tope a la fuerza de salto.
            stateMachine.Player.RbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    /// <summary>
    /// M�todo para comprobar que la animaci�n del doble salto se ha terminado para pasar a fallState.
    /// </summary>
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("DoubleJump") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isJumping = false;
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    /// <summary>
    /// M�todo sobreescrito para cambiar la expresi�n de Brisa cuando est� haciendo un doble salto.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.555f, 0f));
        SetFaceProperty(2, new Vector2(0.375f, 0f));
        SetFaceProperty(3, new Vector2(0.66f, 0f));
    }
    #endregion
}
