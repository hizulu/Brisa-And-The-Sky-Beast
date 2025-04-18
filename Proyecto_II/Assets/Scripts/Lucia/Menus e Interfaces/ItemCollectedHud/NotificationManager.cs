using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private Transform notificationParent;
    [SerializeField] private float displayDuration = 1.5f;
    [SerializeField] private float fadeInDuration = 0.01f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private int maxNotifications = 3;

    private Queue<GameObject> activeNotifications = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowNotification(ItemData itemData, int quantity)
    {
        StartCoroutine(SpawnNotification(itemData, quantity));
    }

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
}
