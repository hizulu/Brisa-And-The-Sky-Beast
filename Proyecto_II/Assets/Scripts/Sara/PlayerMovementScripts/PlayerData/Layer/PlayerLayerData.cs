using System;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerLayerData
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que gestiona la información de las capas (layers) relevantes para el Player.
 * VERSIÓN: 1.0
 */

[Serializable]
public class PlayerLayerData
{
    [field: SerializeField] public LayerMask EnviromentLayer { get; private set; }

    /*
     * Método que comprueba si contiene una capa específica.
     * @param1 _layerMask -> Máscara de capas sobre la que se realiza la comprobación.
     * @param2 _layer -> Capa individual que se quiere verificar.
     * @return bool -> True si la capa está contenida en la máscara, False en caso contrario.
     */
    public bool ContainsLayer(LayerMask _layerMask, int _layer)
    {
        return (1 << _layer & _layerMask) != 0;
    }

    /*
     * Método que comprueba si la capa anterior pertenece a la capa del entorno.
     * @param layer -> La capa que se desea verificar.
     * @return bool -> True si la capa es parte de la capa del entorno, False si no lo es.
     */
    public bool IsGroundLayer(int layer)
    {
        return ContainsLayer(EnviromentLayer, layer);
    }
}
