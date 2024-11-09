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
 *          2.0 salto
 */

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    [SerializeField] InputActionReference walkAction;
    [SerializeField] InputActionReference jumpAction;

    Rigidbody rb;

    [SerializeField] float baseSpeed = 5f;
    [SerializeField] float movementSpeedMultiplier = 1f;
    [SerializeField] float jumpForce = 5f;
    private bool isGrounded = true;

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
    }

    void Update()
    {
        PlayerWalk();
    }

    /* NOMBRE MÉTODO: PlayerWalk
     * AUTOR: Jone Sainz Egea
     * FECHA: 09/11/2024
     * DESCRIPCIÓN: lee el valor de la acción de andar del playerInput
     *              mueve al jugador teniendo en cuenta la velocidad base y el multiplicador de velocidad
     * @param: -
     * @return: - 
     */
    void PlayerWalk()
    {
        Vector2 direction = walkAction.action.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * movementSpeedMultiplier * baseSpeed * Time.deltaTime;
    }

    /* NOMBRE MÉTODO: Jump
     * AUTOR: Jone Sainz Egea
     * FECHA: 09/11/2024
     * DESCRIPCIÓN: añade impulso de salto cuando se llama a la acción de salto
     * @param: contexto de salto
     * @return: - 
     */
    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
            rb.AddForce(new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);
    }
}
