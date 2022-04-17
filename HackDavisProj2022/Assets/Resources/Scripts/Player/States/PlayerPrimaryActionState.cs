using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryActionState: AbstractPlayerState
{
    private float timer = 2;
    private float delay = 0;
    public override void Enter(PlayerController context)
    {
        base.Enter(context);
        context.rb.velocity = Vector3.zero;
        context.animator.SetBool("PrimaryAction", true);
        //timers
        if(context.isLumberjack)
        {
            delay = 0.85f;
        }
        else
        {
            delay = 2f;
        }
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
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                if (context.isLumberjack)
                    LumberjackAction();
                else
                    SpiritAction();
            }
        }
    }

    private void SpiritAction()
    {
        var hits = Physics.OverlapBox(
            context.actionCollider.bounds.center,
            context.actionCollider.bounds.extents,
            context.actionCollider.transform.rotation,
            1 << 8);

        foreach (var hit in hits)
        {
            if (hit.gameObject.CompareTag("TreeStump"))
            {
                TimeManager.instance.SlowTime(0, 0.1f);
                hit.gameObject.GetComponent<PassThroughEvent>().PassThrough(context.gameObject);
            }
        }
    }

    private void LumberjackAction()
    {
        var hits = Physics.OverlapBox(
            context.actionCollider.bounds.center,
            context.actionCollider.bounds.extents,
            context.actionCollider.transform.rotation,
            1 << 8);

        foreach (var hit in hits)
        {
            if (hit.gameObject.CompareTag("TreeTrunk"))
            {
                TimeManager.instance.SlowTime(0, 0.1f);
                hit.gameObject.GetComponent<PassThroughEvent>().PassThrough(context.gameObject);
            }
        }
    }
    
}
