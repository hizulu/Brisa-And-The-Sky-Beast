using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public enum EntityType { Player, Beast, Enemy }

    [Header("Configuración")]
    [SerializeField] private EntityType entityType;
    [SerializeField] private Gradient gradient;

    [Header("Componentes")]
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Image fill;
    [SerializeField] private Image border; // Borde externo para tintar
    [SerializeField] private Color criticalColor = new Color(0.66f, 0.04f, 0.04f); // A90909

    // Referencias a los componentes de salud
    private PlayerStatsData playerStats;
    private Beast beast;
    private Enemy enemy;

    // Palpitación
    private Coroutine pulseCoroutine;
    private bool isPulsing = false;

    void Start()
    {
        switch (entityType)
        {
            case EntityType.Player:
                Player player = FindObjectOfType<Player>();
                if (player != null && player.Data != null)
                {
                    playerStats = player.Data.StatsData;
                    InitializeHealthBar(playerStats.MaxHealth, playerStats.CurrentHealth);
                }
                break;

            case EntityType.Beast:
                beast = FindObjectOfType<Beast>();
                if (beast != null)
                {
                    if(beast)
                    InitializeHealthBar(beast.maxHealth, beast.currentHealth);
                }
                break;

            case EntityType.Enemy:
                enemy = GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    InitializeHealthBar(enemy.maxHealth, enemy.currentHealth);
                }
                break;
        }
    }

    void Update()
    {
        float currentHP = 0;

        switch (entityType)
        {
            case EntityType.Player:
                if (playerStats != null)
                    currentHP = playerStats.CurrentHealth;
                break;

            case EntityType.Beast:
                if (beast != null)
                    currentHP = beast.currentHealth;
                break;

            case EntityType.Enemy:
                if (enemy != null)
                    currentHP = enemy.currentHealth;
                break;
        }

        UpdateHealth(currentHP);
    }

    private void InitializeHealthBar(float maxHP, float currentHP)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHP;
            healthBarSlider.value = currentHP;

            if (fill != null)
            {
                fill.color = gradient.Evaluate(1f);
            }
        }
    }

    private void UpdateHealth(float currentHP)
    {
        if (healthBarSlider == null) return;

        healthBarSlider.value = currentHP;

        if (fill != null)
        {
            fill.color = gradient.Evaluate(healthBarSlider.normalizedValue);
        }

        if (currentHP <= 0)
        {
            if (border != null)
                border.color = criticalColor;

            if (!isPulsing && !enemy)
            {
                pulseCoroutine = StartCoroutine(Pulse());
                isPulsing = true;
            }
        }
        else
        {
            if (border != null)
                border.color = Color.white;

            if (isPulsing)
            {
                StopCoroutine(pulseCoroutine);
                transform.localScale = Vector3.one;
                isPulsing = false;
            }
        }
    }

    public void SetHealth(float health)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = health;
        }
    }

    private IEnumerator Pulse()
    {
        // Escalas para el efecto de latido
        float bigPulse = 1.3f;    // Primer latido fuerte (PUM)
        float smallPulse = 1.1f;   // Segundo latido suave (pum)
        float restDuration = 0.3f;  // Pausa entre latidos

        // Duración de cada fase del latido
        float pulseInDuration = 0.1f;  // Tiempo para crecer
        float pulseOutDuration = 0.2f; // Tiempo para volver a normal

        while (true)
        {
            // Primer latido fuerte (PUM)
            yield return ScaleTo(Vector3.one * bigPulse, pulseInDuration);
            yield return ScaleTo(Vector3.one, pulseOutDuration);

            // Breve pausa entre latidos
            yield return new WaitForSeconds(0.05f);

            // Segundo latido suave (pum)
            yield return ScaleTo(Vector3.one * smallPulse, pulseInDuration * 0.7f);
            yield return ScaleTo(Vector3.one, pulseOutDuration * 0.7f);

            // Pausa completa antes de repetir
            yield return new WaitForSeconds(restDuration);
        }
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
