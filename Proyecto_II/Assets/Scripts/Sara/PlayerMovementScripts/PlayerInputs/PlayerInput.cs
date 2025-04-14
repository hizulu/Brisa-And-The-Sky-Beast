using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions PlayerActions { get; private set; } // Mapa de acciones del Player.
    public PlayerInputActions.UIPanelActions UIPanelActions { get; private set; } // Mapa de acciones de UI, paneles, etc.

    private void Awake()
    {
        InputActions = new PlayerInputActions();

        PlayerActions = InputActions.Player;

        UIPanelActions = InputActions.UIPanel;
    }

    private void OnEnable()
    {
        InputActions.Player.Enable();
        InputActions.UIPanel.Enable();

        EventsManager.CallNormalEvents("UIPanelOpened", ActiveUIActions);
        EventsManager.CallNormalEvents("UIPanelClosed", ActivePlayerActions);
    }

    private void OnDisable()
    {
        InputActions.Player.Disable();
        InputActions.UIPanel.Disable();

        EventsManager.StopCallNormalEvents("UIPanelOpened", ActiveUIActions);
        EventsManager.StopCallNormalEvents("UIPanelClosed", ActivePlayerActions);
    }

    public void ActiveUIActions()
    {
        InputActions.UIPanel.Enable();
        InputActions.Player.Disable();
    }

    public void ActivePlayerActions()
    {
        InputActions.Player.Enable();
        InputActions.UIPanel.Enable();
    }
}
