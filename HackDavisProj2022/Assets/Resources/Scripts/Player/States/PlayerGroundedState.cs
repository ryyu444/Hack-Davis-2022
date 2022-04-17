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
        context.animator.SetBool("Grounded", true);
    }

    public override void Exit()
    {
        context.inputInfo.jumpPressedEvent -= Jump;
        context.inputInfo.LMBPressedEvent -= SwingAxe;
        context.animator.SetBool("Grounded", false);
    }

    public void Jump()
    {
        context.rb.velocity += new Vector3(0, 6, 0);
    }

    public void SwingAxe()
    {
        context.ChangeState(new PlayerPrimaryActionState());
    }

    public override void UpdateState()
    {
        context.rb.MoveWithRotation(context.cameraController.rotation, context.inputInfo.movementVector, 10f);
        context.animator.SetBool("Moving", context.rb.velocity.RemoveY().magnitude > 0.2f);
        if(context.animator.GetBool("Moving"))
            context.modelContainer.transform.RotateTowardsVelocity(context.rb, 6 * Time.deltaTime,true,90f);
        if(!context.IsGrounded())
        {
            context.ChangeState(new PlayerAirState());
        }
    }
}
