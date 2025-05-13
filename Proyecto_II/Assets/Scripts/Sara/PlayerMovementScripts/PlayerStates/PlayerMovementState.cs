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
 * 
 */
public class PlayerMovementState : IState
{
    #region Variables
    #region Variables Generales PlayerMovementState
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundedData groundedData;
    protected readonly PlayerAirborneData airborneData;
    protected readonly PlayerStatsData statsData;
    protected AudioManager audioManager;
    #endregion

    #region Variables Interacción Enemigos
    private List<GameObject> enemiesTarget = new List<GameObject>();
    private int currentLockTarget = -1;
    private float detectionRange = 5f;
    GameObject currentTarget;
    #endregion

    #region Variables Defensa Player
    protected bool shieldButtonPressed = false;
    private float currentTimeWithShield;
    private float maxTimeWithShield = 5f;
    private bool startActiveShield = false;
    #endregion

    #region Variables Cambio Expresiones Player
    protected Dictionary<int, Material> materialFacePlayer;
    protected SkinnedMeshRenderer meshRendererPlayer;
    protected Material[] materials;
    #endregion
    #endregion

    #region Constructor de PlayerMovementState
    /// <summary>
    /// Constructor de <c>PlayerMovementState</c>.
    /// </summary>
    /// <param name="_stateMachine">Referencia a <c>PlayerStateMachine</c> para poder acceder a su información.</param>
    public PlayerMovementState(PlayerStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;

        groundedData = stateMachine.Player.Data.GroundedData;
        airborneData = stateMachine.Player.Data.AirborneData;
        statsData = stateMachine.Player.Data.StatsData;

        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }
    #endregion

    #region Métodos
    #region Métodos Base de la Máquina de Estados
    /// <summary>
    /// Método de entrada del estado de <c>PlayerMovementState</c>.
    /// Se suscriben las entradas del Input System y los eventos.
    /// </summary>
    public virtual void Enter()
    {
        AddInputActionsCallbacks();
        CreateFaceMaterialPlayerDictionary();
        ChangeFacePlayer();
        EventsManager.CallSpecialEvents<float>("OnAttackPlayer", TakeDamage);
        EventsManager.CallNormalEvents("PickUpItem", PickUp);
    }

    /// <summary>
    /// Método de lectura de entrada de los inputs.
    ///Lee la entrada del Player.
    /// </summary>
    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    /// <summary>
    /// Método que actualiza la lógica del Player.
    /// Si el escudo está activo, actualiza el tiempo que puede estar activo.
    /// Verifica si hay enemigos dentro del rango.
    /// </summary>
    public virtual void UpdateLogic()
    {
        if(startActiveShield)
            UpdateTimeWithShield();

        EnemyInRange();
    }

    /// <summary>
    /// Método que actualiza las físicas del juego.
    /// Mueve al jugador.
    /// </summary>
    public virtual void UpdatePhysics()
    {
        Move();
    }

    /// <summary>
    /// Método que recibe la entrada de colisiones de triggers del mundo.
    /// </summary>
    /// <param name="collider">El collider con el que choca el Player.</param>
    public virtual void OnTriggerEnter(Collider collider) {}

    /// <summary>
    /// Método que recibe la salida de colisiones de triggers del mundo.
    /// </summary>
    /// <param name="collider">El collider del que sale el Player.</param>
    public virtual void OnTriggerExit(Collider collider) { }

    /// <summary>
    /// Método de salida del estado de <c>PlayerMovementState</c>.
    /// Se desuscriben las entradas del Input System y los eventos.
    /// </summary>
    public virtual void Exit()
    {
        EventsManager.StopCallSpecialEvents<float>("OnAttackPlayer", TakeDamage);
        EventsManager.StopCallNormalEvents("PickUpItem", PickUp);
        RemoveInputActionsCallbacks();
    }
    #endregion

    #region Métodos Suscripción Acciones Input System
    /// <summary>
    /// Método donde se suscriben las acciones de los inputs correspondientes.
    /// </summary>
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

