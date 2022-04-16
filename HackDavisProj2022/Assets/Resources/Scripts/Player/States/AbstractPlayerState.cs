using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayerState
{
    protected PlayerController context;

    public virtual void Enter(PlayerController context)
    {
        this.context = context;
    }
    public abstract void Exit();
    public abstract void UpdateState();


}
