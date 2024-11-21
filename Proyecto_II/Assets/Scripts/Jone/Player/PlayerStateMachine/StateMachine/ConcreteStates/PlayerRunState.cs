using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player player, PlayerStateMachine playerStateMachine, float baseSpeed, float speedMultiplier) : base(player, playerStateMachine, baseSpeed, speedMultiplier)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        player.anim.SetBool("isRunning", true);
        Debug.Log("Estás corriendo" + " " + baseSpeed * speedMultiplier);
    }

    public override void ExitState()
    {
        base.ExitState();
        player.anim.SetBool("isRunning", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        // Aquí se comprueba la condición para el cambio de estado y se llama a change state de PlayerStateMachine
        if (!player.runAction.action.IsPressed())
        {
            player.StateMachine.ChangeState(player.WalkState);
        }

        Vector2 direction = player.walkAction.action.ReadValue<Vector2>(); // Hemos cambaido a public la variable del input system --> DUDA
        Vector3 newPosition = new Vector3(direction.x, 0, direction.y);

        if (newPosition != Vector3.zero)
        {
            newPosition = Quaternion.AngleAxis(player.camTransform.rotation.eulerAngles.y, Vector3.up) * newPosition;
            Quaternion targetRotation = Quaternion.LookRotation(newPosition);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Player.rotationSpeed * Time.deltaTime);
        }
        else
        {
            player.StateMachine.ChangeState(player.IdleState);
        }

        player.transform.position += newPosition * baseSpeed * speedMultiplier * Time.deltaTime;
    }
}
