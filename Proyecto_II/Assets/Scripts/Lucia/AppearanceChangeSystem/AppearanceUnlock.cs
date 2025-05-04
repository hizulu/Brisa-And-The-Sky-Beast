using TMPro;
using UnityEngine;

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

        // Verificar si ya está desbloqueado
        if (appearanceData.isUnlocked)
        {
            if (debugLogs) Debug.Log($"[Appearance] {appearanceData.appearanceName} ya estaba desbloqueado");
            return false;
        }
        
        // Verificar requisitos
        bool canUnlock = CheckUnlockRequirements(appearanceData);
        
        if (canUnlock)
        {
            appearanceData.isUnlocked = true;
            if (debugLogs) Debug.Log($"[Appearance] Desbloqueado: {appearanceData.appearanceName}");
            AppearanceUIManager.Instance.UpdateAppearanceUI(appearanceData); // Actualizar UI de apariencia
            // Notificar otros sistemas
            EventsManager.TriggerSpecialEvent("OnAppearanceUnlocked", appearanceData);
            return true;
        }
        AppearanceUIManager.Instance.UpdateAppearanceUI(appearanceData); // Actualizar UI de apariencia
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