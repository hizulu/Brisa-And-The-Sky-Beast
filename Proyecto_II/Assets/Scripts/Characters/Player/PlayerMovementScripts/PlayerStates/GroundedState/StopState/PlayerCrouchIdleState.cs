using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerCrouchIdleState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 16/05/2025
 * DESCRIPCI�N: Clase que hereda de PlayerStopState y contiene la l�gica de Player cuando est� en modo sigilo pero sin desplazarse.
 * VERSI�N: 1.0.
 */

public class PlayerCrouchIdleState : PlayerStopState
{
    public PlayerCrouchIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    #region Corrutinas
    private Coroutine repeatCrouchAnimation;
    private Coroutine changeFaceExpresionIdleCrouch;
    #endregion

    bool isWaitingToRepeat;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        isWaitingToRepeat = false;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.CrouchPoseParameterHash);
        changeFaceExpresionIdleCrouch = stateMachine.Player.StartCoroutine(LookAroundFace());
        //Debug.Log("Has entrado en el estado de AGACHADO IDLE");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (stateMachine.MovementData.MovementInput != Vector2.zero)
            stateMachine.ChangeState(stateMachine.CrouchState);
        else
            FinishAnimation();
    }

    public override void Exit()
    {
        base.Exit();
        if (repeatCrouchAnimation != null)
            stateMachine.Player.StopCoroutine(repeatCrouchAnimation);

        if (changeFaceExpresionIdleCrouch != null)
            stateMachine.Player.StopCoroutine(changeFaceExpresionIdleCrouch);

        StopAnimation(stateMachine.Player.PlayerAnimationData.CrouchPoseParameterHash);
        //Debug.Log("Has salido del estado de AGACHADO IDLE");
    }
    #endregion

    #region M�todos Propios IdleCrouch
    /// <summary>
    /// M�todo sobreescrito que ejecuta la l�gica cuando la animaci�n de este estado se termina.
    /// Si est� esperando a repetirse la animaci�n, no hace nada. (Para no ejecutar m�ltiples veces la corrutina.
    /// Si ha terminado la animaci�n, ejecuta la corrutina de repetirse la animaci�n.
    /// </summary>
    protected override void FinishAnimation()
    {
        if (isWaitingToRepeat) return;

        if(stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("PoseCrouch") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isWaitingToRepeat = true;
            ChangeFacePlayer();
            repeatCrouchAnimation = stateMachine.Player.StartCoroutine(RepeatAnimationCrouchPose());
        }
    }

    /// <summary>
    /// Corrutina para que Player vuelva a realizar la animaci�n de observar hacia los lados despu�s de un tiempo determinado aleatorio.
    /// Llama a la corrutina de cambiar la expresi�n facial para que vayan en sincron�a.
    /// </summary>
    /// <returns>Corrutina con la secuencia de pasos que debe realizar.</returns>
    private IEnumerator RepeatAnimationCrouchPose()
    {
        float maxTimeToRepeat = Random.Range(2f, 5f);
        Debug.Log(maxTimeToRepeat);
        yield return new WaitForSeconds(maxTimeToRepeat);
        stateMachine.Player.AnimPlayer.Play("PoseCrouch", 0, 0f); // Fuerza a reproducir la animaci�n desde el princpio.
        changeFaceExpresionIdleCrouch = stateMachine.Player.StartCoroutine(LookAroundFace());
        yield return new WaitForSeconds(0.1f);
        isWaitingToRepeat = false;
    }

    /// <summary>
    /// M�todo sobreescrito que establece la expresi�n de Player al comenzar este estado.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.44f, 0f));
        SetFaceProperty(2, new Vector2(0.875f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }

    /// <summary>
    /// Corrutina para cambiar la expresi�n de Brisa cuando est� vigilando alrededor en estado de sigilo.
    /// </summary>
    /// <returns>Corrutina con la secuencia de pasos que debe realizar.</returns>
    private IEnumerator LookAroundFace()
    {
        yield return new WaitForSeconds(0.7f);
        SetFaceProperty(1, new Vector2(0.33f, 0f));
        SetFaceProperty(2, new Vector2(0.75f, 0f));
        SetFaceProperty(3, new Vector2(0.66f, 0f));
        yield return new WaitForSeconds(1.2f);
        SetFaceProperty(1, new Vector2(0.33f, 0f));
        SetFaceProperty(2, new Vector2(0.625f, 0f));
        SetFaceProperty(3, new Vector2(0.66f, 0f));
    }

    /// <summary>
    /// M�todo sobreescrito para cambiar a IdleState si se deja de pulsar Control.
    /// </summary>
    /// <param name="context"></param>
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        OnStop();
    }
    #endregion
}
