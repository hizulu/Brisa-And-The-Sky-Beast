using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        player.anim.SetBool("isWalking", false);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        // Aquí se comprueba la condición para el cambio de estado y se llama a change state de PlayerStateMachine
        Vector2 direction = player.walkAction.action.ReadValue<Vector2>();
        Vector3 newPosition = new Vector3(direction.x, 0, direction.y);

        if (newPosition != Vector3.zero)
        {
            player.StateMachine.ChangeState(player.WalkState);
        }             
    }
}
