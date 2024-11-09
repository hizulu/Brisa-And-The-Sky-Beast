using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* NOMBRE CLASE: Player Movement
 * AUTOR: Jone Sainz Egea
 * FECHA: 09/11/2024
 * DESCRIPCIÓN: Script base que se encarga del movimiento del personaje jugable usando el New Input System
 * VERSIÓN: 1.0 movimiento base con W/A/S/D
 *              1.1 rotación al girar
 *          2.0 salto
 *          3.0 animaciones
 */

public class PlayerMovement : MonoBehaviour
{
    #region Movement Variables
    Rigidbody rb;
    Animator anim;
    [Header("Movement Settings")]
    [SerializeField] float baseSpeed = 5f;
    [SerializeField] float movementSpeedMultiplier = 1f;
    [SerializeField] float rotationSpeed = 15f;
    #endregion

    #region Jump Variables
    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] private float groundCheckRadius = 0.2f; 
    [SerializeField] private Transform groundCheckPoint; 
    [SerializeField] private LayerMask groundLayer;
    #endregion

    #region New Input System Variables
    PlayerInput playerInput;
    [Space(10)]
    [Header("Inputs")]
    [SerializeField] InputActionReference walkAction;
    [SerializeField] InputActionReference jumpAction;
    #endregion

    private void OnEnable()
    {
        jumpAction.action.started += Jump;
    }

    private void OnDisable()
    {
        jumpAction.action.started -= Jump;
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerWalk();
    }

    /* NOMBRE MÉTODO: PlayerWalk
     * AUTOR: Jone Sainz Egea
     * FECHA: 09/11/2024
     * DESCRIPCIÓN: lee el valor de la acción de andar del playerInput
     *              rota al jugador para que mire en la dirección en la que va a andar
     *              mueve al jugador teniendo en cuenta la velocidad base y el multiplicador de velocidad
     * @param: -
     * @return: - 
     */
    void PlayerWalk()
    {
        Vector2 direction = walkAction.action.ReadValue<Vector2>();
        Vector3 newPosition = new Vector3(direction.x, 0, direction.y);

        if (newPosition != Vector3.zero)
        {
            anim.SetBool("isWalking", true);
            Quaternion targetRotation = Quaternion.LookRotation(newPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        } else
        {
            anim.SetBool("isWalking", false);
        }

        transform.position += newPosition * movementSpeedMultiplier * baseSpeed * Time.deltaTime;
    }

    /* NOMBRE MÉTODO: Jump
     * AUTOR: Jone Sainz Egea
     * FECHA: 09/11/2024
     * DESCRIPCIÓN: si está en el suelo, añade impulso de salto cuando se llama a la acción de salto
     * @param: contexto de salto
     * @return: - 
     */
    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            anim.SetTrigger("jump");
            //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /* NOMBRE MÉTODO: IsGrounded
     * AUTOR: Jone Sainz Egea
     * FECHA: 09/11/2024
     * DESCRIPCIÓN: comprueba si el jugador está en contacto con la capa del suelo
     * @param: -
     * @return: true si está en el suelo, false si no
     */
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }
}
