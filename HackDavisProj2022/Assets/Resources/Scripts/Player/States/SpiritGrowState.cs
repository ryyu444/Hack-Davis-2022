using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SpiritGrowState : AbstractPlayerState
{
    private GameManager minigameManager;
    public override void Enter(PlayerController context)
    {
        base.Enter(context);
        //Create Minigame context
        SceneManager.LoadSceneAsync(2,LoadSceneMode.Additive);
        minigameManager = GameObject.FindObjectOfType<GameManager>();
        minigameManager.OnWinEvent += OnWin;
        minigameManager.OnLoseEvent += OnLose;
    }

    public override void Exit()
    {
        minigameManager.OnWinEvent -= OnWin;
        minigameManager.OnLoseEvent -= OnLose;
    }

    public override void UpdateState()
    {
        context.rb.velocity = Vector3.zero;
    }

    public void OnWin()
    {
        SceneManager.UnloadSceneAsync(2);
        context.growthTarget.StartRegrowth();
        context.ChangeState(new PlayerGroundedState());
    }

    public void OnLose()
    {
        SceneManager.UnloadSceneAsync(2);
        context.ChangeState(new PlayerGroundedState());
    }
}
