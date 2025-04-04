using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

// Jone Sainz Egea
// 03/04/2025

// Clase que maneja por separado el efecto de romperse de objetos
// Separado de la lógica de BreakableBox para seguir el principio SOLID de responsabilidad única
public class BreakableEffectHandler
{
    private readonly VisualEffect smokeEffect;

    public BreakableEffectHandler(VisualEffect effect)
    {
        smokeEffect = effect;
    }

    public void PlayEffect(Vector3 position)
    {
        smokeEffect.transform.position = position;
        smokeEffect.Play();
    }
    public VisualEffect GetVisualEffect()
    {
        return smokeEffect;
    }
}
