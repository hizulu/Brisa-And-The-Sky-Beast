using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerPointedBeastState : PlayerGroundedState
{
    public PlayerPointedBeastState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    bool isPressed;

    public override void Enter()
    {
        CamEnterSetting();
        stateMachine.Player.PlayerInput.PlayerActions.Attack.Disable();
        base.Enter();
        stateMachine.Player.AreaMoveBeast.SetActive(true);
        //stateMachine.Player.PlayerInput.PlayerActions.PointedMode.performed += OnPointedStateCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.MoveBeast.performed += OnLeftClick;
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.started += OnRightClickPressed;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Has entrado en el estado de APUNTANDO");
    }

    public override void UpdateLogic()
    {
        isPressed = true;
        base.UpdateLogic();
        //Debug.Log("Estás en MODO APUNTAR");
    }

    public override void Exit()
    {
        isPressed = false;
        stateMachine.Player.PlayerInput.PlayerActions.Attack.Enable();
        base.Exit();

        stateMachine.Player.AreaMoveBeast.SetActive(false);

        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.canceled -= OnPointedStateCanceled;

        stateMachine.Player.playerCam.m_Lens.FieldOfView = 60f;

        stateMachine.Player.playerCam.transform.position -= new Vector3(0, 0, -5);

        Debug.Log("Has salido del estado de APUNTANDO");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected void CamEnterSetting()
    {
        float orbitMouseX = 50f;
        float orbitmouseY = 50f;

        stateMachine.Player.CamComponents = stateMachine.Player.playerCam.GetCinemachineComponent<CinemachinePOV>();
        stateMachine.Player.playerCam.m_Lens.FieldOfView = 90f;
        stateMachine.Player.CamComponents.m_HorizontalAxis.m_MaxSpeed = orbitMouseX;
        stateMachine.Player.CamComponents.m_VerticalAxis.m_MaxSpeed = orbitmouseY;
        stateMachine.Player.playerCam.transform.position += new Vector3(0, 0, -5);
    }

    protected override void OnPointedStateCanceled(InputAction.CallbackContext context)
    {
        if (isPressed) return;
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void OnRightClickPressed(InputAction.CallbackContext context)
    {
        isPressed = true;
    }

    private void MoveToClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition), out hit, 50f, groundedData.ClickableLayers))
        {
            //agent.destination = hit.point;
            if (groundedData.ClickEffect != null)
            {
                GameObject.Instantiate(groundedData.ClickEffect, hit.point += new Vector3(0, 0.1f, 0), groundedData.ClickEffect.transform.rotation);
                EventsManager.TriggerSpecialEvent<Vector3>("MoveBeast", hit.point);
                Debug.Log("Has hecho click en la posición: " + " " +  hit.point);
            }
        }
    }

    private void OnLeftClick(InputAction.CallbackContext context)
    {
        MoveToClick();
    }
}
