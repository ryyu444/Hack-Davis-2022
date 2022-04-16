using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : AbstractPlayerState
{
    public override void Enter(PlayerController context)
    {
        base.Enter(context);
        context.inputInfo.jumpPressedEvent += Jump;
        context.inputInfo.LMBPressedEvent += SwingAxe;
    }

    public override void Exit()
    {
        context.inputInfo.jumpPressedEvent -= Jump;
        context.inputInfo.LMBPressedEvent -= SwingAxe;
    }

    public void Jump()
    {
        context.rb.velocity += new Vector3(0, 5, 0);
    }

    public void SwingAxe()
    {
        context.ChangeState(new PlayerAxeSwingState());
    }

    public override void UpdateState()
    {
        context.rb.MoveWithRotation(context.cameraController.rotation, context.inputInfo.movementVector, 10f);
        
        if(!context.IsGrounded())
        {
            context.ChangeState(new PlayerAirState());
        }
    }
}