    /// <summary>
    /// Método donde se desuscriben las acciones de los inputs correspondientes.
    /// </summary>
    protected virtual void RemoveInputActionsCallbacks()
    {
        stateMachine.Player.PlayerInput.PlayerActions.Movement.canceled -= OnMovementCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.Run.canceled -= OnMovementCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.canceled -= OnMovementCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.CallBeast.performed -= CallBeast;
        stateMachine.Player.PlayerInput.PlayerActions.LockTarget.performed -= LockTarget;
        stateMachine.Player.PlayerInput.PlayerActions.Shield.started -= OnDefendedStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Shield.canceled -= OnDefendedCanceled;
    }

    /// <summary>
    /// Método que lee el valor de la entrada de movimiento del jugador.
    /// Asigna dicho valor a la variable de movimiento en el estado actual de movimiento.
    /// </summary>
    public void ReadMovementInput()
    {
        stateMachine.MovementData.MovementInput = stateMachine.Player.PlayerInput.PlayerActions.Movement.ReadValue<Vector2>();
    }
    #endregion

    #region Métodos Gestión Animaciones
    /// <summary>
    /// Método que activa la animación correspondiente en el Animator.
    /// </summary>
    /// <param name="hashNumAnimation">Número (hash) que identifica la animación que debe activarse en el Animator.</param>
    protected void StartAnimation(int hashNumAnimation)
    {
        stateMachine.Player.AnimPlayer.SetBool(hashNumAnimation, true);
    }

    /// <summary>
    /// Método que desactiva la animación correspondiente en el Animator.
    /// </summary>
    /// <param name="hashNumAnimation">Número (hash) que identifica la animación que debe desactivarse en el Animator.</param>
    protected void StopAnimation(int hashNumAnimation)
    {
        stateMachine.Player.AnimPlayer.SetBool(hashNumAnimation, false);
    }
    #endregion

