using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: SheepStateMachine
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/05/2025
 * DESCRIPCIÓN: Clase que hereda de StateMachine y se encarga de instanciar y dar acceso a los estados de las ovejas.
 *              Mantiene las referencias a los diferentes estados.
 * VERSIÓN: 1.0. Instanciación de todos los estados de las ovejas.
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
