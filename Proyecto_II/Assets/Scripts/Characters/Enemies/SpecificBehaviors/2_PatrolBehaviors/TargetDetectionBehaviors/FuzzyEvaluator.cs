using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: FuzzyEvaluator
 * AUTOR: Jone Sainz Egea
 * FECHA: 22/04/2025
 * DESCRIPCIÓN: Clase que contiene las reglas difusas y su evaluación.
 *              Se definen los conjuntos difusos para las dos variables que se utilizan a la hora de seleccionar un objetivo: la distancia al objetivo y su porcentaje de salud.
 *              Contiene el conjunto de reglas difusas que combinan estas variables para determinar las prioridades.
 *              Convierte el resultado difuso en un valor final numérico entre 0 y 100 que depende de la distancia y salud del objetivo.
 * VERSIÓN: 1.0. Script base para la evaluación mediante lógica difusa.
 */
public class FuzzyEvaluator
{
    // Conjuntos de distancia (en metros)
    private Dictionary<string, FuzzySet> distanceSets = new()
    {
        { "Cerca",     new FuzzySet(0, 3, 7) },
        { "Lejos",     new FuzzySet(6, 8, 14) },
        { "MuyLejos",  new FuzzySet(12, 15, 30) }
    };

    // Conjuntos de salud (en %)
    private Dictionary<string, FuzzySet> healthSets = new()
    {
        { "Baja",     new FuzzySet(0, 20, 35) },
        { "Media",    new FuzzySet(30, 50, 70) },
        { "Alta",     new FuzzySet(65, 100, 101) }
    };

    /*
     * Método que calcula el valor de prioridad del objetivo
     * @param1 float -  distancia a la que se encuentra del enemigo
     * @param2 float -  porcentaje de salud
     * @return float - valor de prioridad [0, 100]
     */
    public float EvaluatePriority(float distance, float health)
    {
        Dictionary<string, float> distMembership = new();
        Dictionary<string, float> healthMembership = new();

        // Obtiene el grado de pertenencia de cada conjunto
        foreach (var pair in distanceSets)
            distMembership[pair.Key] = pair.Value.GetMembership(distance);

        foreach (var pair in healthSets)
            healthMembership[pair.Key] = pair.Value.GetMembership(health);

        // Reglas
        List<(float weight, float priority)> rules = new();
        AddingRules(rules, distMembership, healthMembership);

        // Defuzzificación por media ponderada
        float totalWeight = 0f;
        float weightedSum = 0f;

        foreach (var (weight, priority) in rules)
        {
            weightedSum += weight * priority;
            totalWeight += weight;
        }
        return (totalWeight == 0f) ? 0f : weightedSum / totalWeight;
    }

    /*
     * Método en el que se establecen las reglas de prioridad
     * @param1 List<(float weight, float priority)> lista de reglas
     * @param2 Dictionary<string, float> diccionario con la información de los conjuntos de distancia
     * @param3 Dictionary<string, float> diccionario con la información de los conjuntos de salud
     */
    private void AddingRules(List<(float weight, float priority)> rules, Dictionary<string, float> distMembership, Dictionary<string, float> healthMembership)
    {
        void AddRule(string distKey, string healthKey, float priorityValue)
        {
            float activation = Mathf.Min(distMembership[distKey], healthMembership[healthKey]);
            if (activation > 0f)
                rules.Add((activation, priorityValue));
        }

        AddRule("Cerca", "Baja", 100);
        AddRule("Cerca", "Media", 80);
        AddRule("Cerca", "Alta", 60);
        AddRule("Lejos", "Baja", 75);
        AddRule("Lejos", "Media", 65);
        AddRule("Lejos", "Alta", 35);
        AddRule("MuyLejos", "Baja", 40);
        AddRule("MuyLejos", "Media", 20);
        AddRule("MuyLejos", "Alta", 10);
    }
}
