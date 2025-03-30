using UnityEngine;
using TMPro;

public class PowersLocked : MonoBehaviour
{
    [SerializeField] private TMP_Text brisaPowerName;
    [SerializeField] private TMP_Text bestiaPowerName;
    [SerializeField] private TMP_Text brisaPowerDescription;
    [SerializeField] private TMP_Text bestiaPowerDescription;
    [SerializeField] private TMP_Text whereToFind;

    [SerializeField] private TMP_FontAsset unlockedFont; // Santana para poderes desbloqueados
    [SerializeField] private TMP_FontAsset lockedFont;   // Fenara para poderes bloqueados

    // Método para cambiar las fuentes
    public void ChangeFont(bool isUnlocked)
    {
        if (isUnlocked)
        {
            // Cambiar a fuente desbloqueada (Santana)
            brisaPowerName.font = unlockedFont;
            bestiaPowerName.font = unlockedFont;
            brisaPowerDescription.font = unlockedFont;
            bestiaPowerDescription.font = unlockedFont;
        }
        else
        {
            // Cambiar a fuente bloqueada (Fenara)
            brisaPowerName.font = lockedFont;
            bestiaPowerName.font = lockedFont;
            brisaPowerDescription.font = lockedFont;
            bestiaPowerDescription.font = lockedFont;
        }
    }
}
