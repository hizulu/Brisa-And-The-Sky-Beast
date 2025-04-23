using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerLayerData
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase que gestiona la informaci�n de las capas (layers) relevantes para el Player.
 * VERSI�N: 1.0
 */

[Serializable]
public class PlayerLayerData
{
    [field: SerializeField] public LayerMask EnviromentLayer { get; private set; }

    /*
     * M�todo que comprueba si contiene una capa espec�fica.
     * @param1 _layerMask -> M�scara de capas sobre la que se realiza la comprobaci�n.
     * @param2 _layer -> Capa individual que se quiere verificar.
     * @return bool -> True si la capa est� contenida en la m�scara, False en caso contrario.
     */
    public bool ContainsLayer(LayerMask _layerMask, int _layer)
    {
        return (1 << _layer & _layerMask) != 0;
    }

    /*
     * M�todo que comprueba si la capa anterior pertenece a la capa del entorno.
     * @param layer -> La capa que se desea verificar.
     * @return bool -> True si la capa es parte de la capa del entorno, False si no lo es.
     */
    public bool IsGroundLayer(int layer)
    {
        return ContainsLayer(EnviromentLayer, layer);
    }
}
