using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
 *          5.0 implementaci�n de tutoriales
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
    [SerializeField] InputActionReference attackAction;
    #endregion

    #region Tutorial Variables
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI textTutorial;
    private bool isWalkTutoActive = true;
    private bool isRunTutoActive = true;
    private bool isJumpTutoActive = true;
    private bool isAttackTutoActive = true;
    #endregion

    private void OnEnable()
    {
        jumpAction.action.started += Jump;
        attackAction.action.started += Attack;
    }

    private void OnDisable()
    {
        jumpAction.action.started -= Jump;
        attackAction.action.started -= Attack;
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentSpeed = baseSpeed;
        tutorialPanel.SetActive(false);
        StartCoroutine(WalkTutorial());
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
        if (isWalkTutoActive)
            return;

        Vector2 direction = walkAction.action.ReadValue<Vector2>();
        Vector3 newPosition = new Vector3(direction.x, 0, direction.y);

        if (newPosition != Vector3.zero)
        {
            anim.SetBool("isWalking", true);
            newPosition = Quaternion.AngleAxis(camTransform.rotation.eulerAngles.y, Vector3.up) * newPosition;
            Quaternion targetRotation = Quaternion.LookRotation(newPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            //Debug.Log("Est�s andando" + " " + currentSpeed);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        transform.position += newPosition * movementSpeedMultiplier * currentSpeed * Time.deltaTime;
    }

    void PlayerRun()
    {
        if (isRunTutoActive)
            return;

        if (runAction.action.IsPressed())
        {
            anim.SetBool("isRunning", true);
            currentSpeed = runSpeed;
            //Debug.Log("Est�s corriendo" + " " + currentSpeed);
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
            //Debug.Log("Est�s en sigilo" + " " + currentSpeed);
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
        if (isJumpTutoActive)
            return;

        if (IsGrounded())
        {
            anim.SetTrigger("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //Debug.Log("Est�s saltando");
        }
    }

    /* NOMBRE M�TODO: Attack
     * AUTOR: Sara Yue Madruga Mart�n
     * FECHA: 10/11/2024
     * DESCRIPCI�N: 
     * @param: 
     * @return: - 
     */
    private void Attack(InputAction.CallbackContext context)
    {
        if (isAttackTutoActive)
            return;

        if (attackAction.action.IsPressed())
        {
            anim.SetBool("isAttacking", true);
            anim.SetTrigger("attack");
            Debug.Log("Est�s realizando el ataque 1");
        }
        else
        {
            anim.SetBool("isAttacking", false);
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

    #region Coroutines Tutoriales

    /* NOMBRE FUNCI�N: WalkTutorial
     * AUTOR: Sara Yue Madruga Mart�n
     * FECHA: 29/11/2024
     * DESCRIPCI�N: activa el panel del tutorial de caminar despu�s de 2 segundos desde que se ha llamado a la corrutina.
                    hasta que no se realiza la acci�n de "caminar" no desactiva el panel del tutorial y activa la siguiente corrutina, la de correr.
     * @param: -
     * @return: -
     */
    IEnumerator WalkTutorial()
    {
        tutorialPanel.GetComponent<Image>().color = Color.gray;
        yield return new WaitForSecondsRealtime(2f);
        tutorialPanel.SetActive(true);
        textTutorial.text = "Para desplazarte utiliza las teclas W A S D.";

        yield return new WaitUntil(() => walkAction.action.IsPressed()); // El tutorial de caminar no se desactiva hasta que no se realiza la acci�n de "walkAction".
        StartCoroutine(CheckTutorial(tutorialPanel.GetComponent<Image>().color, Color.green, 0.5f));
        isWalkTutoActive = false;

        yield return new WaitForSecondsRealtime(2f);
        tutorialPanel.SetActive(false);
        Debug.Log("Tutorial de caminar terminado.");
        StartCoroutine(RunTutorial());
    }

    /* NOMBRE FUNCI�N: RunTutorial
     * AUTOR: Sara Yue Madruga Mart�n
     * FECHA: 29/11/2024
     * DESCRIPCI�N: activa el panel del tutorial de correr despu�s de 2 segundos desde que se ha llamado a la corrutina.
                    hasta que no se realiza la acci�n de "correr" no desactiva el panel del tutorial, detiene la corrutina de caminar y activa la siguiente corrutina, la de saltar.
     * @param: -
     * @return: -
     */
    IEnumerator RunTutorial()
    {
        tutorialPanel.GetComponent<Image>().color = Color.gray;
        yield return new WaitForSecondsRealtime(2f);
        tutorialPanel.SetActive(true);
        textTutorial.text = "Para correr utiliza las teclas W A S D y mant�n pulsado SHIFT izquierdo.";

        yield return new WaitUntil(() => runAction.action.IsPressed()); // El tutorial de correr no se desactiva hasta que no se realiza la acci�n de correr (pulsar LEFT SHIFT).
        StartCoroutine(CheckTutorial(tutorialPanel.GetComponent<Image>().color, Color.green, 0.5f));
        isRunTutoActive = false;
        StopCoroutine(WalkTutorial());

        yield return new WaitForSecondsRealtime(2f);
        tutorialPanel.SetActive(false);
        Debug.Log("Tutorial de correr terminado.");
        StartCoroutine(JumpTutorial());
    }

    /* NOMBRE FUNCI�N: JumpTutorial
     * AUTOR: Sara Yue Madruga Mart�n
     * FECHA: 29/11/2024
     * DESCRIPCI�N: activa el panel del tutorial de saltar despu�s de 2 segundos desde que se ha llamado a la corrutina.
                    hasta que no se realiza la acci�n de "saltar" no desactiva el panel del tutorial, detiene la corrutina de correr y sale de la misma.
     * @param: -
     * @return: -
     */
    IEnumerator JumpTutorial()
    {
        tutorialPanel.GetComponent<Image>().color = Color.gray;
        yield return new WaitForSecondsRealtime(2f);
        tutorialPanel.SetActive(true);
        textTutorial.text = "Para saltar, pulsa la barra espaciadora.";

        yield return new WaitUntil(() => jumpAction.action.IsPressed()); // El tutorial de saltar no se desactiva hasta que no se realiza la acci�n de saltar.
        StartCoroutine(CheckTutorial(tutorialPanel.GetComponent<Image>().color, Color.green, 0.5f));
        isJumpTutoActive = false;
        StopCoroutine(RunTutorial());

        yield return new WaitForSecondsRealtime(2f);
        tutorialPanel.SetActive(false);
        
        Debug.Log("Tutorial de saltar terminado.");
        StartCoroutine(AttackTutorial());
    }

    IEnumerator AttackTutorial()
    {
        tutorialPanel.GetComponent<Image>().color = Color.gray;
        yield return new WaitForSecondsRealtime(2f);
        tutorialPanel.SetActive(true);
        textTutorial.text = "Para atacar haz click izquierdo con el rat�n.";

        yield return new WaitUntil(() => attackAction.action.IsPressed()); // El tutorial de atacar no se desactiva hasta que no se realiza la acci�n de atacar.
        StartCoroutine(CheckTutorial(tutorialPanel.GetComponent<Image>().color, Color.green, 0.5f));
        isAttackTutoActive = false;
        StopCoroutine(JumpTutorial());

        yield return new WaitForSecondsRealtime(2f);
        tutorialPanel.SetActive(false);

        Debug.Log("Tutorial de saltar terminado.");
        yield break;
    }

    IEnumerator CheckTutorial(Color color1, Color color2, float tiempoCambio)
    {
        Image tutoImage = tutorialPanel.GetComponent<Image>();
        float tiempoPasado = 0f;
        tutoImage.color = color1;

        while(tiempoPasado < tiempoCambio)
        {
            tutoImage.color = Color.Lerp(color1, color2, tiempoPasado/tiempoCambio);
            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        tutoImage.color = color2;
    }
    #endregion
}
