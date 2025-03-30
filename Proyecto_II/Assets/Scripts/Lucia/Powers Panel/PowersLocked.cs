using UnityEngine;
using TMPro;

public class PowersLocked : MonoBehaviour
{
    [SerializeField] private TMP_Text brisaPowerNameLocked;   // Nombre Brisa en panel bloqueado
    [SerializeField] private TMP_Text bestiaPowerNameLocked;  // Nombre Bestia en panel bloqueado
    [SerializeField] private TMP_Text brisaPowerNameUnlocked; // Nombre Brisa en panel desbloqueado
    [SerializeField] private TMP_Text bestiaPowerNameUnlocked;// Nombre Bestia en panel desbloqueado
    [SerializeField] private TMP_Text whereToFind;

    [SerializeField] private TMP_FontAsset unlockedFont; // Santana
    [SerializeField] private TMP_FontAsset lockedFont;   // Fenara

    public void UpdatePowerNamesFont(bool isUnlocked)
    {
        // Fuentes para los textos en el panel bloqueado
        brisaPowerNameLocked.font = isUnlocked ? unlockedFont : lockedFont;
        bestiaPowerNameLocked.font = isUnlocked ? unlockedFont : lockedFont;

        // Fuentes para los textos en el panel desbloqueado
        brisaPowerNameUnlocked.font = unlockedFont;
        bestiaPowerNameUnlocked.font = unlockedFont;

        // Fuente para "Dónde encontrarlo" (siempre Santana)
        whereToFind.font = unlockedFont;
    }
}