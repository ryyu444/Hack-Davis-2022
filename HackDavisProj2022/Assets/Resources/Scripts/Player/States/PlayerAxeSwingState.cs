using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxeSwingState: AbstractPlayerState
{
    private float timer = 1;
    public override void Enter(PlayerController context)
    {
        base.Enter(context);
    }

    public override void Exit()
    {

    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            context.ChangeState(new PlayerGroundedState());
    }
}
