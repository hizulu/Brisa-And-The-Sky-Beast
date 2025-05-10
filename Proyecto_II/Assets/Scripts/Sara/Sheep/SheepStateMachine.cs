using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepStateMachine
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 10/05/2025
 * DESCRIPCI�N: Clase que hereda de StateMachine y se encarga de instanciar y dar acceso a los estados de las ovejas.
 *              Mantiene las referencias a los diferentes estados.
 * VERSI�N: 1.0. Instanciaci�n de todos los estados de las ovejas.
 */

public class SheepStateMachine : StateMachine
{
    public Sheep Sheep { get; }
    public SheepIdleState SheepIdleState { get; }
    public SheepWalkState SheepWalkState { get; }
    public SheepGrazeState SheepGrazeState { get; }

    public SheepStateMachine(Sheep _sheep)
    {
        Sheep = _sheep;

        SheepIdleState = new SheepIdleState(this);
        SheepWalkState = new SheepWalkState(this);
        SheepGrazeState = new SheepGrazeState(this);
    }
}
