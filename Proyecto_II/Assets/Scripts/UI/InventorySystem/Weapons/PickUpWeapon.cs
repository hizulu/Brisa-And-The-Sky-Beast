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
    [Header("Configuración")]
    [SerializeField] private OutlineDetector outlineDetector;
    [SerializeField] private WeaponSlot weaponSlot;

    private bool playerInRange = false;
    private Player player;
    private Weapon weaponScript;
    #endregion

    void Start()
    {
        player = FindObjectOfType<Player>();
        weaponScript = GetComponent<Weapon>();

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
            outlineDetector.HighlightForPickup(true); // Destacar fuerte
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            outlineDetector.HighlightForPickup(false); // Volver a tenue
        }
    }

    private void PickUpWeapon(InputAction.CallbackContext context)
    {
        if (playerInRange && weaponScript != null)
        {
            // Recoger el arma
            weaponScript.CollectWeapon();

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

            if (weaponScript.weaponData.weaponName == "Baculo")
                EventsManager.TriggerNormalEvent("PickUpSceptre");

            Debug.Log("Palo collected");
        }
    }
}
