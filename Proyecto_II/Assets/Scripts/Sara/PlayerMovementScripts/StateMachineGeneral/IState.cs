using UnityEngine;

/*
 * NOMBRE SCRIPT (Interfaz): IState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Interfaz que contiene todos los m�todos que deben tener los diferentes estados, cualquier clase que herede esta interfaz debe definirlos.
 * VERSI�N: 1.0. M�todos necesarios para la m�quina de estados definidos.
*/

public interface IState
{
    void Enter();
    void HandleInput();
    void UpdateLogic();
    void UpdatePhysics();
    void OnTriggerEnter(Collider collider);
    void OnTriggerExit(Collider collider);
    void Exit();
}
