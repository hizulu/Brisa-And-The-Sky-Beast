/*
 * NOMBRE CLASE: FuzzySet
 * AUTOR: Jone Sainz Egea
 * FECHA: 22/04/2025
 * DESCRIPCI�N: Clase que sirve de base para definir los conjuntos difusos.
 *              Se basa en una funci�n de pertenencia triangular.
 *              Cada conjunto se representa mediante tres par�metros (A, B, C) que definen el inicio, el punto m�ximo y el final del conjunto.
 * VERSI�N: 1.0. Script base para la definici�n de los conjuntos difusos.
 */
public class FuzzySet
{
    public float A, B, C;

    public FuzzySet(float a, float b, float c)
    {
        A = a; B = b; C = c;
    }

    /*
     * M�todo que calcula el grado de pertenencia de un valor x al conjunto difuso definido
     * @param1 float x -  valor del que se quiere obtener el grado de pertenencia
     * @return float - n�mero entre 0 y 1 que indica en qu� medida se ajusta ese valor a la categor�a representada
     */
    public float GetMembership(float x)
    {
        if (x <= A || x >= C) return 0f;
        if (x == B) return 1f;
        if (x < B) return (x - A) / (B - A);
        return (C - x) / (C - B);
    }
}
