using UnityEngine;

/*
 * NOMBRE CLASE: PlayerIdleState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerGroundeState
 *              Subestado que gestiona la acción de estar parado.
 * VERSIÓN: 1.0. 
 */
public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    private float blinkTimer;
    private float blinkInterval;
    private bool isBlinking = false;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        stateMachine.MovementData.MovementSpeedModifier = 0f;
        base.Enter();
        SetRandomBlink();
        StartAnimation(stateMachine.Player.PlayerAnimationData.IdleParameterHash);
        //Debug.Log("Has entrado en el estado de IDLE.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (stateMachine.MovementData.MovementInput == Vector2.zero)
        {
            HandleBlinking();
            return;
        }

        OnMove();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.IdleParameterHash);
        //Debug.Log("Has salido del estado de IDLE.");
    }
    #endregion

    #region Métodos Propios IdleState
    /*
     * Método para cambiar la expresión de Brisa al aterrizar desde muy alto.
     */
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }

    /*
     * Método que gestiona el pestañeo de Player cuando está en Idle.
     */
    private void HandleBlinking()
    {
        blinkTimer += Time.deltaTime;

        if (!isBlinking && blinkTimer >= blinkInterval)
        {
            isBlinking = true;
            blinkTimer = 0f;

            SetFaceProperty(2, new Vector2(0.125f, 0f)); // Semi-cerrados
        }

        if (isBlinking && blinkTimer >= 0.1f && blinkTimer < 0.15f)
        {
            SetFaceProperty(2, new Vector2(0.25f, 0f)); // Cerrados
        }

        if (isBlinking && blinkTimer >= 0.15f)
        {
            isBlinking = false;
            SetFaceProperty(2, new Vector2(0f, 0f)); // Abiertos
            SetRandomBlink();
        }
    }

    private void SetRandomBlink()
    {
        blinkInterval = Random.Range(3f, 8f);
    }
    #endregion
}
