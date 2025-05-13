using UnityEngine;

/*
 * NOMBRE CLASE: PlayerSO
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2015
 * DESCRIPCIÓN: ScriptableObject que almacena todos los datos del Player.
 * VERSIÓN: 1.0
 */
[CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObjects/Player")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField] public PlayerGroundedData GroundedData { get; private set; }
    [field: SerializeField] public PlayerAirborneData AirborneData { get; private set; }
    [field: SerializeField] public PlayerStatsData StatsData { get; private set; }
}
