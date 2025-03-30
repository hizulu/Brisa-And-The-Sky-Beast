using UnityEngine;
using TMPro;

public class PowersLocked : MonoBehaviour
{
    [SerializeField] private TMP_Text brisaPowerName;
    [SerializeField] private TMP_Text bestiaPowerName;
    [SerializeField] private TMP_Text whereToFind;
    [SerializeField] private TMP_FontAsset unlockedFont; // Santana para poderes desbloqueados
    [SerializeField] private TMP_FontAsset lockedFont;   // Fenara para poderes bloqueados

    public void ChangeFont(bool isUnlocked)
    {
        brisaPowerName.font = isUnlocked ? unlockedFont : lockedFont;
        bestiaPowerName.font = isUnlocked ? unlockedFont : lockedFont;
        whereToFind.font = unlockedFont; // Siempre usa Santana
    }
}