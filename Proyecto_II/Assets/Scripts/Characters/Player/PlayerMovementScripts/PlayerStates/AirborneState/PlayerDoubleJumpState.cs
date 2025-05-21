using UnityEngine;

/*
 * NOMBRE CLASE: PlayerDoubleJumpState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 03/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerAirborneState.
 *              Subestado que gestiona la acción del doble salto.
 * VERSIÓN: 1.0. 
 */
public class PlayerDoubleJumpState : PlayerAirborneState
{
    public PlayerDoubleJumpState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Variables
    private ParticleSystem swirlEffect;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Has entrado en el estado de DOBLE-SALTO");
        StartAnimation(stateMachine.Player.PlayerAnimationData.DoubleJumpParameterHash);
        stateMachine.Player.DoubleJumpEffect.gameObject.SetActive(true);
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.DoubleJump);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        DoubleJumpEffect();
        FinishAnimation();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        Jump();
    }

    public override void Exit()
    {
        stateMachine.Player.DoubleJumpEffect.gameObject.SetActive(false);
        base.Exit();
        //Debug.Log("Has salido del estado de DOBLE-SALTO");
        StopAnimation(stateMachine.Player.PlayerAnimationData.DoubleJumpParameterHash);
    }
    #endregion

    #region Métodos Propios DoubleJumpState
    /// <summary>
    /// Método sobreescrito que gestiona la física para realizar un doble salto.
    /// </summary>
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

    /// <summary>
    /// Método para comprobar que la animación del doble salto se ha terminado para pasar a fallState.
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
    /// Método que gestiona el efecto visual del doble salto.
    /// Accede a su material y modica el offset para hacer que gire.
    /// </summary>
    private void DoubleJumpEffect()
    {

        MeshRenderer meshRenderEffect = stateMachine.Player.DoubleJumpEffect;
        Material materialEffect = meshRenderEffect.material;

        Vector2 currentOffset = materialEffect.mainTextureOffset;
        float speed = -2f;
        Vector2 newOffset = currentOffset + new Vector2(speed * Time.deltaTime, 0f); // Para que se vea cómo gira.

        materialEffect.mainTextureOffset = newOffset;
    }

    /// <summary>
    /// Método sobreescrito para cambiar la expresión de Brisa cuando está haciendo un doble salto.
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
