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
    private float rightButtontimePressed = 0f;

    protected ItemData healIncreaseSpecificItem;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        base.Enter();
        EventsManager.CallNormalEvents("AcariciarBestia_Player", AcariciarBestia);
        EventsManager.CallNormalEvents("SanarBestia_Player", HealBeast);
        EventsManager.CallNormalEvents("MontarBestia_Player", RideBeast);
        StartAnimation(stateMachine.Player.PlayerAnimationData.GroundedParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        ChangeToPointedState();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        EventsManager.StopCallNormalEvents("AcariciarBestia_Player", AcariciarBestia);
        EventsManager.StopCallNormalEvents("SanarBestia_Player", HealBeast);
        EventsManager.StopCallNormalEvents("MontarBestia_Player", RideBeast);
        StopAnimation(stateMachine.Player.PlayerAnimationData.GroundedParameterHash);
    }
    #endregion

    #region M�todos Suscripci�n Acciones Input System
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        stateMachine.Player.PlayerInput.PlayerActions.Run.performed += RunStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.performed += CrouchStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Attack.started += AttackStart;
        stateMachine.Player.PlayerInput.PlayerActions.Heal.started += HealPlayer;
        stateMachine.Player.PlayerInput.PlayerActions.Sprint.started += SprintStart;
        stateMachine.Player.PlayerInput.PlayerActions.Jump.started += JumpStarted;
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.started += OnPointedStarted;
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.canceled += OnPointedCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.ReviveBeast.started += OnReviveStarted;
        stateMachine.Player.PlayerInput.PlayerActions.ReviveBeast.canceled += OnReviveCanceled;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        stateMachine.Player.PlayerInput.PlayerActions.Run.performed -= RunStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.performed -= CrouchStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Attack.started -= AttackStart;
        stateMachine.Player.PlayerInput.PlayerActions.Heal.started -= HealPlayer;
        stateMachine.Player.PlayerInput.PlayerActions.Sprint.started -= SprintStart;
        stateMachine.Player.PlayerInput.PlayerActions.Jump.started -= JumpStarted;
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

    #region M�todos de Acciones Input System
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
        else if (stateMachine.CurrentState is PlayerRideBeastState) // Si est� en el estado de Montar a la Bestia no puede atacar.
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

        string[] healingItemNames = { "Baya Voladora" }; // Guardamos en un array los items espec�ficos que curan.

        foreach (string itemName in healingItemNames)
        {
            ItemData healingBerry = InventoryManager.Instance.GetItemByName(itemName); // Los buscamos en el inventario por el nombre espec�fico (puesto en el ItemDataSO).

            if (healingBerry != null && InventoryManager.Instance.CheckForItem(healingBerry)) // Comprobamos que est�n en el inventario.
            {
                stateMachine.HealState.SetHealingBerry(healingBerry); // Pasamos el valor de curaci�n del item espec�fico que vayamos a comer.
                stateMachine.ChangeState(stateMachine.HealState);
            }
        }
    }

    /*
     * M�todo que transiciona al estado de saltar.
     * Pasa al estado de JumpState siempre y cuando que no est� en estado de doble salto o del estado de caer.
     * @param context - Informaci�n sobre la tecla / acci�n que se activa (Barra espaciadora / Jump).
     */
    protected virtual void JumpStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.CurrentState is PlayerRideBeastState)
            return;
        // Solo salta si el estado actual del jugador no es ni Doble Salto ni Caer.
        if (!(stateMachine.CurrentState is PlayerDoubleJumpState || stateMachine.CurrentState is PlayerFallState))
            stateMachine.ChangeState(stateMachine.JumpState);
    }

    /*
     * M�todo que transiciona al estado de sprintar.
     * Si es estado en el que est� es IdleState, no puede sprintar.
     * @param context - Informaci�n sobre la tecla / acci�n que se activa (Click derecho / Sprint).
     */
    protected virtual void SprintStart(InputAction.CallbackContext context)
    {
        //Debug.Log("Est�s sprintando");
        if (stateMachine.CurrentState is PlayerIdleState || stateMachine.CurrentState is PlayerRideBeastState) return;

        stateMachine.ChangeState(stateMachine.SprintState);
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
        if (stateMachine.CurrentState is PlayerRideBeastState) return;

        stateMachine.ChangeState(stateMachine.FallState);
    }

    /*
     * M�todo que devuelve True/False para comprobar si Player ha tocado suelo o no.
     */
    protected virtual bool IsGrounded()
    {
        Vector3 boxCenter = stateMachine.Player.GroundCheckCollider.transform.position;
        Vector3 boxHalfExtents = new Vector3(0.25f, 0.05f, 0.25f);
        Quaternion boxOrientation = Quaternion.identity;
        LayerMask groundMask = LayerMask.GetMask("Enviroment");

        bool isGrounded = Physics.CheckBox(boxCenter, boxHalfExtents, boxOrientation, groundMask, QueryTriggerInteraction.Ignore);

        return isGrounded;
    }
    #endregion

    #region M�todos de Interacci�n con la Bestia
    private void AcariciarBestia()
    {
        // L�gica de acariciar a la Bestia.
        Debug.Log("Est�s acariciando a la Bestia.");
        stateMachine.ChangeState(stateMachine.PetBeastState);
    }

    // L�gica de curar a la Bestia.
    private void HealBeast()
    {
        if (stateMachine.Player.Beast.currentHealth == stateMachine.Player.Beast.maxHealth)
        {
            Debug.Log("La Bestia tiene la vida al m�ximo");
            return;
        }

        Debug.Log("Est�s sanando a la Bestia");

        string[] healingItemNames = { "Mango Luminoso" }; // Guardamos en un array los items espec�ficos que curan.

        foreach (string itemName in healingItemNames)
        {
            ItemData healingMango = InventoryManager.Instance.GetItemByName(itemName); // Los buscamos en el inventario por el nombre espec�fico (puesto en el ItemDataSO).

            if (healingMango != null && InventoryManager.Instance.CheckForItem(healingMango)) // Comprobamos que est�n en el inventario.
            {
                Debug.Log("Curando");
                stateMachine.HealBeastState.SetHealingMango(healingMango); // Pasamos el valor de curaci�n del item espec�fico que vayamos a comer.
                stateMachine.ChangeState(stateMachine.HealBeastState);
            }
            else
            {
                Debug.Log("No tienes un mango en el inventario para curar a la Bestia.");
            }
        }
    }

    // L�gica de montar en la Bestia.
    private void RideBeast()
    {
        stateMachine.ChangeState(stateMachine.RideBeastState);
    }

    protected bool isCentralButtonPressed;
    protected virtual void OnReviveStarted(InputAction.CallbackContext context)
    {
        if (Vector3.Distance(stateMachine.Player.transform.position, stateMachine.Player.Beast.transform.position) < 3.5f)
        {
            isCentralButtonPressed = true;
            stateMachine.ChangeState(stateMachine.ReviveBeastState);
        }
        else
            Debug.Log("Est�s muy lejos de Bestia como para poder revivirle");
    }

    protected virtual void OnReviveCanceled(InputAction.CallbackContext context)
    {
        isCentralButtonPressed = false;
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
        rightButtontimePressed = 0f;
    }

    private void ChangeToPointedState()
    {
        if (isPointed && stateMachine.CurrentState is PlayerIdleState)
        {
            rightButtontimePressed += Time.deltaTime;

            if (rightButtontimePressed >= 0.5f)
            {
                isPointed = false;
                stateMachine.ChangeState(stateMachine.PointedBeastState);
            }
        }
    }

    protected virtual void OnPointedStateCanceled(InputAction.CallbackContext context) { }
    #endregion
}