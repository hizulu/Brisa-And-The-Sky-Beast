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
    protected ItemData healIncreaseSpecificItem;

    protected bool canFall = true;

    #region Variables PointedState
    protected bool isPointed = false;
    protected float rightButtontimePressed = 0f;
    protected bool isCentralButtonPressed;
    #endregion

    #region Variables Pesta�eo Player
    private float blinkTimer;
    private float blinkInterval;
    private bool isBlinking = false;
    #endregion
    #endregion

    #region M�todos Base de la M�quina de Estados
    /// <summary>
    /// M�todo de entrada del estado de <c>PlayerGroundedState</c> que lo inicializa.
    /// Se suscribe a los eventos.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        EventsManager.CallNormalEvents("AcariciarBestia_Player", AcariciarBestia);
        EventsManager.CallNormalEvents("SanarBestia_Player", HealBeast);
        EventsManager.CallNormalEvents("MontarBestia_Player", RideBeast);
        StartAnimation(stateMachine.Player.PlayerAnimationData.GroundedParameterHash);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void UpdateLogic()
    {
        if (!IsGrounded() && canFall)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

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
    /// <summary>
    /// M�todo que suscribe las acciones del New Input System.
    /// </summary>
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

    /// <summary>
    /// M�todo que desuscribe las acciones del New Input System.
    /// </summary>
    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        stateMachine.Player.PlayerInput.PlayerActions.Run.performed -= RunStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.performed -= CrouchStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Attack.started -= AttackStart;
        stateMachine.Player.PlayerInput.PlayerActions.Heal.started -= HealPlayer;
        stateMachine.Player.PlayerInput.PlayerActions.Sprint.started -= SprintStart;
        stateMachine.Player.PlayerInput.PlayerActions.Jump.started -= JumpStarted;
        //stateMachine.Player.PlayerInput.PlayerActions.PointedMode.started -= OnPointedStarted;
        //stateMachine.Player.PlayerInput.PlayerActions.PointedMode.canceled -= OnPointedCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.ReviveBeast.started -= OnReviveStarted;
        stateMachine.Player.PlayerInput.PlayerActions.ReviveBeast.canceled -= OnReviveCanceled;
    }
    #endregion

    #region M�todos Gesti�n Movimiento Player
    /// <summary>
    /// M�todo que gestiona las transiciones de Player de movimiento.
    /// Si detecta que el input de correr est� presionado, pasa al estado de correr.
    /// Si detecta que el input de sigilo est� presionado, pasa al estado de sigilo.
    /// Si ninguno de los anteriores inputs se detecta presionado, pasa al estado de caminar.
    /// </summary>
    protected virtual void OnMove()
    {
        if (stateMachine.Player.PlayerInput.PlayerActions.Run.IsPressed())
            stateMachine.ChangeState(stateMachine.RunState);
        else if(stateMachine.Player.PlayerInput.PlayerActions.Crouch.IsPressed())
            stateMachine.ChangeState(stateMachine.CrouchState);
        else
            stateMachine.ChangeState(stateMachine.WalkState);
    }

    /// <summary>
    /// M�todo que pasa al estado de idle.
    /// </summary>
    protected virtual void OnStop()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
    #endregion

    #region M�todos de Acciones Input System
    /// <summary>
    /// M�todo que gestiona la acci�n de correr.
    /// Si detecta que no est� tocando el suelo, no hace nada.
    /// Si detecta que el estado actual en el que est� Player es el de sigilo, no hace nada.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void RunStarted(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) // Si no est� tocando suelo, no se cambia a correr.
            return;

        if (stateMachine.CurrentState == stateMachine.CrouchState) // Si el estado actual del jugador es "Crouch", no se cambia a "Run".
            return;

        stateMachine.ChangeState(stateMachine.RunState);
    }

    /// <summary>
    /// M�todo que gestiona la acci�n de sigilo.
    /// Si detecta que el estado actual en el que est� Player es el de correr, no hace nada.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void CrouchStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.CurrentState == stateMachine.RunState) // Si el estado actual del jugador es "Run", no se cambia a "Crouch".
            return;

        if(stateMachine.MovementData.MovementInput == Vector2.zero)
            stateMachine.ChangeState(stateMachine.CrouchPoseState);
        else
            stateMachine.ChangeState(stateMachine.CrouchState);
    }

    /// <summary>
    /// M�todo que gestiona la acci�n de atacar.
    /// Si Player no tiene activo el objeto "Palo" en la jerarqu�a, no hace nada.
    /// Si el estado actual de Player es el de montando a la Bestia, no hace nada.
    /// Si est� en medio de un combo (ataque 2 o 3) no puede pasar a realizar el ataque 1.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void AttackStart(InputAction.CallbackContext context)
    {
        if ((!stateMachine.Player.PaloBrisa.activeInHierarchy && !stateMachine.Player.Baculo.activeInHierarchy) || stateMachine.CurrentState is PlayerRideBeastState || startActiveShield) return;
        else if (!stateMachine.Player.PaloBrisa.activeInHierarchy && stateMachine.Player.Baculo.activeInHierarchy)
        {
            if (!(stateMachine.CurrentState is PlayerAttack02 || stateMachine.CurrentState is PlayerAttack03))
                stateMachine.ChangeState(stateMachine.Attack01State);
        }
        //else
        //{
        //    // Solo cambiar a Attack01 si no estamos en medio de un combo o ataque.
        //    if (!(stateMachine.CurrentState is PlayerAttack02 || stateMachine.CurrentState is PlayerAttack03))
        //        stateMachine.ChangeState(stateMachine.Attack01State);
        //}
    }

    /// <summary>
    /// M�todo que gestiona la acci�n de curar.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
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
                EventsManager.TriggerSpecialEvent<float>("PlayerHealth", statsData.CurrentHealth); // Llamamos al evento de curar.
            }
        }
    }

    /// <summary>
    /// M�todo que gestiona la acci�n de saltar.
    /// Si est� en estado de montando a la Bestia, no hace nada.
    /// Pasa al estado de JumpState siempre y cuando que no est� en estado de doble salto o del estado de caer.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void JumpStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.CurrentState is PlayerRideBeastState)
            return;
        // Solo salta si el estado actual del jugador no es ni Doble Salto ni Caer.
        if (!(stateMachine.CurrentState is PlayerDoubleJumpState || stateMachine.CurrentState is PlayerFallState))
            stateMachine.ChangeState(stateMachine.JumpState);
    }

    /// <summary>
    /// M�todo que gestiona la acci�n de sprintar.
    /// Si es estado en el que est� es IdleState, no puede sprintar.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void SprintStart(InputAction.CallbackContext context)
    {
        //Debug.Log("Est�s sprintando");
        if (stateMachine.CurrentState is PlayerIdleState || stateMachine.CurrentState is PlayerRideBeastState) return;

        stateMachine.ChangeState(stateMachine.SprintState);
    }
    #endregion

    #region M�todos de Interacci�n con la Bestia
    /// <summary>
    /// M�todo que gestiona la acci�n de acariciar a la Bestia.
    /// </summary>
    private void AcariciarBestia()
    {
        Debug.Log("Est�s acariciando a la Bestia.");
        stateMachine.ChangeState(stateMachine.PetBeastState);
    }

    /// <summary>
    /// M�todo que gestiona la acci�n de curar a la Bestia.
    /// Si la Bestia tiene la vida al m�ximo, no hace nada.
    /// </summary>
    private void HealBeast()
    {
        if (stateMachine.Player.Beast.currentHealth == stateMachine.Player.Beast.maxHealth)
        {
            //Debug.Log("La Bestia tiene la vida al m�ximo");
            return;
        }

        string[] healingItemNames = { "Mango Luminoso" }; // Guardamos en un array los items espec�ficos que curan.

        foreach (string itemName in healingItemNames)
        {
            ItemData healingMango = InventoryManager.Instance.GetItemByName(itemName); // Los buscamos en el inventario por el nombre espec�fico (puesto en el ItemDataSO).

            if (healingMango != null && InventoryManager.Instance.CheckForItem(healingMango)) // Comprobamos que est�n en el inventario.
            {
                //Debug.Log("Est�s sanando a la Bestia");
                stateMachine.HealBeastState.SetHealingMango(healingMango); // Pasamos el valor de curaci�n del item espec�fico que vayamos a comer.
                stateMachine.ChangeState(stateMachine.HealBeastState);
            }
            else
            {
                Debug.Log("No tienes un mango en el inventario para curar a la Bestia.");
            }
        }
    }

    /// <summary>
    /// M�todo que gestiona la acci�n de montar a la Bestia.
    /// </summary>
    private void RideBeast()
    {
        stateMachine.ChangeState(stateMachine.RideBeastState);
    }

    /// <summary>
    /// M�todo que gestiona la acci�n de poder revivir a la Bestia.
    /// Player debe estar dentro de un rango concreto de la Bestia para revivirla.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void OnReviveStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Pulsando para revivir a Bestia");
        if (Vector3.Distance(stateMachine.Player.transform.position, stateMachine.Player.Beast.transform.position) < 3.5f && stateMachine.Player.Beast.currentHealth <= 0f)
        {
            isCentralButtonPressed = true;
            stateMachine.ChangeState(stateMachine.ReviveBeastState);
        }
        else
            Debug.Log("Est�s muy lejos de Bestia como para poder revivirle");
    }

    /// <summary>
    /// M�todo que detecta si se ha dejado de presionar el bot�n para revivir a la Bestia.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void OnReviveCanceled(InputAction.CallbackContext context)
    {
        isCentralButtonPressed = false;
    }
    #endregion

    #region M�todos Pasar a PointedBeast
    /// <summary>
    /// M�todo que detecta si se ha emppezado a pulsar la tecla para entrar en modo Pointed.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void OnPointedStarted(InputAction.CallbackContext context)
    {
        isPointed = true;
    }

    /// <summary>
    /// M�todo que detecta si se ha dejado de pulsar la tecla para entrar en modo Pointed.
    /// Resetea el tiempo necesario que hay que estar pulsando el click para entrar en modo Pointed.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void OnPointedCanceled(InputAction.CallbackContext context)
    {
        isPointed = false;
        rightButtontimePressed = 0f;
    }

    /// <summary>
    /// M�todo que comprueba si se lleva pulsado el tiempo concreto para pasar a PointedState.
    /// Solo se puede entrar en PointedState si se est� en IdleState.
    /// </summary>
    private void ChangeToPointedState()
    {
        if (isPointed && stateMachine.CurrentState is PlayerIdleState)
        {
            rightButtontimePressed += Time.deltaTime;

            if (rightButtontimePressed >= 1f)
            {
                isPointed = false;
                stateMachine.ChangeState(stateMachine.PointedBeastState);
            }
        }
    }

    protected virtual void OnPointedStateCanceled(InputAction.CallbackContext context) { }
    #endregion

    #region M�todos Pesta�ear
    /// <summary>
    /// M�todo que gestiona el pesta�eo de Player cuando est� en Idle.
    /// El tiempo entre pesta�eos tiene una aleatoriedad entre 2 y 5 segundos.
    /// Realiza tres posicines para simular el pesta�eo; abiertos, semi-cerrados y cerrados completamente.
    /// </summary>
    protected virtual void HandleBlinking()
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

    /// <summary>
    /// M�todo para dar aleatoriedad entre los pesta�eos de Player.
    /// </summary>
    protected virtual void SetRandomBlink()
    {
        blinkInterval = Random.Range(2f, 5f);
    }
    #endregion
}