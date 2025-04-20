using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public enum EntityType { Player, Beast /*, Enemy*/ }

    [Header("Configuración")]
    [SerializeField] private EntityType entityType;
    [SerializeField] private Gradient gradient;

    [Header("Componentes")]
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Image fill;

    // Referencias a los componentes de salud
    private PlayerStatsData playerStats;
    private Beast beast;
    // private Enemy enemy;

    void Start()
    {
        // Obtener referencias según el tipo de entidad
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
                    InitializeHealthBar(beast.maxHealth, beast.currentHealth);
                }
                break;

                /*case EntityType.Enemy:
                    enemy = GetComponentInParent<Enemy>();
                    if (enemy != null)
                    {
                        InitializeHealthBar(enemy.MaxHealth, enemy.CurrentHealth);
                    }
                    break;*/
        }
    }

    void Update()
    {
        // Actualizar valores según la entidad
        switch (entityType)
        {
            case EntityType.Player:
                if (playerStats != null)
                {
                    UpdateHealth(playerStats.CurrentHealth);
                }
                break;

            case EntityType.Beast:
                if (beast != null)
                {
                    UpdateHealth(beast.currentHealth);
                }
                break;

                /*case EntityType.Enemy:
                    if (enemy != null)
                    {
                        UpdateHealth(enemy.CurrentHealth);
                    }
                    break;*/
        }
    }

    // Inicializa la barra de salud con valores máximos y actuales
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

    // Actualiza la barra de salud con el valor actual
    private void UpdateHealth(float currentHP)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHP;

            if (fill != null)
            {
                fill.color = gradient.Evaluate(healthBarSlider.normalizedValue);
            }
        }
    }

    // Método público para actualizar la salud manualmente
    public void SetHealth(float health)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = health;
        }
    }
}