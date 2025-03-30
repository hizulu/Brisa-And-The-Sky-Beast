using UnityEngine;
using TMPro;

public class PowersLocked : MonoBehaviour
{
    [SerializeField] private TMP_Text brisaPowerName;
    [SerializeField] private TMP_Text bestiaPowerName;
    [SerializeField] private TMP_Text whereToFind;

    [SerializeField] private TMP_FontAsset unlockedFont; // Santana para poderes desbloqueados
    [SerializeField] private TMP_FontAsset lockedFont;   // Fenara para poderes bloqueados

    // Método para cambiar las fuentes
    public void ChangeFont(bool isUnlocked)
    {
        if (isUnlocked)
        {
            brisaPowerName.font = unlockedFont;
            bestiaPowerName.font = unlockedFont;
        }
        else
        {
            brisaPowerName.font = lockedFont;
            bestiaPowerName.font = lockedFont;
        }

        // Asegurar que whereToFind siempre use la fuente desbloqueada (Santana)
        whereToFind.font = unlockedFont;
    }
}
