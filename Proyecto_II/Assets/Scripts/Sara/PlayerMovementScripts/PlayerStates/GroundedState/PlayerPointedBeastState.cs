using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerPointedBeastState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 17/03/2015
 * DESCRIPCI�N: Estado en el que Player puede mandar a la Bestia moverse haciendo click dentro de un �rea determinada.
 * VERSI�N: 1.0. 
 */
public class PlayerPointedBeastState : PlayerGroundedState
{
    public PlayerPointedBeastState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        stateMachine.MovementData.MovementSpeedModifier = 0f; // En este estado Player no puede moverse.
        CamEnterSetting();
        stateMachine.Player.PlayerInput.PlayerActions.Attack.Disable();
        base.Enter();
        stateMachine.Player.AreaMoveBeast.SetActive(true);
        stateMachine.Player.CursorMarker.SetActive(true);
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.canceled += OnPointedStateCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.MoveBeast.performed += OnLeftClick;

        UnlockCursor();
        //Debug.Log("Has entrado en el estado de APUNTANDO");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        stateMachine.Player.CursorMarker.transform.position = CursorPosition();
    }

    public override void Exit()
    {
        stateMachine.Player.PlayerInput.PlayerActions.PointedMode.canceled -= OnPointedStateCanceled;
        stateMachine.Player.PlayerInput.PlayerActions.MoveBeast.performed -= OnLeftClick;
        base.Exit();
        stateMachine.Player.PlayerInput.PlayerActions.Attack.Enable();
        stateMachine.Player.AreaMoveBeast.SetActive(false);
        stateMachine.Player.CursorMarker.SetActive(false);

        //Debug.Log("Has salido del estado de APUNTANDO");

        CamExitSetting();
        LockCursor();
    }
    #endregion

    #region M�todos Propios PointedBeastState
    /// <summary>
    /// M�todo para visualizar d�nde est� el cursor dentro del �rea permitida.
    /// Se activa dentro del �rea permitida, si se sale, se desactiva.
    /// </summary>
    /// <returns>Vector que representa la posici�n en el mundo.</returns>
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

    /// <summary>
    /// M�todo que reconoce la entrada del input (click izquierdo).
    /// Si lo detecta, ejecuta el m�todo de desplazarse hasta el punto en el que se ha hecho el click.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    private void OnLeftClick(InputAction.CallbackContext context)
    {
        MoveToClick();
    }

    /// <summary>
    /// M�todo para que la Bestia se mueva a la posici�n donde Player hizo click.
    /// Se instancia un efecto para que el jugador sepa el lugar donde ha pulsado.
    /// Se crea un evento de llamada para que la Bestia ejecute el m�todo pertinente (desplazarse al lugar del click).
    /// </summary>
    private void MoveToClick()
    {
        Vector3 clickPosition = CursorPosition();

        if (groundedData.ClickEffect != null)
        {
            GameObject.Instantiate(groundedData.ClickEffect, clickPosition + new Vector3(0, 0.1f, 0), groundedData.ClickEffect.transform.rotation);
            EventsManager.TriggerSpecialEvent<Vector3>("BeastDirected", clickPosition); // EVENTO: Crear evento de mover a la Bestia.
            //Debug.Log("Has hecho click en la posici�n: " + " " + clickPosition);
        }
    }
    #endregion

    #region M�todos Cambio C�mara
    /// <summary>
    /// M�todo que mueve la c�mara a una posici�n concreta al entrar al estado de apuntado.
    /// </summary>
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

    /// <summary>
    /// M�todo que reposiciona la c�mara al salir del estado de apuntado.
    /// </summary>
    private void CamExitSetting()
    {
        stateMachine.Player.AreaMoveBeast.SetActive(false);        
        stateMachine.Player.playerCam.m_Lens.FieldOfView = 60f;
        stateMachine.Player.CamComponents.m_HorizontalAxis.m_MaxSpeed = 300f;
        stateMachine.Player.CamComponents.m_VerticalAxis.m_MaxSpeed = 300f;
        stateMachine.Player.playerCam.transform.position -= new Vector3(0, 50, -10);
    }
    #endregion

    #region M�todo Cancelar Entrada Input
    /// <summary>
    /// M�todo que reconoce si se ha dejado de pulsar el input (click derecho) y si as� es, cambia de estado.
    /// </summary>
    /// <param name="context"></param>
    protected override void OnPointedStateCanceled(InputAction.CallbackContext context)
    {
        OnStop();
    }
    #endregion
}
