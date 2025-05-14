using UnityEngine;

/*
 * NOMBRE CLASE: PlayerLandState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerStopState.
 *              Subestado que gestiona la acción de aterrizar.
 * VERSIÓN: 1.0. 
 */
public class PlayerLandState : PlayerStopState
{
    public PlayerLandState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Has entrado en estado de ATERRIZAR");
        StartAnimation(stateMachine.Player.PlayerAnimationData.LandParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishLand();
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("Has salido del estado de ATERRIZAR");
        StopAnimation(stateMachine.Player.PlayerAnimationData.LandParameterHash);
    }
    #endregion

    #region Método Propio LandState
    /*
     * Método para comprobar que la animación de aterrizar se ha terminado para pasar al siguiente estado requerido.
     */
    private void FinishLand()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("Land") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.78f, 0f));
        SetFaceProperty(2, new Vector2(0.375f, 0f));
        SetFaceProperty(3, new Vector2(0.66f, 0f));
    }
    #endregion
}
