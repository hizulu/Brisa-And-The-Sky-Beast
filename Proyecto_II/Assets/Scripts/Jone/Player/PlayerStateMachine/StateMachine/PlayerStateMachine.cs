//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerStateMachine
//{
//    // Máquina de estado del jugador, inicializa en el primer estado y se encarga del cambio de estado

//    public PlayerState CurrentPlayerState { get; set; }

//    public void Initialize(PlayerState startingState)
//    {
//        CurrentPlayerState = startingState;
//        CurrentPlayerState.EnterState();
//    }


//    // Se le llama desde los estados y recibe el estado al que va a cambiar
//    public void ChangeState(PlayerState newState)
//    {
//        Debug.Log("Cambia de estado");
//        CurrentPlayerState.ExitState(); // Finaliza el estado en el que se encuentra
//        CurrentPlayerState = newState; // Asigna el estado al que va a cambiar
//        CurrentPlayerState.EnterState(); // Entra en el nuevo estado
//    }
//}
