using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Player player;
    private PlayerStatsData statsData;
    private Slider healthBarSlider;
    public Gradient gradient;
    private Image fill;

    void Start()
    {
        player = FindObjectOfType<Player>();

        if (player != null && player.Data != null)
        {
            statsData = player.Data.StatsData;

            healthBarSlider = GetComponent<Slider>();
            healthBarSlider.maxValue = statsData.MaxHealth;
            healthBarSlider.value = statsData.CurrentHealth;
            fill = healthBarSlider.fillRect.GetComponent<Image>();
            fill.color = gradient.Evaluate(1f);
        }
        else
        {
            Debug.LogError("No se pudo encontrar al Player o su Data.");
        }
    }

    void Update()
    {
        if (statsData != null)
        {
            SetHealth(statsData.CurrentHealth);
            fill.color = gradient.Evaluate(healthBarSlider.normalizedValue);
        }
    }

    void SetHealth(float health)
    {
        healthBarSlider.value = health;
    }
}
