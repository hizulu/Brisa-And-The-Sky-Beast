using UnityEngine;
using UnityEngine.InputSystem;

/* NOMBRE CLASE: Player Movement
 * AUTOR: Jone Sainz Egea
          Sara Yue Madruga Mart�n
 * FECHA: 09/11/2024
 * DESCRIPCI�N: Script base que se encarga del movimiento del personaje jugable usando el New Input System
 * VERSI�N: 1.0 movimiento base con W/A/S/D
 *              1.1 rotaci�n al girar
 *              1.2 rotaci�n del player junto con la c�mara
 *          2.0 salto
 *          3.0 correr
 *          4.0 animaciones
 */

public class PlayerMovement : MonoBehaviour
{
    #region Movements Variables
    Rigidbody rb;
    Animator anim;
    [Header("Movement Settings")]
    [SerializeField] float baseSpeed = 5f;
    [SerializeField] float movementSpeedMultiplier = 1f;
    [SerializeField] float rotationSpeed = 15f;
    private float currentSpeed;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float crouchedSpeed = 0.25f;

    [SerializeField] private Transform camTransform;
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
    [SerializeField] InputActionReference runAction;
    [SerializeField] InputActionReference crouchedAction;
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
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        PlayerRun();
        PlayerCrouched();
        PlayerWalk();
    }

    /* NOMBRE M�TODO: PlayerWalk
     * AUTOR: Jone Sainz Egea
              Sara Yue Madruga Mart�n
     * FECHA: 09/11/2024
     * DESCRIPCI�N: lee el valor de la acci�n de andar del playerInput
     *              rota al jugador para que mire en la direcci�n en la que va a andar
     *              mueve al jugador teniendo en cuenta la velocidad base y el multiplicador de velocidad
     *              rota al jugador en base a la rotaci�n de la c�mara
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
            newPosition = Quaternion.AngleAxis(camTransform.rotation.eulerAngles.y, Vector3.up) * newPosition;
            Quaternion targetRotation = Quaternion.LookRotation(newPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            Debug.Log("Est�s andando" + " " + currentSpeed);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        transform.position += newPosition * movementSpeedMultiplier * currentSpeed * Time.deltaTime;
    }

    void PlayerRun()
    {
        if (runAction.action.IsPressed())
        {
            anim.SetBool("isRunning", true);
            currentSpeed = runSpeed;
            Debug.Log("Est�s corriendo" + " " + currentSpeed);
        }
        else
        {
            currentSpeed = baseSpeed;
            anim.SetBool("isRunning", false);
        }
    }

    void PlayerCrouched()
    {
        if (crouchedAction.action.IsPressed())
        {
            anim.SetBool("isCrouching", true);
            currentSpeed = crouchedSpeed;
            Debug.Log("Est�s en sigilo" + " " + currentSpeed);
        }
        else
        {
            currentSpeed = baseSpeed;
            anim.SetBool("isCrouching", false);
        }
    }

    /* NOMBRE M�TODO: Jump
     * AUTOR: Jone Sainz Egea
     * FECHA: 09/11/2024
     * DESCRIPCI�N: si est� en el suelo, a�ade impulso de salto cuando se llama a la acci�n de salto
     * @param: contexto de salto
     * @return: - 
     */
    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            anim.SetTrigger("jump");
            //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Est�s saltando");
        }
    }

    /* NOMBRE M�TODO: IsGrounded
     * AUTOR: Jone Sainz Egea
     * FECHA: 09/11/2024
     * DESCRIPCI�N: comprueba si el jugador est� en contacto con la capa del suelo
     * @param: -
     * @return: true si est� en el suelo, false si no
     */
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }
}
