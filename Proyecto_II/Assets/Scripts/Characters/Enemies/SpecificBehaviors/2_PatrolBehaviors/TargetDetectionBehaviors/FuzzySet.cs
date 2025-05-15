/*
 * NOMBRE CLASE: FuzzySet
 * AUTOR: Jone Sainz Egea
 * FECHA: 22/04/2025
 * DESCRIPCIÓN: Clase que sirve de base para definir los conjuntos difusos.
 *              Se basa en una función de pertenencia triangular.
 *              Cada conjunto se representa mediante tres parámetros (A, B, C) que definen el inicio, el punto máximo y el final del conjunto.
 * VERSIÓN: 1.0. Script base para la definición de los conjuntos difusos.
 */
public class FuzzySet
{
    public float A, B, C;

    public FuzzySet(float a, float b, float c)
    {
        A = a; B = b; C = c;
    }

    /*
     * Método que calcula el grado de pertenencia de un valor x al conjunto difuso definido
     * @param1 float x -  valor del que se quiere obtener el grado de pertenencia
     * @return float - número entre 0 y 1 que indica en qué medida se ajusta ese valor a la categoría representada
     */
    public float GetMembership(float x)
    {
        if (x <= A || x >= C) return 0f;
        if (x == B) return 1f;
        if (x < B) return (x - A) / (B - A);
        return (C - x) / (C - B);
    }
}
