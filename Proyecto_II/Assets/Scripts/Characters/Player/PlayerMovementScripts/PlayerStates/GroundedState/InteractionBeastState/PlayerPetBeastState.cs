using UnityEngine;

/*
 * NOMBRE CLASE: PlayerPetBeastState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 14/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerInteractionState.
 *              Subestado que gestiona la acción de acariciar a la Bestia.
 * VERSIÓN: 1.0. 
 */
public class PlayerPetBeastState : PlayerInteractionState
{
    public PlayerPetBeastState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    private bool petBeastFinish;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        petBeastFinish = false;
        base.Enter();
        AlignPlayerToBeast();
        //Debug.Log("Has entrado en estado de Acariciar a la Bestia.");
        StartAnimation(stateMachine.Player.PlayerAnimationData.PetBeastParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishAnimation();
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("Has salido del estado de Acariciar a la Bestia.");
        StopAnimation(stateMachine.Player.PlayerAnimationData.PetBeastParameterHash);
    }
    #endregion

    #region Métodos Propios PetBeastState
    /// <summary>
    /// Método sobreescrito para comprobar que la animación de acariciar se ha terminado para pasar a idle.
    /// </summary>
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("PetBeast") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            petBeastFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    /// <summary>
    /// Método para orientar a Player hacia donde esté la Bestia para acariciarle.
    /// </summary>
    private void AlignPlayerToBeast()
    {
        Transform beastPosition = stateMachine.Player.Beast.transform;
        Transform playerPosition = stateMachine.Player.transform;

        Vector3 orientationToBeast = (beastPosition.position - playerPosition.position);
        orientationToBeast.y = 0f;

        if (orientationToBeast != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(orientationToBeast);
            playerPosition.rotation = targetRotation;
        }
    }

    /// <summary>
    /// Método sobreescrito para cambiar la expresión de Brisa cuando está acariciando a la Bestia.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.885f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }
    #endregion
}