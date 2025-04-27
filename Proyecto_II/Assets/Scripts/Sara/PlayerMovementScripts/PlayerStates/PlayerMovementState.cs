using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerMovementState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que hereda de IState y contiene la lógica básica de Player.
 *              Gestiona las entradas de movimiento, las transiciones de animaciones y las interacciones con el mundo (colliders).
 * VERSIÓN: 1.0. Entradas del Input System y entrada y salida de animaciones.
 * VERSIÓN: 2.0. Entradas y salidas de triggers.
 */
public class PlayerMovementState : IState
{
    #region Variables
    protected PlayerStateMachine stateMachine;

    protected readonly PlayerGroundedData groundedData;
    protected readonly PlayerAirborneData airborneData;
    protected readonly PlayerStatsData statsData;

    protected AudioManager audioManager;

    private float currentTimeWithShield;
    #endregion

    /*
     * Constructor de PlayerMovementState.
     * @param1 _stateMachine - Recibe una referencia de PlayerStateMachine para poder acceder a su información.
     */
    public PlayerMovementState(PlayerStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;

        groundedData = stateMachine.Player.Data.GroundedData;
        airborneData = stateMachine.Player.Data.AirborneData;
        statsData = stateMachine.Player.Data.StatsData;

        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }


    #region Métodos Base de la Máquina de Estados
    /*
     * Método de entrada.
     * Se suscriben las entradas del Input System y los eventos.
     */
    public virtual void Enter()
    {
        //stateMachine.Player.CamComponents.m_HorizontalAxis.m_MaxSpeed = 200f;
        //stateMachine.Player.CamComponents.m_VerticalAxis.m_MaxSpeed = 200f;
        AddInputActionsCallbacks();
        EventsManager.CallSpecialEvents<float>("OnAttackPlayer", TakeDamage);
        EventsManager.CallNormalEvents("PickUpItem", PickUp);
    }

    /*
     * Método de lectura de entrada de los inputs.
     * Lee la entrada del Player.
     */
    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    /*
     * Método que actualiza la lógica del juego.
     */
    public virtual void UpdateLogic()
    {
        if(startActiveShield)
            UpdateTimeWithShield();

        EnemyInRange();
    }

    /*
     * Método que actualiza las físicas del juego.
     * Mueve al jugador.
     */
    public virtual void UpdatePhysics()
    {
        Move();
    }

    /*
     * Método que recibe la entrada de colisiones de triggers del mundo.
     * Comprueba si el jugador ha entrado en contacto con el suelo.
     * @param1: collider - El collider con el que choca el Player.
     */
    public virtual void OnTriggerEnter(Collider collider)
    {
        if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            ContactWithGround(collider);
    }

    /*
     * Método que recibe la salida de colisiones de triggers del mundo.
     * Comprueba si el jugador ha dejado de estar en contacto con el suelo.
     * @param1: collider - El collider del que sale el Player.
     */
    public virtual void OnTriggerExit(Collider collider)
    {
        if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            NoContactWithGround(collider);
    }

    /*
     * Método de salida.
     * Se desuscriben las entradas del Input System y los eventos.
     */
    public virtual void Exit()
    {
        EventsManager.StopCallSpecialEvents<float>("OnAttackPlayer", TakeDamage);
        EventsManager.StopCallNormalEvents("PickUpItem", PickUp);
        RemoveInputActionsCallbacks();
    }
    #endregion

    #region Métodos Suscripción Acciones Input System
    /*
     * Método donde se suscriben las acciones de los inputs correspondientes.
     */
    protected virtual void AddInputActionsCallbacks()
    {
        stateMachine.Player.PlayerInput.PlayerActions.Movement.canceled += OnMovementCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.Run.canceled += OnMovementCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.canceled += OnMovementCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.CallBeast.performed += CallBeast;
        stateMachine.Player.PlayerInput.PlayerActions.LockTarget.performed += LockTarget;
        stateMachine.Player.PlayerInput.PlayerActions.Shield.started += OnDefendedStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Shield.canceled+= OnDefendedCanceled;
    }

    /*
     * Método donde se desuscriben las acciones de los inputs correspondientes.
     */
    protected virtual void RemoveInputActionsCallbacks()
    {
        stateMachine.Player.PlayerInput.PlayerActions.Movement.canceled -= OnMovementCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.Run.canceled -= OnMovementCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.canceled -= OnMovementCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.CallBeast.performed -= CallBeast;
        stateMachine.Player.PlayerInput.PlayerActions.LockTarget.performed -= LockTarget;
    }

