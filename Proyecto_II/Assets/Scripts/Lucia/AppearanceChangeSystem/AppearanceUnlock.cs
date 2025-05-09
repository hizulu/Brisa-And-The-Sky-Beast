using TMPro;
using UnityEngine;

/* NOMBRE CLASE: Appearance Unlock
 * AUTOR: Lucía García López
 * FECHA: 04/05/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar el desbloqueo de apariencias.
 * VERSIÓN: 1.0 Sistema de desbloqueo de apariencias.
 */

public class AppearanceUnlock : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool debugLogs = true;

    #region Singleton
    public static AppearanceUnlock Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public bool TryUnlockAppearance(AppearanceChangeData appearanceData)
    {
        if (appearanceData == null) return false;
        //Debug.Log("Bandera 1");

        // Verificar si ya está desbloqueado
        if (appearanceData.isUnlocked)
        {
            if (debugLogs) Debug.Log($"[Appearance] {appearanceData.appearanceName} ya estaba desbloqueado");
            //Debug.Log("Bandera 2");
            return false;
        }
        
        // Verificar requisitos
        bool canUnlock = CheckUnlockRequirements(appearanceData);
        //Debug.Log("Bandera 3");

        if (canUnlock)
        {
            appearanceData.isUnlocked = true;
            //Debug.Log("Bandera 4");
            if (debugLogs) Debug.Log($"[Appearance] Desbloqueado: {appearanceData.appearanceName}");
            AppearanceUIManager.Instance?.UpdateAppearanceUI(appearanceData); // Actualizar UI de apariencia
            //Debug.Log("Bandera 5");
            // Notificar otros sistemas
            EventsManager.TriggerSpecialEvent("OnAppearanceUnlocked", appearanceData);
            //Debug.Log("Bandera 6");
            AppearanceUIManager.Instance?.UpdateAppearanceUI(appearanceData); // Actualizar UI de apariencia
            //Debug.Log("Bandera 7");
            return true;
        }

        //Debug.Log("Bandera 8");
        return false;
    }

    private bool CheckUnlockRequirements(AppearanceChangeData data)
    {
        // Caso especial: sin requisitos
        if (data.objectsNeededPrefab == null || data.toUnlockQuantity <= 0)
        {
            if (debugLogs) Debug.Log($"[Appearance] {data.appearanceName} no requiere ítems");
            return true;
        }

        // Verificar inventario
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("[Appearance] InventoryManager no encontrado");
            return false;
        }

        int currentQuantity = InventoryManager.Instance.GetItemQuantity(data.objectsNeededPrefab);
        bool hasEnough = currentQuantity >= data.toUnlockQuantity;

        if (debugLogs && !hasEnough)
        {
            Debug.Log($"[Appearance] Faltan {data.toUnlockQuantity - currentQuantity} {data.objectsNeededPrefab.itemName} para {data.appearanceName}");
        }

        return hasEnough;
    }

    // Método para comprobar múltiples apariencias
    public void CheckMultipleAppearances(AppearanceChangeData[] appearancesToCheck)
    {
        foreach (var appearance in appearancesToCheck)
        {
            TryUnlockAppearance(appearance);
        }
    }
}