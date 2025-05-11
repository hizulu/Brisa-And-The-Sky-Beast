using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: NotificationManager
 * AUTOR: Lucía García López
 * FECHA: 18/04/2025
 * DESCRIPCIÓN: Script que gestiona la aparición de notificaciones en la interfaz de usuario.
 * VERSIÓN: 1.0 Sistema de notificaciones inicial.
 * 1.1 Efecto de desvanecimiento FadeIn y FadeOut añadido.
 * 1.2 Se ha añadido la opción de mostrar una notificación de apariencia.
 */

public class NotificationManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject appearanceNotificationPrefab;
    [SerializeField] private Transform notificationParent;
    [SerializeField] private float displayDuration = 1.5f;
    [SerializeField] private float fadeInDuration = 0.01f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private int maxNotifications = 3;

    private Queue<GameObject> activeNotifications = new Queue<GameObject>();
    #endregion

    #region Singleton
    public static NotificationManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    //Muestra los objetos recogidos y su cantidad en la interfaz de usuario.
    public void ShowNotification(ItemData itemData, int quantity)
    {
        StartCoroutine(SpawnNotification(itemData, quantity));
    }

    // Método que se encarga de crear la notificación y gestionar su aparición y desaparición.
    private IEnumerator SpawnNotification(ItemData itemData, int quantity)
    {
        while (activeNotifications.Count >= maxNotifications)
        {
            yield return null;
        }

        GameObject newNotif = Instantiate(notificationPrefab, notificationParent);
        NotificationUI ui = newNotif.GetComponent<NotificationUI>();
        CanvasGroup canvasGroup = newNotif.GetComponent<CanvasGroup>();

        ui.Setup(itemData, quantity);

        // FadeIn
        canvasGroup.alpha = 0f;
        float timeElapsed = 0f;
        while (timeElapsed < fadeInDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeInDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        activeNotifications.Enqueue(newNotif);

        yield return new WaitForSeconds(displayDuration);

        // FadeOut
        timeElapsed = 0f;
        while (timeElapsed < fadeOutDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeOutDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        // Eliminar la notificación
        Destroy(newNotif);
        activeNotifications.Dequeue();
    }
    // Método que se encarga de crear la notificación de aparición y gestionar su aparición y desaparición.
    public void ShowAppearanceNotification()
    {
        if(AppearanceUnlock.Instance.canUnlock && AppearanceUnlock.Instance.canShowNotification)
        {
            StartCoroutine(SpawnAppearanceNotification());
        }
        else
        {
            Debug.Log("No se puede mostrar la notificación de aparición porque no está desbloqueada o ya se ha mostrado.");
        }
    }

    private IEnumerator SpawnAppearanceNotification()
    {
        while (activeNotifications.Count >= maxNotifications-1) // Se reduce el número máximo de notificaciones para que no se solapen
        {
            yield return null;
        }
        GameObject newNotif = Instantiate(appearanceNotificationPrefab, notificationParent);
        AppearanceNotificationUI ui = newNotif.GetComponent<AppearanceNotificationUI>();
        CanvasGroup canvasGroup = newNotif.GetComponent<CanvasGroup>();
        ui.Setup();
        // FadeIn
        canvasGroup.alpha = 0f;
        float timeElapsed = 0f;
        while (timeElapsed < fadeInDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeInDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
        activeNotifications.Enqueue(newNotif);
        yield return new WaitForSeconds(displayDuration);
        // FadeOut
        timeElapsed = 0f;
        while (timeElapsed < fadeOutDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeOutDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        // Eliminar la notificación
        Destroy(newNotif);
        activeNotifications.Dequeue();
    }
}
