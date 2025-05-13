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
 * 1.3 Se hace bien el limite de notificaciones. La notificacion de apariencia aparece sola.
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

    private Queue<IEnumerator> notificationQueue = new Queue<IEnumerator>();
    private int currentNotifications = 0;
    private const string notificationCompletedEvent = "OnNotificationComplete";
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

        // Suscribir al evento de notificación completada
        EventsManager.CallNormalEvents(notificationCompletedEvent, ProcessNextNotification);
    }

    private void OnDestroy()
    {
        // Desuscribir eventos
        EventsManager.StopCallNormalEvents(notificationCompletedEvent, ProcessNextNotification);
    }
    #endregion

    public void ShowNotification(ItemData itemData, int quantity)
    {
        notificationQueue.Enqueue(SpawnNotificationCoroutine(itemData, quantity));
        TryProcessQueue();
    }

    public void ShowAppearanceNotification()
    {
        if (AppearanceUnlock.Instance.canUnlock && AppearanceUnlock.Instance.canShowNotification)
        {
            // Crear cola temporal para priorizar la notificación de apariencia
            var tempQueue = new Queue<IEnumerator>();
            tempQueue.Enqueue(SpawnAppearanceNotificationCoroutine());

            while (notificationQueue.Count > 0)
            {
                tempQueue.Enqueue(notificationQueue.Dequeue());
            }

            notificationQueue = tempQueue;
            TryProcessQueue();
        }
    }

    private void ProcessNextNotification()
    {
        if (notificationQueue.Count > 0 && currentNotifications < maxNotifications)
        {
            StartCoroutine(ProcessNotificationCoroutine());
        }
    }

    private void TryProcessQueue()
    {
        if (notificationQueue.Count > 0 && currentNotifications < maxNotifications)
        {
            StartCoroutine(ProcessNotificationCoroutine());
        }
    }

    private IEnumerator ProcessNotificationCoroutine()
    {
        currentNotifications++;
        yield return StartCoroutine(notificationQueue.Dequeue());
        currentNotifications--;

        // Disparar evento de notificación completada
        EventsManager.TriggerNormalEvent(notificationCompletedEvent);
    }

    private IEnumerator SpawnNotificationCoroutine(ItemData itemData, int quantity)
    {
        GameObject newNotif = Instantiate(notificationPrefab, notificationParent);
        NotificationUI ui = newNotif.GetComponent<NotificationUI>();
        CanvasGroup canvasGroup = newNotif.GetComponent<CanvasGroup>();

        ui.Setup(itemData, quantity);

        // FadeIn
        yield return FadeCanvasGroup(canvasGroup, 0f, 1f, fadeInDuration);

        yield return new WaitForSeconds(displayDuration);

        // FadeOut
        yield return FadeCanvasGroup(canvasGroup, 1f, 0f, fadeOutDuration);

        Destroy(newNotif);
    }

    private IEnumerator SpawnAppearanceNotificationCoroutine()
    {
        GameObject newNotif = Instantiate(appearanceNotificationPrefab, notificationParent);
        AppearanceNotificationUI ui = newNotif.GetComponent<AppearanceNotificationUI>();
        CanvasGroup canvasGroup = newNotif.GetComponent<CanvasGroup>();

        ui.Setup();

        // FadeIn
        yield return FadeCanvasGroup(canvasGroup, 0f, 1f, fadeInDuration);

        yield return new WaitForSeconds(displayDuration);

        // FadeOut
        yield return FadeCanvasGroup(canvasGroup, 1f, 0f, fadeOutDuration);

        Destroy(newNotif);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            group.alpha = Mathf.Lerp(start, end, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        group.alpha = end;
    }
}