    #region Métodos Físicas de Movimiento
    /// <summary>
    /// Método que gestiona el movimiento del personaje según la dirección y velocidad actual.
    /// </summary>
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
        //movementDirection.Normalize();
        stateMachine.Player.RbPlayer.MovePosition(stateMachine.Player.RbPlayer.position + movementDirection * movSpeed * Time.deltaTime);
        Rotate(movementDirection);
    }

    /// <summary>
    /// Método que rota al personaje hacia la dirección del movimiento.
    /// </summary>
    /// <param name="_movementDirection">Dirección hacia la que se debe orientar el personaje.</param>
    public void Rotate(Vector3 _movementDirection)
    {
        if (_movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_movementDirection); // Hace que el personaje gire en la dirección donde se produce el movimiento.
            stateMachine.Player.RbPlayer.rotation = Quaternion.Slerp(stateMachine.Player.RbPlayer.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    /// <summary>
    /// Método que devuelve la dirección del input de movimiento en un Vector3.
    /// </summary>
    /// <returns>Un Vector3 que representa la dirección de movimiento en los ejes X y Z, ignorando el eje Y.</returns>
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.MovementData.MovementInput.x, 0f, stateMachine.MovementData.MovementInput.y);
    }

    /// <summary>
    /// Método que calcula y devuelve la velocidad actual del personaje.
    /// </summary>
    /// <returns>Devuelve un float que representa la velocidad del personaje.</returns>
    protected float GetMovementSpeed()
    {
        float movementSpeed = groundedData.BaseSpeed * stateMachine.MovementData.MovementSpeedModifier;
        return movementSpeed;
    }
    #endregion

    #region Método Comprobar si Player Toca Suelo
    /// <summary>
    /// Método que devuelve True/False para comprobar si Player ha tocado suelo o no.
    /// </summary>
    /// <returns>Si detecta que Player toca un elemento con la layer de "Enviroment" devuelve True, sino, False.</returns>
    protected virtual bool IsGrounded()
    {
        Vector3 boxCenter = stateMachine.Player.GroundCheckCollider.transform.position;
        Vector3 boxHalfExtents = new Vector3(0.5f, 0.1f, 0.55f); // Tamaño de la caja.
        Quaternion boxOrientation = Quaternion.identity; // Mantener la rotación como la del GroundCheckCollider.
        LayerMask groundMask = stateMachine.Player.LayerData.EnviromentLayer;

        bool isGrounded = Physics.CheckBox(boxCenter, boxHalfExtents, boxOrientation, groundMask, QueryTriggerInteraction.Ignore);

        return isGrounded;
    }
    #endregion

    #region Métodos para Sobrescribir
    /// <summary>
    /// Método virtual que cancela el movimiento de Player.
    /// Se crea en este estado sin lógica, se sobreescriben en otros estados que hereden de <c>PlayerMovementState</c>.
    /// </summary>
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) { }

    /// <summary>
    /// Método virtual que comprueba si una animación ha terminado o no.
    /// Se crea en este estado sin lógica, se sobreescriben en otros estados que hereden de <c>PlayerMovementState</c>.
    /// </summary>
    protected virtual void FinishAnimation() { }
    #endregion

    #region Métodos de Llamadas de Eventos
    /// <summary>
    /// Método que cambia el estado del jugador a PickUpState.
    /// </summary>
    private void PickUp()
    {
        stateMachine.ChangeState(stateMachine.PickUpState);
    }
    #endregion

    #region Métodos Interacción Bestia
    /// <summary>
    /// Método que cambia al estado de llamar a la Bestia.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    private void CallBeast(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.CallBeastState);
    }
    #endregion

    #region Métodos Interactions Enemies
    /// <summary>
    /// Cambia el objetivo fijado al siguiente más cercano dentro de un límite.
    /// Si no hay enemigos se actualiza la lista.
    /// Después del último objetivo de la lista, deja de fijar.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
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
            return;
        }

        currentLockTarget = (currentLockTarget + 1) % enemiesTarget.Count;
        GameObject selectedEnemy = enemiesTarget[currentLockTarget];
        stateMachine.Player.pointTarget.SetTarget(selectedEnemy.transform);
        stateMachine.Player.playerCam.LookAt = selectedEnemy.transform;
        //Debug.Log("Enemigo fijado: " + selectedEnemy.name);
    }

    /// <summary>
    /// Comprueba si el enemigo se mantiene dentro del rango de detección para poder fijarle.
    /// Si se sale del rango, se elimina de la lista.
    /// </summary>
    /// <returns>Devuelve True si hay al menos un enemigo en la lista, en caso de no haber ninguno, devuelve False.</returns>
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

    /// <summary>
    /// Actualiza la lista de enemigos posibles dentro del rango de detección de Player.
    /// Limpia la lista actual y añade los objetos que tengan el tag "Enemy" y que estén dentro del área.
    /// </summary>
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

    /// <summary>
    /// Verifica si el enemigo actualmente fijado sigue dentro del rango de detección.
    /// Si el enemigo es nulo o está fuera de rango, se elimina el objetivo fijado y se reinicia el índice.
    /// </summary>
    private void EnemyInRange()
    {
        if (currentLockTarget >= 0 && currentLockTarget < enemiesTarget.Count)
        {
            currentTarget = enemiesTarget[currentLockTarget];

            if (currentTarget == null || Vector3.Distance(stateMachine.Player.transform.position, currentTarget.transform.position) > detectionRange)
            {
                //Debug.Log("El enemigo fijado se salió del rango.");
                stateMachine.Player.pointTarget.ClearTarget();
                currentLockTarget = -1;
            }
        }
    }

    /// <summary>
    /// Método que disminuye la salud del jugador en función del daño recibido y cambia al estado de Medio-Muerta si la salud llega a cero.
    /// </summary>
    /// <param name="_enemyDamage">Daño recibido por parte del enemigo.</param>
    private void TakeDamage(float _enemyDamage)
    {
        if (stateMachine.CurrentState is PlayerHalfDeadState || stateMachine.Player.Shield.activeSelf) return;

        statsData.CurrentHealth -= _enemyDamage;
        EventsManager.TriggerSpecialEvent<float>("PlayerHealth", statsData.CurrentHealth);

        if (statsData.CurrentHealth < Mathf.Epsilon)
            PlayerDead();
        else
            stateMachine.ChangeState(stateMachine.TakeDamageState);
    }
    #endregion

    #region Métodos Defensa
    /// <summary>
    /// Método para que se active el escudo de Player.
    /// Comienza el tiempo que puede estar el escudo activo.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected virtual void OnDefendedStarted(InputAction.CallbackContext context)
    {
        shieldButtonPressed = true;
        startActiveShield = true;
        currentTimeWithShield = 0f;
        ActivateShield();
    }

    /// <summary>
    /// Método para que se desactive el escudo de Player.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected virtual void OnDefendedCanceled(InputAction.CallbackContext context)
    {
        shieldButtonPressed = false;
        startActiveShield = false;
        DesactivateShield();
    }

    /// <summary>
    /// Actualiza el tiempo que puede estar Player con el escudo activo.
    /// Si pasa del tiempo máximo, se desactiva el escudo.
    /// </summary>
    private void UpdateTimeWithShield()
    {
        currentTimeWithShield += Time.deltaTime;

        //Debug.Log(currentTimeWithShield);

        if (shieldButtonPressed && currentTimeWithShield < maxTimeWithShield)
            ActivateShield();
        else
            DesactivateShield();
    }

    /// <summary>
    /// Activa el escudo.
    /// </summary>
    private void ActivateShield()
    {
        stateMachine.Player.Shield.SetActive(true);
    }

    /// <summary>
    /// Desactiva el escudo.
    /// </summary>
    private void DesactivateShield()
    {
        startActiveShield = false;
        stateMachine.Player.Shield.SetActive(false);
    }
    #endregion

    #region Métodos Cambiar Expresiones Player
    /// <summary>
    /// Se crea un diccionario para almacenar la información de los materiales de la cara de Player.
    /// </summary>
    private void CreateFaceMaterialPlayerDictionary()
    {
        meshRendererPlayer = stateMachine.Player.RenderPlayer;
        materials = meshRendererPlayer.materials;

        materialFacePlayer = new Dictionary<int, Material>();
        for (int i = 0; i < materials.Length; i++)
            materialFacePlayer[i] = materials[i];
    }

    /// <summary>
    /// Verifica si el diccionario de materiales faciales del jugador está creado; si no, lo crea.
    /// </summary>
    protected virtual void ChangeFacePlayer()
    {
        if (materialFacePlayer == null)
            CreateFaceMaterialPlayerDictionary();        
    }

    /// <summary>
    /// Método para indicar el material que se quiere cambiar y las coordenadas del cambio.
    /// </summary>
    /// <param name="materialIndex">Índice del material (para diferenciar boca, ojos y cejas) del diccionario.</param>
    /// <param name="offset">Coordenadas a las que se va a desplazar el material para cambiar la expresión facial.</param>
    protected void SetFaceProperty(int materialIndex, Vector2 offset)
    {
        const string propertyName = "_Offset";

        if (materialFacePlayer.ContainsKey(materialIndex))
        {
            Material specificMaterial = materialFacePlayer[materialIndex];

            if (specificMaterial.HasProperty(propertyName))
                specificMaterial.SetVector(propertyName, offset);
        }
    }
    #endregion

    #region Métodos Cursor
    /// <summary>
    /// Método para bloquear el cursor y hacerlo invisible.
    /// </summary>
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Método para desbloquear el cursor y hacerlo visible.
    /// </summary>
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion

    #region Método PlayerMorir
    /// <summary>
    /// Si Player pierde toda la vida se cambia al estado de MEDIO-MUERTA.
    /// </summary>
    protected virtual void PlayerDead()
    {
        statsData.CurrentHealth = Mathf.Max(statsData.CurrentHealth, 0f);
        stateMachine.ChangeState(stateMachine.HalfDeadState);
    }
    #endregion
    #endregion
}