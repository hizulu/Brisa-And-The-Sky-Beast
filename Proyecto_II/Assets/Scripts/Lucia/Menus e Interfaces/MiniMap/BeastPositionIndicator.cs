using UnityEngine;
using UnityEngine.UI;

public class BeastPositionIndicator : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float minimapRadius = 40f;
    [SerializeField] private float indicatorRadius = 100f;
    [SerializeField] private float fadeSpeed = 4f; // Velocidad de fade (mayor = más rápido)

    [Header("Referencias")]
    [SerializeField] private RectTransform indicatorRect;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private GameObject beast;

    private Transform player;
    private BeastTrapped beastTrapped;
    private float targetAlpha = 0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        beastTrapped = beast.GetComponent<BeastTrapped>();

        // Configuración inicial
        indicatorImage.color = new Color(1, 1, 1, 0);
        indicatorRect.gameObject.SetActive(true); // Siempre activo
    }

    private void Update()
    {
        // Control de visibilidad
        bool shouldShow = beastTrapped.beasIsFree &&
                         Vector3.Distance(beast.transform.position, player.position) > minimapRadius;

        targetAlpha = shouldShow ? 1f : 0f;

        // Aplicar fade suave
        float currentAlpha = indicatorImage.color.a;
        float newAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
        indicatorImage.color = new Color(1, 1, 1, newAlpha);

        // Actualizar posición si es visible
        if (newAlpha > 0.01f)
        {
            Vector3 direction = beast.transform.position - player.position;
            direction.y = 0;
            UpdatePosition(direction.normalized);
        }
    }

    private void UpdatePosition(Vector3 direction)
    {
        Vector2 dir = new Vector2(direction.x, direction.z);
        indicatorRect.anchoredPosition = dir * indicatorRadius;
        indicatorRect.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
    }
}