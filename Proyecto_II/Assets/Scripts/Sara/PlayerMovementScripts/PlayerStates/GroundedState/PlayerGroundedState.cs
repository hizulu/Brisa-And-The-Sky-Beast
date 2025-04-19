using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerGroundedState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase que hereda de PlayerMovementState y contiene la l�gica b�sica de Player cuando est� en el suelo.
 * VERSI�N: 1.0.
 */
public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    protected bool isPointed = false;
    private float timePressed = 0f;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        base.Enter();
        EventsManager.CallNormalEvents("AcariciarBestia_Player", AcariciarBestia);
        EventsManager.CallNormalEvents("MontarBestia_Player", RideBeast);
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
        EventsManager.StopCallNormalEvents("AcariciarBestia_Player", AcariciarBestia);
        EventsManager.StopCallNormalEvents("MontarBestia_Player", RideBeast);
        StopAnimation(stateMachine.Player.PlayerAnimationData.GroundedParameterHash);
    }
    #endregion

    #region M�todos Suscripci�n Acciones Input System
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        stateMachine.Player.PlayerInput.PlayerActions.Run.performed += RunStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Jump.started += JumpStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.performed += CrouchStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Attack.started += AttackStart;
        stateMachine.Player.PlayerInput.PlayerActions.Heal.started += HealPlayer;
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.started += OnPointedStarted;
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.canceled += OnPointedCanceled;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        stateMachine.Player.PlayerInput.PlayerActions.Jump.started -= JumpStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Attack.started -= AttackStart;
        stateMachine.Player.PlayerInput.PlayerActions.Heal.started -= HealPlayer;
    }
    #endregion

    #region M�todos Gesti�n Movimiento Player
    protected virtual void OnMove()
    {
        if (stateMachine.Player.PlayerInput.PlayerActions.Run.IsPressed())
            stateMachine.ChangeState(stateMachine.RunState);
        else if(stateMachine.Player.PlayerInput.PlayerActions.Crouch.IsPressed())
            stateMachine.ChangeState(stateMachine.CrouchState);
        else
            stateMachine.ChangeState(stateMachine.WalkState);
    }

    protected virtual void OnStop()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
    #endregion

    #region M�todos Suscripci�n de Acciones Input System
    /*
     * M�todo que maneja la acci�n de correr.
     * @param context - Informaci�n sobre la tecla / acci�n que se activa (Left Shift / Run).
     */
    protected virtual void RunStarted(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) // Si no est� tocando suelo, no se cambia a correr.
            return;

        if (stateMachine.CurrentState == stateMachine.CrouchState) // Si el estado actual del jugador es "Crouch", no se cambia a "Run".
            return;

        stateMachine.ChangeState(stateMachine.RunState);
    }

    /*
     * M�todo que maneja la acci�n de sigilo.
     * @param context - Informaci�n sobre la tecla / acci�n que se activa (Left Control / Crouch).
     */
    protected virtual void CrouchStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.CurrentState == stateMachine.RunState) // Si el estado actual del jugador es "Run", no se cambia a "Crouch".
            return;

        stateMachine.ChangeState(stateMachine.CrouchState);
    }

    /*
     * M�todo que maneja la acci�n de atacar.
     * @param context - Informaci�n sobre la tecla / acci�n que se activa (Click izquierdo / Attack).
     */
    protected virtual void AttackStart(InputAction.CallbackContext context)
    {
        if (!stateMachine.Player.PaloBrisa.activeInHierarchy) // Player no puede atacar si no tiene el palo activo en la jerarqu�a (recoger en el juego).
            return;
        else
        {
            // Solo cambiar a Attack01 si no estamos en medio de un combo o ataque.
            if (!(stateMachine.CurrentState is PlayerAttack02 || stateMachine.CurrentState is PlayerAttack03))
                stateMachine.ChangeState(stateMachine.Attack01State);
        }
    }

    /*
     * M�todo que maneja la acci�n de curar.
     * @param context - Informaci�n sobre la tecla / acci�n que se activa (H / Heal).
     */
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
    }

    /*
     * M�todo que maneja la acci�n de saltar.
     * @param context - Informaci�n sobre la tecla / acci�n que se activa (Barra espaciadora / Jump).
     */
    protected virtual void JumpStarted(InputAction.CallbackContext context)
    {
        // Solo salta si el estado actual del jugador no es ni Doble Salto ni Caer.
        if (!(stateMachine.CurrentState is PlayerDoubleJumpState || stateMachine.CurrentState is PlayerFallState))
            stateMachine.ChangeState(stateMachine.JumpState);
    }
    #endregion

    #region M�todos Comprobar Si Player Toca Suelo
    /*
     * M�todo que comprueba si ya no est� en el suelo, si es as�, cambia el estado a ca�da.
     * Se llama cuando el jugador pierde contacto con el suelo. 
     * @param collider - El collider que ha perdido el contacto con el suelo.
     */
    protected override void NoContactWithGround(Collider collider)
    {
        if (!IsGrounded())
            stateMachine.ChangeState(stateMachine.FallState);
    }

    /*
     * M�todo que devuelve True/False para comprobar si Player ha tocado suelo o no.
     */
    private bool IsGrounded()
    {
        float radius = groundedData.GroundCheckDistance;
        Vector3 groundCheckPosition = stateMachine.Player.GroundCheckCollider.transform.position;
        Collider[] colliders = Physics.OverlapSphere(groundCheckPosition, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enviroment") && !collider.isTrigger)
                return true;
        }
        return false;
    }
    #endregion

    #region M�todos de Interacci�n con la Bestia
    private void AcariciarBestia()
    {
        // L�gica de acariciar a la Bestia.
        stateMachine.ChangeState(stateMachine.PetBeastState);
        Debug.Log("Est�s acariciando a la Bestia.");
    }

    private void HealBeast()
    {
        Debug.Log("Est�s sanando a la Bestia");
    }

    private void RideBeast()
    {
        stateMachine.ChangeState(stateMachine.RideBeastState);
    }
    #endregion

    #region M�todos Pasar a PointedBeast
    protected virtual void OnPointedStarted(InputAction.CallbackContext context)
    {
        isPointed = true;
    }

    protected virtual void OnPointedCanceled(InputAction.CallbackContext context)
    {
        isPointed = false;
        timePressed = 0f;
    }

    protected virtual void OnPointedStateCanceled(InputAction.CallbackContext context) { }
    #endregion
}