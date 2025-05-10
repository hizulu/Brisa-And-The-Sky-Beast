using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: Sheep
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 10/05/2025
 * DESCRIPCI�N: Clase que gestiona la l�gica de las ovejas.
 * VERSI�N: 1.0.
 */

public class Sheep : MonoBehaviour
{
    private SheepStateMachine sheepStateMachine;

    public Animator AnimSheep {  get; private set; }

    

    private void Awake()
    {
        AnimSheep = GetComponent<Animator>();

        sheepStateMachine = new SheepStateMachine(this);
    }

    void Start()
    {
        RandomInitialStateSheep();
    }

    void Update()
    {
        sheepStateMachine.UpdateLogic();
    }

    private void FixedUpdate()
    {
        sheepStateMachine.UpdatePhysics();
    }

    #region M�todos Propios Sheep
    private void RandomInitialStateSheep()
    {
        List<IState> initialState = new List<IState>()
        {
            sheepStateMachine.SheepIdleState,
            sheepStateMachine.SheepWalkState,
            sheepStateMachine.SheepGrazeState
        };

        int randomState = Random.Range(0, initialState.Count);
        sheepStateMachine.ChangeState(initialState[randomState]);
    }
    #endregion
}