    /*
     * Método que lee el valor de la entrada de movimiento del jugador y la asigna al estado de movimiento.
     */
    public void ReadMovementInput()
    {
        stateMachine.MovementData.MovementInput = stateMachine.Player.PlayerInput.PlayerActions.Movement.ReadValue<Vector2>();
    }
    #endregion

    #region Métodos Gestión Animaciones
    /*
     * Método que activa la animación correspondiente en el Animator.
     * @param hashNumAnimation - Número (hash) que identifica la animación que debe activarse en el Animator.
     */
    protected void StartAnimation(int hashNumAnimation)
    {
        stateMachine.Player.AnimPlayer.SetBool(hashNumAnimation, true);
    }

    /*
     * Método que desactiva la animación correspondiente en el Animator.
     * @param hashNumAnimation - Número (hash) que identifica la animación que debe desactivarse en el Animator.
     */
    protected void StopAnimation(int hashNumAnimation)
    {
        stateMachine.Player.AnimPlayer.SetBool(hashNumAnimation, false);
    }
    #endregion

    #region Métodos Físicas de Movimiento
    /*
     * Método que gestiona el movimiento del personaje según la dirección y velocidad actual.
     */
    protected virtual void Move()
    {
        if (stateMachine.MovementData.MovementInput == Vector2.zero || stateMachine.MovementData.MovementSpeedModifier == 0f)
            return;

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 movementDirection = cameraForward * stateMachine.MovementData.MovementInput.y + Camera.main.transform.right * stateMachine.MovementData.MovementInput.x;
        movementDirection.Normalize();

        float movSpeed = GetMovementSpeed();
        movementDirection.Normalize();
        stateMachine.Player.RbPlayer.MovePosition(stateMachine.Player.RbPlayer.position + movementDirection * movSpeed * Time.deltaTime);
        Rotate(movementDirection);
    }

    /*
     * Método que rota al personaje hacia la dirección del movimiento.
     * @param _movementDirection - Dirección hacia la que se debe orientar el personaje.
     */
    public void Rotate(Vector3 _movementDirection)
    {
        if (_movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_movementDirection); // Hace que el personaje gire en la dirección donde se produce el movimiento.
            stateMachine.Player.RbPlayer.rotation = Quaternion.Slerp(stateMachine.Player.RbPlayer.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    /*
     * Método que devuelve la dirección del input de movimiento en un Vector3.
     * @return Vector3 - Devuelve la dirección del movimiento.
     */
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.MovementData.MovementInput.x, 0f, stateMachine.MovementData.MovementInput.y);
    }

    /*
     * Método que calcula y devuelve la velocidad actual del personaje.
     * @return float - Devuelve la velocidad del personaje.
     */
    protected float GetMovementSpeed()
    {
        float movementSpeed = groundedData.BaseSpeed * stateMachine.MovementData.MovementSpeedModifier;
        return movementSpeed;
    }
    #endregion

