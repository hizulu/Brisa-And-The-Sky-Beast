using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private CanvasGroup canvasGroup;

    public void Setup(ItemData itemData, int quantity)
    {
        itemText.text = $"+{quantity} {itemData.itemName}";
        itemIcon.sprite = itemData.itemIcon;
        canvasGroup.alpha = 1;
    }

    public IEnumerator FadeOut(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }
    }
}
