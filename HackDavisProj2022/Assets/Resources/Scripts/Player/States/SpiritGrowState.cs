using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritGrowState : AbstractPlayerState
{
    public override void Enter(PlayerController context)
    {
        base.Enter(context);
        //For now just instantly start the growth lol
        context.growthTarget.StartRegrowth();
    }

    public override void Exit()
    {

    }

    public override void UpdateState()
    {
        context.ChangeState(new PlayerGroundedState());
    }
}