    #region Métodos para Sobrescribir
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) { }

    protected virtual void ContactWithGround(Collider collider) { }

    protected virtual void NoContactWithGround(Collider collider) { }

    protected virtual void FinishAnimation() { }
    #endregion

    #region Métodos de Llamadas de Eventos
    /*
     * Método que cambia el estado del jugador a PickUpState.
     */
    private void PickUp()
    {
        stateMachine.ChangeState(stateMachine.PickUpState);
    }
    #endregion

    #region Métodos Interacción Bestia
    /*
     * Método que gestiona la llamada a la Bestia.
     * @param context - Información sobre la tecla / acción que se activa.
     */
    private void CallBeast(InputAction.CallbackContext context)
    {
        //Debug.Log("Has llamado a la Bestia");
        stateMachine.Player.StartCoroutine(StopCallBeast());
        EventsManager.TriggerNormalEvent("CallBeast");
    }

    /*
     * Corrutina que gestiona que la animación de llamar a la Bestia se realice correctamente.
     */
    IEnumerator StopCallBeast()
    {
        StopAnimation(stateMachine.Player.PlayerAnimationData.GroundedParameterHash);
        StopAnimation(stateMachine.Player.PlayerAnimationData.IdleParameterHash);
        StartAnimation(stateMachine.Player.PlayerAnimationData.CallBeastParameterHash);
        yield return new WaitForSecondsRealtime(1f);
        StopAnimation(stateMachine.Player.PlayerAnimationData.CallBeastParameterHash);
        StartAnimation(stateMachine.Player.PlayerAnimationData.IdleParameterHash);
        StartAnimation(stateMachine.Player.PlayerAnimationData.GroundedParameterHash);
    }
    #endregion

    #region Métodos Interactions Enemies
    private List<GameObject> enemiesTarget = new List<GameObject>();
    private int currentLockTarget = -1;
    private float detectionRange = 5f;

    private void LockTarget(InputAction.CallbackContext context)
    {
        if (enemiesTarget.Count == 0 || !IsListStillValid())
        {
            RefreshEnemyList();
            currentLockTarget = -1;
        }

        if (enemiesTarget.Count == 0) return;

        if (currentLockTarget == enemiesTarget.Count - 1)
        {
            stateMachine.Player.pointTarget.ClearTarget();
            currentLockTarget = -1;
            stateMachine.Player.playerCam.LookAt = stateMachine.Player.lookCamPlayer;
            stateMachine.Player.CamComponents.m_HorizontalAxis.m_MaxSpeed = 200f;
            stateMachine.Player.CamComponents.m_VerticalAxis.m_MaxSpeed = 200f;
            return;
        }

        currentLockTarget = (currentLockTarget + 1) % enemiesTarget.Count;

        GameObject selectedEnemy = enemiesTarget[currentLockTarget];
        stateMachine.Player.pointTarget.SetTarget(selectedEnemy.transform);
        stateMachine.Player.playerCam.LookAt = selectedEnemy.transform;
        stateMachine.Player.CamComponents.m_HorizontalAxis.m_MaxSpeed = 0f;
        stateMachine.Player.CamComponents.m_VerticalAxis.m_MaxSpeed = 0f;
        Debug.Log("Enemigo fijado: " + selectedEnemy.name);
    }

    private bool IsListStillValid()
    {
        for (int i = enemiesTarget.Count - 1; i >= 0; i--)
        {
            GameObject enemy = enemiesTarget[i];
            if (enemy == null || Vector3.Distance(stateMachine.Player.transform.position, enemy.transform.position) > detectionRange)
                enemiesTarget.RemoveAt(i);
        }
        return enemiesTarget.Count > 0;
    }

    private void RefreshEnemyList()
    {
        enemiesTarget.Clear();
        Collider[] enemiesColliders = Physics.OverlapSphere(stateMachine.Player.transform.position, detectionRange);

        foreach (Collider collider in enemiesColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                    enemiesTarget.Add(enemy.gameObject);
            }
        }
    }

    GameObject currentTarget;
    private void EnemyInRange()
    {
        if (currentLockTarget >= 0 && currentLockTarget < enemiesTarget.Count)
        {
            currentTarget = enemiesTarget[currentLockTarget];

            if (currentTarget == null || Vector3.Distance(stateMachine.Player.transform.position, currentTarget.transform.position) > detectionRange)
            {
                Debug.Log("El enemigo fijado se salió del rango.");
                stateMachine.Player.pointTarget.ClearTarget();
                currentLockTarget = -1;
            }
        }
    }

    /*
     * Método de recibir daño.
     * Disminuye la salud del jugador en función del daño recibido y cambia al estado de Medio-Muerta si la salud llega a cero.
     * @param _enemyDamage - Daño recibido por parte del enemigo.
     */
    protected bool isHalfDead = false;
    private void TakeDamage(float _enemyDamage)
    {
        if (isHalfDead) return;

        statsData.CurrentHealth -= _enemyDamage;

        if (statsData.CurrentHealth < Mathf.Epsilon)
            PlayerDead();
        else
            stateMachine.ChangeState(stateMachine.TakeDamageState);
    }
    #endregion

    #region Métodos Defensa
    protected bool shieldButtonPressed = false;
    private float maxTimeWithShield = 5f;
    private bool startActiveShield = false;
    protected virtual void OnDefendedStarted(InputAction.CallbackContext context)
    {
        shieldButtonPressed = true;
        startActiveShield = true;
        currentTimeWithShield = 0f;
        ActivateShield();
    }

    protected virtual void OnDefendedCanceled(InputAction.CallbackContext context)
    {
        shieldButtonPressed = false;
        startActiveShield = false;
        DesactivateShield();
    }

    private void UpdateTimeWithShield()
    {
        currentTimeWithShield += Time.deltaTime;

        Debug.Log(currentTimeWithShield);

        if (shieldButtonPressed && currentTimeWithShield < maxTimeWithShield)
        {
            ActivateShield();
        }
        else
        {
            DesactivateShield();
        }
    }

    private void ActivateShield()
    {
        stateMachine.Player.Shield.SetActive(true);
    }

    private void DesactivateShield()
    {
        startActiveShield = false;
        stateMachine.Player.Shield.SetActive(false);
    }
    #endregion

    protected virtual void PlayerDead()
    {
        statsData.CurrentHealth = Mathf.Max(statsData.CurrentHealth, 0f);
        isHalfDead = true;
        stateMachine.ChangeState(stateMachine.HalfDeadState);
    }

    #region Métodos Cursor
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion
}
