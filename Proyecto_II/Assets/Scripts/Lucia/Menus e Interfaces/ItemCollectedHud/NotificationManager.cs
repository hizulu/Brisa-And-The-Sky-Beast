using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: NotificationManager
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 18/04/2025
 * DESCRIPCI�N: Script que gestiona la aparici�n de notificaciones en la interfaz de usuario.
 * VERSI�N: 1.0 Sistema de notificaciones inicial.
 * 1.1 Efecto de desvanecimiento FadeIn y FadeOut a�adido.
 * 1.2 Se ha a�adido la opci�n de mostrar una notificaci�n de apariencia.
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

    // M�todo que se encarga de crear la notificaci�n y gestionar su aparici�n y desaparici�n.
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

        // Eliminar la notificaci�n
        Destroy(newNotif);
        activeNotifications.Dequeue();
    }
    // M�todo que se encarga de crear la notificaci�n de aparici�n y gestionar su aparici�n y desaparici�n.
    public void ShowAppearanceNotification()
    {
        if(AppearanceUnlock.Instance.canUnlock && AppearanceUnlock.Instance.canShowNotification)
        {
            StartCoroutine(SpawnAppearanceNotification());
        }
        else
        {
            Debug.Log("No se puede mostrar la notificaci�n de aparici�n porque no est� desbloqueada o ya se ha mostrado.");
        }
    }

    private IEnumerator SpawnAppearanceNotification()
    {
        while (activeNotifications.Count >= maxNotifications-1) // Se reduce el n�mero m�ximo de notificaciones para que no se solapen
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
        // Eliminar la notificaci�n
        Destroy(newNotif);
        activeNotifications.Dequeue();
    }
}
