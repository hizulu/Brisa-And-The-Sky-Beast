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

    public override void Enter()
    {
        CamEnterSetting();
        stateMachine.Player.PlayerInput.PlayerActions.Attack.Disable();
        base.Enter();
        stateMachine.Player.AreaMoveBeast.SetActive(true);
        stateMachine.Player.CursorMarker.SetActive(true);
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.canceled += OnPointedStateCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.MoveBeast.performed += OnLeftClick;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Has entrado en el estado de APUNTANDO");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        stateMachine.Player.CursorMarker.transform.position = CursorPosition();
    }

    public override void Exit()
    {
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.canceled -= OnPointedStateCanceled;
        base.Exit();
        stateMachine.Player.PlayerInput.PlayerActions.Attack.Enable();
        stateMachine.Player.CursorMarker.SetActive(false);

        Debug.Log("Has salido del estado de APUNTANDO");

        CamExitSetting();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected void CamEnterSetting()
    {
        float orbitMouseX = 50f;
        float orbitmouseY = 0f;

        stateMachine.Player.CamComponents = stateMachine.Player.playerCam.GetCinemachineComponent<CinemachinePOV>();
        stateMachine.Player.playerCam.m_Lens.FieldOfView = 70f;
        stateMachine.Player.CamComponents.m_HorizontalAxis.m_MaxSpeed = orbitMouseX;
        stateMachine.Player.CamComponents.m_VerticalAxis.m_MaxSpeed = orbitmouseY;
        stateMachine.Player.playerCam.transform.position += new Vector3(0, 50, -10);
    }

    private void CamExitSetting()
    {
        stateMachine.Player.AreaMoveBeast.SetActive(false);        
        stateMachine.Player.playerCam.m_Lens.FieldOfView = 60f;
        stateMachine.Player.CamComponents.m_HorizontalAxis.m_MaxSpeed = 300f;
        stateMachine.Player.CamComponents.m_VerticalAxis.m_MaxSpeed = 300f;
        stateMachine.Player.playerCam.transform.position -= new Vector3(0, 50, -10);
    }

    protected override void OnPointedStateCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private Vector3 CursorPosition()
    {
        SpriteRenderer circleArea = stateMachine.Player.AreaMoveBeast.GetComponent<SpriteRenderer>();
        float areaRadius = circleArea.bounds.extents.x;

        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
        Vector3 areaCenter = stateMachine.Player.AreaMoveBeast.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit, areaRadius, groundedData.ClickableLayers))
        {
            Vector3 hitPoint = hit.point;

            Vector3 flatHit = new Vector3(hitPoint.x, areaCenter.y, hitPoint.z);
            Vector3 direction = flatHit - areaCenter;
            float distance = direction.magnitude;

            if (distance > areaRadius)
                stateMachine.Player.CursorMarker.SetActive(false);
            else
                stateMachine.Player.CursorMarker.SetActive(true);

            return flatHit;
        }
        return stateMachine.Player.CursorMarker.transform.position;
    }

    private void MoveToClick()
    {
        Vector3 clickPosition = CursorPosition();

        if (groundedData.ClickEffect != null)
        {
            GameObject.Instantiate(groundedData.ClickEffect, clickPosition + new Vector3(0, 0.1f, 0), groundedData.ClickEffect.transform.rotation);
            EventsManager.TriggerSpecialEvent<Vector3>("MoveBeast", clickPosition);
            Debug.Log("Has hecho click en la posición: " + " " + clickPosition);
        }
    }

    private void OnLeftClick(InputAction.CallbackContext context)
    {
        MoveToClick();
    }
}
