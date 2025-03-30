using UnityEngine;
using TMPro;

public class PowersLocked : MonoBehaviour
{
    [SerializeField] private PowersData powersData;
    [SerializeField] private TextMeshProUGUI brisaPowerName;
    [SerializeField] private TextMeshProUGUI bestiaPowerName;
    [SerializeField] private TextMeshProUGUI brisaPowerDescription;
    [SerializeField] private TextMeshProUGUI bestiaPowerDescription;
    [SerializeField] private TextMeshProUGUI whereToFind;

    [SerializeField] private TMP_FontAsset unlockedFont;
    [SerializeField] private TMP_FontAsset lockedFont;

    private void Start()
    {
        ChangeFont();
    }

    public void ChangeFont()
    {
        if (powersData.isUnlocked)
        {
            brisaPowerName.font = unlockedFont;
            bestiaPowerName.font = unlockedFont;
            brisaPowerDescription.font = unlockedFont;
            bestiaPowerDescription.font = unlockedFont;
        }
        else
        {
            brisaPowerName.font = lockedFont;
            bestiaPowerName.font = lockedFont;
            brisaPowerDescription.font = lockedFont;
            bestiaPowerDescription.font = lockedFont;
        }
    }
}
