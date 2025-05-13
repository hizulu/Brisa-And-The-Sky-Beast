using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerMovementState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase que hereda de IState y contiene la l�gica b�sica de Player.
 *              Gestiona las entradas de movimiento, las transiciones de animaciones y las interacciones con el mundo (colliders).
 * VERSI�N: 1.0. Entradas del Input System y entrada y salida de animaciones.
 * VERSI�N: 2.0. Entradas y salidas de triggers.
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

    #region Variables Interacci�n Enemigos
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
    /// <param name="_stateMachine">Referencia a <c>PlayerStateMachine</c> para poder acceder a su informaci�n.</param>
    public PlayerMovementState(PlayerStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;

        groundedData = stateMachine.Player.Data.GroundedData;
        airborneData = stateMachine.Player.Data.AirborneData;
        statsData = stateMachine.Player.Data.StatsData;

        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }
    #endregion

    #region M�todos
    #region M�todos Base de la M�quina de Estados
    /// <summary>
    /// M�todo de entrada del estado de <c>PlayerMovementState</c>.
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
    /// M�todo de lectura de entrada de los inputs.
    ///Lee la entrada del Player.
    /// </summary>
    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    /// <summary>
    /// M�todo que actualiza la l�gica del Player.
    /// Si el escudo est� activo, actualiza el tiempo que puede estar activo.
    /// Verifica si hay enemigos dentro del rango.
    /// </summary>
    public virtual void UpdateLogic()
    {
        if(startActiveShield)
            UpdateTimeWithShield();

        EnemyInRange();
    }

    /// <summary>
    /// M�todo que actualiza las f�sicas del juego.
    /// Mueve al jugador.
    /// </summary>
    public virtual void UpdatePhysics()
    {
        Move();
    }

    /// <summary>
    /// M�todo que recibe la entrada de colisiones de triggers del mundo.
    /// </summary>
    /// <param name="collider">El collider con el que choca el Player.</param>
    public virtual void OnTriggerEnter(Collider collider) {}

    /// <summary>
    /// M�todo que recibe la salida de colisiones de triggers del mundo.
    /// </summary>
    /// <param name="collider">El collider del que sale el Player.</param>
    public virtual void OnTriggerExit(Collider collider) { }

    /// <summary>
    /// M�todo de salida del estado de <c>PlayerMovementState</c>.
    /// Se desuscriben las entradas del Input System y los eventos.
    /// </summary>
    public virtual void Exit()
    {
        EventsManager.StopCallSpecialEvents<float>("OnAttackPlayer", TakeDamage);
        EventsManager.StopCallNormalEvents("PickUpItem", PickUp);
        RemoveInputActionsCallbacks();
    }
    #endregion

    #region M�todos Suscripci�n Acciones Input System
    /// <summary>
    /// M�todo donde se suscriben las acciones de los inputs correspondientes.
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
    /// M�todo donde se desuscriben las acciones de los inputs correspondientes.
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
    /// M�todo que lee el valor de la entrada de movimiento del jugador.
    /// Asigna dicho valor a la variable de movimiento en el estado actual de movimiento.
    /// </summary>
    public void ReadMovementInput()
    {
        stateMachine.MovementData.MovementInput = stateMachine.Player.PlayerInput.PlayerActions.Movement.ReadValue<Vector2>();
    }
    #endregion

    #region M�todos Gesti�n Animaciones
    /// <summary>
    /// M�todo que activa la animaci�n correspondiente en el Animator.
    /// </summary>
    /// <param name="hashNumAnimation">N�mero (hash) que identifica la animaci�n que debe activarse en el Animator.</param>
    protected void StartAnimation(int hashNumAnimation)
    {
        stateMachine.Player.AnimPlayer.SetBool(hashNumAnimation, true);
    }

    /// <summary>
    /// M�todo que desactiva la animaci�n correspondiente en el Animator.
    /// </summary>
    /// <param name="hashNumAnimation">N�mero (hash) que identifica la animaci�n que debe desactivarse en el Animator.</param>
    protected void StopAnimation(int hashNumAnimation)
    {
        stateMachine.Player.AnimPlayer.SetBool(hashNumAnimation, false);
    }
    #endregion

    #region M�todos F�sicas de Movimiento
    /// <summary>
    /// M�todo que gestiona el movimiento del personaje seg�n la direcci�n y velocidad actual.
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
    /// M�todo que rota al personaje hacia la direcci�n del movimiento.
    /// </summary>
    /// <param name="_movementDirection">Direcci�n hacia la que se debe orientar el personaje.</param>
    public void Rotate(Vector3 _movementDirection)
    {
        if (_movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_movementDirection); // Hace que el personaje gire en la direcci�n donde se produce el movimiento.
            stateMachine.Player.RbPlayer.rotation = Quaternion.Slerp(stateMachine.Player.RbPlayer.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    /// <summary>
    /// M�todo que devuelve la direcci�n del input de movimiento en un Vector3.
    /// </summary>
    /// <returns>Un Vector3 que representa la direcci�n de movimiento en los ejes X y Z, ignorando el eje Y.</returns>
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.MovementData.MovementInput.x, 0f, stateMachine.MovementData.MovementInput.y);
    }

    /// <summary>
    /// M�todo que calcula y devuelve la velocidad actual del personaje.
    /// </summary>
    /// <returns>Devuelve un float que representa la velocidad del personaje.</returns>
    protected float GetMovementSpeed()
    {
        float movementSpeed = groundedData.BaseSpeed * stateMachine.MovementData.MovementSpeedModifier;
        return movementSpeed;
    }
    #endregion

    #region M�todo Comprobar si Player Toca Suelo
    /// <summary>
    /// M�todo que devuelve True/False para comprobar si Player ha tocado suelo o no.
    /// </summary>
    /// <returns>Si detecta que Player toca un elemento con la layer de "Enviroment" devuelve True, sino, False.</returns>
    protected virtual bool IsGrounded()
    {
        Vector3 boxCenter = stateMachine.Player.GroundCheckCollider.transform.position;
        Vector3 boxHalfExtents = new Vector3(0.5f, 0.1f, 0.55f); // Tama�o de la caja.
        Quaternion boxOrientation = Quaternion.identity; // Mantener la rotaci�n como la del GroundCheckCollider.
        LayerMask groundMask = stateMachine.Player.LayerData.EnviromentLayer;

        bool isGrounded = Physics.CheckBox(boxCenter, boxHalfExtents, boxOrientation, groundMask, QueryTriggerInteraction.Ignore);

        return isGrounded;
    }
    #endregion

    #region M�todos para Sobrescribir
    /// <summary>
    /// M�todo virtual que cancela el movimiento de Player.
    /// Se crea en este estado sin l�gica, se sobreescriben en otros estados que hereden de <c>PlayerMovementState</c>.
    /// </summary>
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) { }

    /// <summary>
    /// M�todo virtual que comprueba si una animaci�n ha terminado o no.
    /// Se crea en este estado sin l�gica, se sobreescriben en otros estados que hereden de <c>PlayerMovementState</c>.
    /// </summary>
    protected virtual void FinishAnimation() { }
    #endregion

    #region M�todos de Llamadas de Eventos
    /// <summary>
    /// M�todo que cambia el estado del jugador a PickUpState.
    /// </summary>
    private void PickUp()
    {
        stateMachine.ChangeState(stateMachine.PickUpState);
    }
    #endregion

    #region M�todos Interacci�n Bestia
    /// <summary>
    /// M�todo que cambia al estado de llamar a la Bestia.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    private void CallBeast(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.CallBeastState);
    }
    #endregion

    #region M�todos Interactions Enemies
    /// <summary>
    /// Cambia el objetivo fijado al siguiente m�s cercano dentro de un l�mite.
    /// Si no hay enemigos se actualiza la lista.
    /// Despu�s del �ltimo objetivo de la lista, deja de fijar.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
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
    /// Comprueba si el enemigo se mantiene dentro del rango de detecci�n para poder fijarle.
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
    /// Actualiza la lista de enemigos posibles dentro del rango de detecci�n de Player.
    /// Limpia la lista actual y a�ade los objetos que tengan el tag "Enemy" y que est�n dentro del �rea.
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
    /// Verifica si el enemigo actualmente fijado sigue dentro del rango de detecci�n.
    /// Si el enemigo es nulo o est� fuera de rango, se elimina el objetivo fijado y se reinicia el �ndice.
    /// </summary>
    private void EnemyInRange()
    {
        if (currentLockTarget >= 0 && currentLockTarget < enemiesTarget.Count)
        {
            currentTarget = enemiesTarget[currentLockTarget];

            if (currentTarget == null || Vector3.Distance(stateMachine.Player.transform.position, currentTarget.transform.position) > detectionRange)
            {
                //Debug.Log("El enemigo fijado se sali� del rango.");
                stateMachine.Player.pointTarget.ClearTarget();
                currentLockTarget = -1;
            }
        }
    }

    /// <summary>
    /// M�todo que disminuye la salud del jugador en funci�n del da�o recibido y cambia al estado de Medio-Muerta si la salud llega a cero.
    /// </summary>
    /// <param name="_enemyDamage">Da�o recibido por parte del enemigo.</param>
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

    #region M�todos Defensa
    /// <summary>
    /// M�todo para que se active el escudo de Player.
    /// Comienza el tiempo que puede estar el escudo activo.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void OnDefendedStarted(InputAction.CallbackContext context)
    {
        shieldButtonPressed = true;
        startActiveShield = true;
        currentTimeWithShield = 0f;
        ActivateShield();
    }

    /// <summary>
    /// M�todo para que se desactive el escudo de Player.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected virtual void OnDefendedCanceled(InputAction.CallbackContext context)
    {
        shieldButtonPressed = false;
        startActiveShield = false;
        DesactivateShield();
    }

    /// <summary>
    /// Actualiza el tiempo que puede estar Player con el escudo activo.
    /// Si pasa del tiempo m�ximo, se desactiva el escudo.
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

    #region M�todos Cambiar Expresiones Player
    /// <summary>
    /// Se crea un diccionario para almacenar la informaci�n de los materiales de la cara de Player.
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
    /// Verifica si el diccionario de materiales faciales del jugador est� creado; si no, lo crea.
    /// </summary>
    protected virtual void ChangeFacePlayer()
    {
        if (materialFacePlayer == null)
            CreateFaceMaterialPlayerDictionary();        
    }

    /// <summary>
    /// M�todo para indicar el material que se quiere cambiar y las coordenadas del cambio.
    /// </summary>
    /// <param name="materialIndex">�ndice del material (para diferenciar boca, ojos y cejas) del diccionario.</param>
    /// <param name="offset">Coordenadas a las que se va a desplazar el material para cambiar la expresi�n facial.</param>
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

    #region M�todos Cursor
    /// <summary>
    /// M�todo para bloquear el cursor y hacerlo invisible.
    /// </summary>
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// M�todo para desbloquear el cursor y hacerlo visible.
    /// </summary>
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion

    #region M�todo PlayerMorir
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