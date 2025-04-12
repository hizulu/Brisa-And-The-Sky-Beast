using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions PlayerActions { get; private set; }

    private void Awake()
    {
        InputActions = new PlayerInputActions();

        PlayerActions = InputActions.Player;
    }

    private void OnEnable()
    {
        EventsManager.CallSpecialEvents<bool>("PauseMode", PauseMode);
        InputActions.Enable();
    }

    private void OnDisable()
    {
        EventsManager.StopCallSpecialEvents<bool>("PauseMode", PauseMode);
        InputActions.Disable();
    }

    private void PauseMode(bool isPause)
    {
        if (isPause)
        {
            Debug.Log(isPause);
            InputActions.Disable();
        }
        else
        {
            Debug.Log(isPause);
            InputActions.Enable();
        }
    }
}
