using UnityEngine;

/*
 * NOMBRE SCRIPT (Interfaz): IState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Interfaz que contiene todos los métodos que deben tener los diferentes estados, cualquier clase que herede esta interfaz debe definirlos.
 * VERSIÓN: 1.0. Métodos necesarios para la máquina de estados definidos.
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
