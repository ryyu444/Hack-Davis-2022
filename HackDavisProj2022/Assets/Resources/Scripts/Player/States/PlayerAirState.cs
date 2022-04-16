using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : AbstractPlayerState
{
    public override void Enter(PlayerController context)
    {
        base.Enter(context);
    }

    public override void Exit()
    {

    }

    public override void UpdateState()
    {
        context.rb.MoveWithRotation(context.cameraController.rotation, context.inputInfo.movementVector, 10f);

        if (context.IsGrounded())
        {
            context.ChangeState(new PlayerGroundedState());
        }

    }
}
