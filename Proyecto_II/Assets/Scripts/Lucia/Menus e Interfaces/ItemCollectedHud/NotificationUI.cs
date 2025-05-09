using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * NOMBRE CLASE: NotificationUI
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 18/04/2025
 * DESCRIPCI�N: Script que gestiona la interfaz de usuario para las notificaciones de objetos recogidos. Va dentro del prefab de la notificaci�n.
 * VERSI�N: 1.0 Solo l�gica para el panel de timer screen 
 */

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
