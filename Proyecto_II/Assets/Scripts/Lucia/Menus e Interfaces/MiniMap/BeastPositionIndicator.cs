using UnityEngine;
using UnityEngine.UI;

/*
 * NOMBRE CLASE: BeastPositionIndicator
 * AUTOR: Lucía García López
 * FECHA: 20/04/2025
 * DESCRIPCIÓN: Script que gestiona la posición del indicador de la bestia en el minimapa. 
 *              Si la bestia está fuera del minimapa aparece un indicador de su posición que va desplazandose alrededor del mismo 
 *              dependiendo de la posicion de la bestia.
 * VERSIÓN: 1.0 Sistema de minimapa inicial.
 * 1.1 Efecto de fade para el indicador de la bestia.
 */

public class BeastPositionIndicator : MonoBehaviour
{
    #region Variables
    [Header("Configuración")]
    [SerializeField] private float minimapRadius = 40f;
    [SerializeField] private float indicatorRadius = 100f;
    [SerializeField] private float fadeSpeed = 4f; // Velocidad de fade (mayor = más rápido)

    [Header("Referencias")]
    [SerializeField] private RectTransform indicatorRect;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private Beast beastScript;

    private Transform player;
    private BeastTrapped beastTrapped;
    private float targetAlpha = 0f;
    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //Se tiene que revisar si la bestia está en estado de libertado o no
        beastTrapped = FindAnyObjectByType<BeastTrapped>();

        // Configuración inicial
        indicatorImage.color = new Color(1, 1, 1, 0);
        indicatorRect.gameObject.SetActive(true); // Siempre activo
    }

    private void Update()
    {
        // Control de visibilidad
        bool shouldShow = beastTrapped.beasIsFree &&
                         Vector3.Distance(beastScript.transform.position, player.position) > minimapRadius;

        targetAlpha = shouldShow ? 1f : 0f;

        // Aplicar fade suave
        float currentAlpha = indicatorImage.color.a;
        float newAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
        indicatorImage.color = new Color(1, 1, 1, newAlpha);

        // Actualizar posición si es visible
        if (newAlpha > 0.01f)
        {
            Vector3 direction = beastScript.transform.position - player.position;
            direction.y = 0;
            UpdatePosition(direction.normalized);
        }
    }

    //El indicador de posicion de la bestia gira entorno al minimapa indicando en que direccion esta la Bestia
    private void UpdatePosition(Vector3 direction)
    {
        Vector2 dir = new Vector2(direction.x, direction.z);
        indicatorRect.anchoredPosition = dir * indicatorRadius;
        indicatorRect.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
    }
}