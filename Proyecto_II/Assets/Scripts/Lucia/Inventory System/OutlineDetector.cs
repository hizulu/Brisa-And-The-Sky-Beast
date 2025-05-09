using UnityEngine;

/*
 * NOMBRE CLASE: OutlineDetector
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 01/05/2025
 * DESCRIPCI�N: Clase que gestiona el efecto de contorno (outline) en un objeto al entrar en el rango del jugador.
 *              Permite cambiar el color y tama�o del contorno seg�n la distancia al jugador.
 *              Cuando el jugador entra en el rango para poder recoger el objeto, se cambia el color y tama�o del contorno.
 * VERSI�N: 1.0 Sistema de contorno inicial.
 */

public class OutlineDetector : MonoBehaviour
{
    #region Variables
    [Header("Configuraci�n Visual")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color farColor = Color.gray; // Color cuando est� lejos
    [SerializeField] private float farSize = 0.1f;

    private Material outlineMaterial;
    private Color originalColor;
    private float originalSize;
    private Color highlightColor = Color.white; // Color del outline brillante
    private float highlightSize = 0.2f; // Tama�o del outline brillante
    #endregion

    private void Start()
    {
        if (targetRenderer != null)
        {
            outlineMaterial = Instantiate(targetRenderer.materials[targetRenderer.materials.Length - 1]);
            // Asignar el material de contorno al �ltimo material del objeto
            Material[] mats = targetRenderer.materials;
            mats[mats.Length - 1] = outlineMaterial;
            targetRenderer.materials = mats;

            // Guardar el color y tama�o originales
            originalColor = outlineMaterial.color;
            originalSize = outlineMaterial.GetFloat("_Size");
        }
    }

    //Si el jugador entra en el rango del objeto, se activa el contorno.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SetOutline(farColor, farSize);
    }

    //Si el jugador sale del rango del objeto, se desactiva el contorno.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SetOutline(Color.clear, 0f);
    }

    //Si el jugador est� en rango para recoger y presiona la tecla de interacci�n, se activa o desactiva el contorno.
    public void HighlightForPickup(bool highlight)
    {
        if (highlight)
            SetOutline(highlightColor, highlightSize);
        else
            SetOutline(farColor, farSize);
    }

    //M�todo para cambiar el color y tama�o del contorno
    public void SetOutline(Color color, float size)
    {
        if (outlineMaterial != null)
        {
            outlineMaterial.color = color;
            outlineMaterial.SetFloat("_Size", size);
        }
    }
}