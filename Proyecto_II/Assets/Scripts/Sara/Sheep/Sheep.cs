using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: Sheep
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/05/2025
 * DESCRIPCIÓN: Clase que gestiona la lógica de las ovejas.
 * VERSIÓN: 1.0.
 */

public class Sheep : MonoBehaviour
{
    private SheepStateMachine sheepStateMachine;

    private void Awake()
    {
        sheepStateMachine = new SheepStateMachine(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        sheepStateMachine.ChangeState(sheepStateMachine.SheepIdleState);
    }

    // Update is called once per frame
    void Update()
    {
        sheepStateMachine.UpdateLogic();
    }

    private void FixedUpdate()
    {
        sheepStateMachine.UpdatePhysics();
    }
}
