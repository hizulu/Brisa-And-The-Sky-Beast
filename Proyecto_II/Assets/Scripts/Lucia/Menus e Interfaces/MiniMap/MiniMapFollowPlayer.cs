using UnityEngine;

/*
 * NOMBRE CLASE: MinimapFollowPlayer
 * AUTOR: Lucía García López
 * FECHA: 13/04/2025
 * DESCRIPCIÓN: Script que gestiona la posición de la cámara del minimapa para seguir al jugador.
 * VERSIÓN: 1.0 Sistema de minimapa inicial.
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
