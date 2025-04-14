using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Player player;
    private PlayerStatsData statsData;
    private Slider healthBarSlider;

    void Start()
    {
        player = FindObjectOfType<Player>();

        if (player != null && player.Data != null)
        {
            statsData = player.Data.StatsData;

            healthBarSlider = GetComponent<Slider>();
            healthBarSlider.maxValue = statsData.MaxHealth;
            healthBarSlider.value = statsData.CurrentHealth;
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
        }
    }

    void SetHealth(float health)
    {
        healthBarSlider.value = health;
    }
}
