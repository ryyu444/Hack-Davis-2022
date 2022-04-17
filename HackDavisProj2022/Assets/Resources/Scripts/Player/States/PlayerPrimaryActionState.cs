using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryActionState: AbstractPlayerState
{
    private float timer = 2;
    public override void Enter(PlayerController context)
    {
        base.Enter(context);
        context.rb.velocity = Vector3.zero;
        context.animator.SetBool("PrimaryAction", true);
    }

    public override void Exit()
    {
        context.animator.SetBool("PrimaryAction", false);
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            context.ChangeState(new PlayerGroundedState());
    }
}
