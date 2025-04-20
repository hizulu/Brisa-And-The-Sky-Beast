#region Bibliotecas
using UnityEngine.InputSystem;
using UnityEngine;
#endregion

/* NOMBRE CLASE: Pick Up Weapons
 * AUTOR: Lucía García López
 * FECHA: 05/04/2025
 * DESCRIPCIÓN: Script que se encarga de recoger las armas en el juego.
 * VERSIÓN: 1.0
 */

public class PickUpWeapons : MonoBehaviour
{
    #region Variables
    private bool playerInRange = false;
    private Weapon weaponScript;
    private Renderer outline;
    private Material outlineMaterial;
    private Player player;
    private Color outlineOriginalColor;
    private Color highlightColor = Color.white;

    [SerializeField] private WeaponSlot weaponSlot;
    #endregion

    void Start()
    {
        player = FindObjectOfType<Player>();
        outline = GetComponentInChildren<Renderer>();
        weaponScript = GetComponent<Weapon>();

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
            Debug.LogError("No se encontró Renderer o materiales en el objeto o sus hijos");
        }

        player.PlayerInput.UIPanelActions.PickUpItem.performed += PickUpWeapon;
    }

    private void OnDestroy()
    {
        player.PlayerInput.UIPanelActions.PickUpItem.performed -= PickUpWeapon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ChangeOutline(highlightColor, 0.1f);
            //Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ChangeOutline(outlineOriginalColor, 0.01f);
            //Debug.Log("Player out of range");
        }
    }

    private void PickUpWeapon(InputAction.CallbackContext context)
    {
        if (playerInRange && weaponScript != null)
        {
            // Recoger el arma
            weaponScript.CollectWeapon();
            ChangeOutline(outlineOriginalColor, 0.01f);

            // Si el jugador ya tiene un arma equipada, reemplázala
            if (weaponSlot.HasWeapon())
            {
                weaponSlot.SetWeapon(weaponScript.weaponData); // Actualiza el slot con el nuevo arma
                Debug.Log("Reemplazando el arma antigua con la nueva");
            }
            else
            {
                weaponSlot.SetWeapon(weaponScript.weaponData); // Si no tiene arma, asigna la nueva
                Debug.Log("Arma nueva equipada");
            }

            if (weaponScript.weaponData.weaponName == "Palo")
                player.PaloRecogido();

            Debug.Log("Palo collected");
        }
    }

    private void ChangeOutline(Color newColor, float outlineSize)
    {
        if (outlineMaterial != null)
        {
            outlineMaterial.color = newColor;

            if (outlineMaterial.HasProperty("_Size"))
            {
                outlineMaterial.SetFloat("_Size", outlineSize);
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el material del outline");
        }
    }
}
