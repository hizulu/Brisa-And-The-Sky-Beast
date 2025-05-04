using UnityEngine;

/*
 * NOMBRE CLASE: PlayerRideBeastState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 18/04/2025
 * DESCRIPCI�N: Clase que hereda de PlayerInteractionState.
 *              Subestado que gestiona la acci�n de montar a la Bestia.
 * VERSI�N: 1.0. 
 */
public class PlayerRideBeastState : PlayerInteractionState
{
    public PlayerRideBeastState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Variables
    private float blinkTimer;
    private float blinkInterval;
    private bool isBlinking = false;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        stateMachine.MovementData.MovementSpeedModifier = groundedData.RideBeastData.RideBeastSpeedModif;
        SetRandomBlink();
        base.Enter();
        Debug.Log("Has entrado en el estado de MONTAR A LA BESTIA");
        StartAnimation(stateMachine.Player.PlayerAnimationData.RideBeastParameterHash);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void UpdateLogic()
    {
        //Debug.Log("Est�s en el estado de MONTAR A LA BESTIA");
        base.UpdateLogic();
        HandleBlinking();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Has salido del estado de MONTAR A LA BESTIA");
        StopAnimation(stateMachine.Player.PlayerAnimationData.RideBeastParameterHash);
    }
    #endregion

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.885f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }

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
}
