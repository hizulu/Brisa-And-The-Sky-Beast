#region Bibliotecas
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

/* NOMBRE CLASE: Pick Up Items
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 13/03/2025
 * DESCRIPCI�N: Script que se encarga de recoger objetos del escenario.
 * VERSI�N: 1.1 
 * 1.1 ChangeOutline: Cambiar el color y el tama�o del outline.
 */

public class PickUpItems : MonoBehaviour
{
    #region Variables
    private bool playerInRange = false;
    private Item itemScript;
    private Renderer outline;
    private Material outlineMaterial;
    private Player player;
    private Color outlineOriginalColor;
    private Color highlightColor=Color.white;
    #endregion

    void Start()
    {
        player = FindObjectOfType<Player>();
        outline = GetComponentInChildren<Renderer>();
        itemScript = GetComponent<Item>();

        // Se instancia un material nuevo para evitar cambios en materiales compartidos.
        if (outline != null && outline.materials.Length > 0)
        {
            outlineMaterial = outline.materials[outline.materials.Length - 1];

            outlineMaterial = new Material(outlineMaterial);

            Material[] materials = outline.materials;
            materials[materials.Length - 1] = outlineMaterial;
            outline.materials = materials;

            outlineOriginalColor = outlineMaterial.color;

            if (outlineMaterial.HasProperty("_Size"))
            {
                outlineMaterial.SetFloat("_Size", 0.01f);
            }
        }
        else
        {
            Debug.LogError("No se encontr� Renderer o materiales en el objeto o sus hijos");
        }

        player.PlayerInput.PlayerActions.Interact.performed += PickUpItem; // Suscribirse a la acci�n de recoger objetos.
    }

    private void OnDestroy()
    {
        player.PlayerInput.PlayerActions.Interact.performed -= PickUpItem; // Desuscribirse a la acci�n de recoger objetos.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ChangeOutline(highlightColor, 0.1f); // Cambiar el color y el tama�o del outline.
            Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ChangeOutline(outlineOriginalColor, 0.01f);
            Debug.Log("Player out of range");
        }
    }

    private void PickUpItem(InputAction.CallbackContext context)
    {
        if (playerInRange && itemScript != null)
        {
            itemScript.CollectItem();
            ChangeOutline(outlineOriginalColor, 0.01f);
        }
    }

    private void ChangeOutline(Color newColor, float outlineSize)
    {
        if (outlineMaterial != null)
        {
            Debug.Log($"Cambiando outline - Color: {newColor}, Tama�o: {outlineSize}");

            // Cambiar el color
            if (outlineMaterial.HasProperty("_Color"))
            {
                outlineMaterial.color = newColor;
            }
            else
            {
                Debug.LogWarning("El material no tiene una propiedad '_Color'");
            }

            // Cambiar el tama�o del outline
            if (outlineMaterial.HasProperty("_Size"))
            {
                outlineMaterial.SetFloat("_Size", outlineSize);
            }
            else
            {
                Debug.LogWarning("El material no tiene una propiedad '_Size'");
            }
        }
        else
        {
            Debug.LogWarning("No se encontr� el material del outline");
        }
    }
}