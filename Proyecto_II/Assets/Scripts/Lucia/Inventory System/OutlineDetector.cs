using UnityEngine;

public class OutlineDetector : MonoBehaviour
{
    [Header("Configuración Visual")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color farColor = Color.gray; // Color cuando está lejos
    [SerializeField] private float farSize = 0.1f;

    private Material outlineMaterial;
    private Color originalColor;
    private float originalSize;
    private Color highlightColor = Color.white; // Color del outline brillante
    private float highlightSize = 0.2f; // Tamaño del outline brillante

    private void Start()
    {
        if (targetRenderer != null)
        {
            outlineMaterial = Instantiate(targetRenderer.materials[targetRenderer.materials.Length - 1]);
            Material[] mats = targetRenderer.materials;
            mats[mats.Length - 1] = outlineMaterial;
            targetRenderer.materials = mats;

            originalColor = outlineMaterial.color;
            originalSize = outlineMaterial.GetFloat("_Size");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SetOutline(farColor, farSize); // Outline tenue al entrar en rango lejano
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SetOutline(Color.clear, 0f); // Desactiva outline cuando sale del rango
    }

    public void HighlightForPickup(bool highlight)
    {
        if (highlight)
            SetOutline(highlightColor, highlightSize); // Outline brillante para recoger
        else
            SetOutline(farColor, farSize); // Vuelve al outline tenue
    }

    public void SetOutline(Color color, float size)
    {
        if (outlineMaterial != null)
        {
            outlineMaterial.color = color;
            outlineMaterial.SetFloat("_Size", size);
        }
    }
}