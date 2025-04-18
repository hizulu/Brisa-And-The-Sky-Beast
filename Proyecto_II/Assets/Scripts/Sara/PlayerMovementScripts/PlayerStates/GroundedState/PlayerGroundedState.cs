using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE SCRIPT: PlayerGroundedState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase que hereda de PlayerMovementState
 * VERSI�N: 1.0.
 */
public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    protected bool isPointed = false;
    private float timePressed = 0f;

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.started += ctx => OnPointedStarted(ctx);
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.canceled += ctx => OnPointedCanceled(ctx);
        EventsManager.CallNormalEvents("AcariciarBestia_Player", AcariciarBestia);
        StartAnimation(stateMachine.Player.PlayerAnimationData.GroundedParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (isPointed)
        {
            timePressed += Time.deltaTime;

            if (timePressed >= 2f)
            {
                stateMachine.ChangeState(stateMachine.PointedBeastState);
                isPointed = false;
            }
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.started -= ctx => OnPointedStarted(ctx);
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.canceled -= ctx => OnPointedCanceled(ctx);
        EventsManager.StopCallNormalEvents("AcariciarBestia_Player", AcariciarBestia);
        StopAnimation(stateMachine.Player.PlayerAnimationData.GroundedParameterHash);
    }

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        stateMachine.Player.PlayerInput.PlayerActions.Run.performed += RunStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Jump.started += JumpStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.performed += CrouchStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Attack.started += AttackStart;
        stateMachine.Player.PlayerInput.PlayerActions.Heal.started += HealPlayer;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        stateMachine.Player.PlayerInput.PlayerActions.Jump.started -= JumpStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Attack.started -= AttackStart;
        stateMachine.Player.PlayerInput.PlayerActions.Heal.started -= HealPlayer;
    }

    protected virtual void OnMove()
    {
        if (stateMachine.Player.PlayerInput.PlayerActions.Run.IsPressed())
            stateMachine.ChangeState(stateMachine.RunState);
        else
            stateMachine.ChangeState(stateMachine.WalkState);
    }

    protected virtual void OnStop()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    protected virtual void RunStarted(InputAction.CallbackContext context)
    {
        if (!IsGrounded())
            return;

        stateMachine.ChangeState(stateMachine.RunState);
    }

    protected virtual void JumpStarted(InputAction.CallbackContext context)
    {
        if (!(stateMachine.CurrentState is PlayerDoubleJumpState || stateMachine.CurrentState is PlayerFallState))
        {
            stateMachine.ChangeState(stateMachine.JumpState);
        }
    }

    protected virtual void CrouchStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.CrouchState);
    }

    protected virtual void AttackStart(InputAction.CallbackContext context)
    {
        // Solo cambiar a Attack01 si no estamos en medio de un combo o ataque
        if (!(stateMachine.CurrentState is PlayerAttack02 || stateMachine.CurrentState is PlayerAttack03))
        {
            stateMachine.ChangeState(stateMachine.Attack01State);
        }
    }

    protected virtual void HealPlayer(InputAction.CallbackContext context)
    {
        if (statsData.CurrentHealth >= statsData.MaxHealth) return; // Si la vida actual est� al m�ximo, no hacemos nada.

        string[] healingItemNames = { "Mango Luminoso", "Baya Voladora" }; // Guardamos en un array los items espec�ficos que curan.

        foreach (string itemName in healingItemNames)
        {
            ItemData healingItem = InventoryManager.Instance.GetItemByName(itemName); // Los buscamos en el inventario por el nombre espec�fico (puesto en el ItemDataSO).

            if (healingItem != null && InventoryManager.Instance.CheckForItem(healingItem)) // Comprobamos que est�n en el inventario.
            {
                stateMachine.HealState.SetHealingItem(healingItem); // Pasamos el valor de curaci�n del item espec�fico que vayamos a comer.
                stateMachine.ChangeState(stateMachine.HealState);
            }
        }

        // TODO : Poner un recuadro o algo en la pantalla para avisar de que est�s curado al completo. (�O simplemente se da por hecho y no pasa nada?).
        Debug.Log("No tienes para curarte");
    }

    protected override void NoContactWithGround(Collider collider)
    {
        if (!IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    private bool IsGrounded()
    {
        //Debug.Log("Se ha ejecutado el m�todo IsGrounded");

        float radius = groundedData.GroundCheckDistance;
        Vector3 groundCheckPosition = stateMachine.Player.GroundCheckCollider.transform.position;
        Collider[] colliders = Physics.OverlapSphere(groundCheckPosition, radius);
        //Debug.Log($"N�mero de colisiones detectadas: {colliders.Length}");

        foreach (Collider collider in colliders)
        {
            string objName = collider.gameObject.name;
            string layerName = LayerMask.LayerToName(collider.gameObject.layer);
            bool isTrigger = collider.isTrigger;

            //Debug.Log($"Detectado: {objName} | Capa: {layerName} | isTrigger: {isTrigger}");

            if (collider.gameObject.layer == LayerMask.NameToLayer("Enviroment") && !collider.isTrigger)
            {
                //Debug.Log("Se ha detectado suelo");
                return true;
            }
        }
        return false;
    }

    private void AcariciarBestia()
    {
        // L�gica de acariciar a la Bestia.
        stateMachine.ChangeState(stateMachine.PetBeastState);
        Debug.Log("Est�s acariciando a la Bestia.");
    }

    protected virtual void OnPointedStarted(InputAction.CallbackContext context)
    {
        isPointed = true;
    }

    protected virtual void OnPointedCanceled(InputAction.CallbackContext context)
    {
        isPointed = false;
        timePressed = 0f;
    }

    protected virtual void OnPointedStateCanceled(InputAction.CallbackContext context)
    {

    }
}
