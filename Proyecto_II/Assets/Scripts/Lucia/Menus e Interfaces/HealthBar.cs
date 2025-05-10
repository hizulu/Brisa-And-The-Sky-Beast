using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * NOMBRE SCRIPT: HealthBar
 * AUTOR: Lucía García López
 * FECHA: 21/04/2025
 * DESCRIPCIÓN: Script que gestiona la barra de salud de los personajes. Utiliza un Slider para mostrar la salud actual y un Gradient para el color de la barra.
 *              Se puede seleccionar el tipo de entidad (Player, Beast, Enemy) para adaptar la barra a cada uno.
 * VERSIÓN: 1.0. Solo para player.
 * 1.1 . Se añade la lógica para Beast y Enemy.
 */

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
        //Se hacen por separado porque cada uno tiene su propia lógica
        switch (entityType)
        {
            case EntityType.Player:
                Player player = FindObjectOfType<Player>();
                if (player != null && player.Data != null)
                {
                    playerStats = player.Data.StatsData;
                    InitializeHealthBar(playerStats.MaxHealth, playerStats.CurrentHealth);
                    //EventsManager.CallSpecialEvents<float>("PlayerHealth", UpdateHealth);
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

    //private void OnDestroy()
    //{
    //    EventsManager.StopCallSpecialEvents<float>("PlayerHealth", UpdateHealth);
    //}

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

    private IEnumerator Pulse()
    {
        //Efecto latido para la barra cuando la vida es 0
        float bigPulse = 1.3f;
        float smallPulse = 1.1f;
        float restDuration = 0.3f;

        float pulseInDuration = 0.1f;
        float pulseOutDuration = 0.2f;

        while (true)
        {
            yield return ScaleTo(Vector3.one * bigPulse, pulseInDuration);
            yield return ScaleTo(Vector3.one, pulseOutDuration);

            yield return new WaitForSeconds(0.05f);

            yield return ScaleTo(Vector3.one * smallPulse, pulseInDuration * 0.7f);
            yield return ScaleTo(Vector3.one, pulseOutDuration * 0.7f);

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
