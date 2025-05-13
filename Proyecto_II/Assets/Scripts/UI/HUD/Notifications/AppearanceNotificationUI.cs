using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * NOMBRE CLASE: AppearanceNotificationUI
 * AUTOR: Lucía García López
 * FECHA: 10/05/2025
 * DESCRIPCIÓN: Script que gestiona la interfaz de usuario para las notificaciones de las nuevas apariencias desbloqueadas. Va dentro del prefab de la notificación.
 * VERSIÓN: 1.0 
 */

public class AppearanceNotificationUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public void Setup()
    {
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
