using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* NOMBRE CLASE: Player Movement
 * AUTOR: Jone Sainz Egea
 * FECHA: 09/11/2024
 * DESCRIPCIÓN: Script base que se encarga del movimiento del personaje jugable usando el New Input System
 * VERSIÓN: 1.0 movimiento base con W/A/S/D
 */

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction walkAction;
    [SerializeField] float baseSpeed = 5f;
    [SerializeField] float movementSpeedMultiplier = 1f;
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        walkAction = playerInput.actions.FindAction("Walk");
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
        Vector2 direction = walkAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * movementSpeedMultiplier * baseSpeed * Time.deltaTime;
    }
}
