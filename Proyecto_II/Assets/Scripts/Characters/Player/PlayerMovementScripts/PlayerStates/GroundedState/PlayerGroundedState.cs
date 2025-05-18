 using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerGroundedState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerMovementState y contiene la lógica básica de Player cuando está en el suelo.
 * VERSIÓN: 1.0.
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

    #region Variables Pestañeo Player
    private float blinkTimer;
    private float blinkInterval;
    private bool isBlinking = false;
    #endregion
    #endregion

    #region Métodos Base de la Máquina de Estados
    /// <summary>
    /// Método de entrada del estado de <c>PlayerGroundedState</c> que lo inicializa.
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

    #region Métodos Suscripción Acciones Input System
    /// <summary>
    /// Método que suscribe las acciones del New Input System.
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
    /// Método que desuscribe las acciones del New Input System.
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

    #region Métodos Gestión Movimiento Player
    /// <summary>
    /// Método que gestiona las transiciones de Player de movimiento.
    /// Si detecta que el input de correr está presionado, pasa al estado de correr.
    /// Si detecta que el input de sigilo está presionado, pasa al estado de sigilo.
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
    /// Método que pasa al estado de idle.
    /// </summary>
    protected virtual void OnStop()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
    #endregion

    #region Métodos de Acciones Input System
    /// <summary>
    /// Método que gestiona la acción de correr.
    /// Si detecta que no está tocando el suelo, no hace nada.
    /// Si detecta que el estado actual en el que está Player es el de sigilo, no hace nada.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected virtual void RunStarted(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) // Si no está tocando suelo, no se cambia a correr.
            return;

        if (stateMachine.CurrentState == stateMachine.CrouchState) // Si el estado actual del jugador es "Crouch", no se cambia a "Run".
            return;

        stateMachine.ChangeState(stateMachine.RunState);
    }

    /// <summary>
    /// Método que gestiona la acción de sigilo.
    /// Si detecta que el estado actual en el que está Player es el de correr, no hace nada.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
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
    /// Método que gestiona la acción de atacar.
    /// Si Player no tiene activo el objeto "Palo" en la jerarquía, no hace nada.
    /// Si el estado actual de Player es el de montando a la Bestia, no hace nada.
    /// Si está en medio de un combo (ataque 2 o 3) no puede pasar a realizar el ataque 1.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
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
    /// Método que gestiona la acción de curar.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected virtual void HealPlayer(InputAction.CallbackContext context)
    {
        if (statsData.CurrentHealth >= statsData.MaxHealth) return; // Si la vida actual está al máximo, no hacemos nada.

        string[] healingItemNames = { "Baya Voladora" }; // Guardamos en un array los items específicos que curan.

        foreach (string itemName in healingItemNames)
        {
            ItemData healingBerry = InventoryManager.Instance.GetItemByName(itemName); // Los buscamos en el inventario por el nombre específico (puesto en el ItemDataSO).

            if (healingBerry != null && InventoryManager.Instance.CheckForItem(healingBerry)) // Comprobamos que estén en el inventario.
            {
                stateMachine.HealState.SetHealingBerry(healingBerry); // Pasamos el valor de curación del item específico que vayamos a comer.
                stateMachine.ChangeState(stateMachine.HealState);
                EventsManager.TriggerSpecialEvent<float>("PlayerHealth", statsData.CurrentHealth); // Llamamos al evento de curar.
            }
        }
    }

    /// <summary>
    /// Método que gestiona la acción de saltar.
    /// Si está en estado de montando a la Bestia, no hace nada.
    /// Pasa al estado de JumpState siempre y cuando que no esté en estado de doble salto o del estado de caer.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected virtual void JumpStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.CurrentState is PlayerRideBeastState)
            return;
        // Solo salta si el estado actual del jugador no es ni Doble Salto ni Caer.
        if (!(stateMachine.CurrentState is PlayerDoubleJumpState || stateMachine.CurrentState is PlayerFallState))
            stateMachine.ChangeState(stateMachine.JumpState);
    }

    /// <summary>
    /// Método que gestiona la acción de sprintar.
    /// Si es estado en el que está es IdleState, no puede sprintar.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected virtual void SprintStart(InputAction.CallbackContext context)
    {
        //Debug.Log("Estás sprintando");
        if (stateMachine.CurrentState is PlayerIdleState || stateMachine.CurrentState is PlayerRideBeastState) return;

        stateMachine.ChangeState(stateMachine.SprintState);
    }
    #endregion

    #region Métodos de Interacción con la Bestia
    /// <summary>
    /// Método que gestiona la acción de acariciar a la Bestia.
    /// </summary>
    private void AcariciarBestia()
    {
        Debug.Log("Estás acariciando a la Bestia.");
        stateMachine.ChangeState(stateMachine.PetBeastState);
    }

    /// <summary>
    /// Método que gestiona la acción de curar a la Bestia.
    /// Si la Bestia tiene la vida al máximo, no hace nada.
    /// </summary>
    private void HealBeast()
    {
        if (stateMachine.Player.Beast.currentHealth == stateMachine.Player.Beast.maxHealth)
        {
            //Debug.Log("La Bestia tiene la vida al máximo");
            return;
        }

        string[] healingItemNames = { "Mango Luminoso" }; // Guardamos en un array los items específicos que curan.

        foreach (string itemName in healingItemNames)
        {
            ItemData healingMango = InventoryManager.Instance.GetItemByName(itemName); // Los buscamos en el inventario por el nombre específico (puesto en el ItemDataSO).

            if (healingMango != null && InventoryManager.Instance.CheckForItem(healingMango)) // Comprobamos que estén en el inventario.
            {
                //Debug.Log("Estás sanando a la Bestia");
                stateMachine.HealBeastState.SetHealingMango(healingMango); // Pasamos el valor de curación del item específico que vayamos a comer.
                stateMachine.ChangeState(stateMachine.HealBeastState);
            }
            else
            {
                Debug.Log("No tienes un mango en el inventario para curar a la Bestia.");
            }
        }
    }

    /// <summary>
    /// Método que gestiona la acción de montar a la Bestia.
    /// </summary>
    private void RideBeast()
    {
        stateMachine.ChangeState(stateMachine.RideBeastState);
    }

    /// <summary>
    /// Método que gestiona la acción de poder revivir a la Bestia.
    /// Player debe estar dentro de un rango concreto de la Bestia para revivirla.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected virtual void OnReviveStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Pulsando para revivir a Bestia");
        if (Vector3.Distance(stateMachine.Player.transform.position, stateMachine.Player.Beast.transform.position) < 3.5f && stateMachine.Player.Beast.currentHealth <= 0f)
        {
            isCentralButtonPressed = true;
            stateMachine.ChangeState(stateMachine.ReviveBeastState);
        }
        else
            Debug.Log("Estás muy lejos de Bestia como para poder revivirle");
    }

    /// <summary>
    /// Método que detecta si se ha dejado de presionar el botón para revivir a la Bestia.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected virtual void OnReviveCanceled(InputAction.CallbackContext context)
    {
        isCentralButtonPressed = false;
    }
    #endregion

    #region Métodos Pasar a PointedBeast
    /// <summary>
    /// Método que detecta si se ha emppezado a pulsar la tecla para entrar en modo Pointed.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected virtual void OnPointedStarted(InputAction.CallbackContext context)
    {
        isPointed = true;
    }

    /// <summary>
    /// Método que detecta si se ha dejado de pulsar la tecla para entrar en modo Pointed.
    /// Resetea el tiempo necesario que hay que estar pulsando el click para entrar en modo Pointed.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected virtual void OnPointedCanceled(InputAction.CallbackContext context)
    {
        isPointed = false;
        rightButtontimePressed = 0f;
    }

    /// <summary>
    /// Método que comprueba si se lleva pulsado el tiempo concreto para pasar a PointedState.
    /// Solo se puede entrar en PointedState si se está en IdleState.
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

    #region Métodos Pestañear
    /// <summary>
    /// Método que gestiona el pestañeo de Player cuando está en Idle.
    /// El tiempo entre pestañeos tiene una aleatoriedad entre 2 y 5 segundos.
    /// Realiza tres posicines para simular el pestañeo; abiertos, semi-cerrados y cerrados completamente.
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
    /// Método para dar aleatoriedad entre los pestañeos de Player.
    /// </summary>
    protected virtual void SetRandomBlink()
    {
        blinkInterval = Random.Range(2f, 5f);
    }
    #endregion
}