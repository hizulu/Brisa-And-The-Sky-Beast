using UnityEngine;

/*
 * NOMBRE CLASE: MinimapFollowPlayer
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 13/04/2025
 * DESCRIPCI�N: Script que gestiona la posici�n de la c�mara del minimapa para seguir al jugador.
 * VERSI�N: 1.0 Sistema de minimapa inicial.
 */

public class MinimapFollowPlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 20, 0);

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
