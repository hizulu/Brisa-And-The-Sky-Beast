
using UnityEngine;

// Jone Sainz Egea
// 22/04/2025
// Clase para definir los conjuntos difusos
// Funciones de pertenencia
public class FuzzySet
{
    public float A, B, C;

    public FuzzySet(float a, float b, float c)
    {
        A = a; B = b; C = c;
    }

    // Función triangular
    public float GetMembership(float x)
    {
        if (x <= A || x >= C) return 0f;
        if (x == B) return 1f;
        if (x < B) return (x - A) / (B - A);
        return (C - x) / (C - B);
    }
}
