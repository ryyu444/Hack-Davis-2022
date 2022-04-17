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
        context.rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public override void Exit()
    {
        minigameManager.OnWinEvent -= OnWin;
        minigameManager.OnLoseEvent -= OnLose;
        context.rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private bool subscribed = false;
    public override void UpdateState()
    {
        minigameManager = GameObject.FindObjectOfType<GameManager>();
        if(minigameManager != null && !subscribed)
        {
            subscribed = true;
            minigameManager.OnWinEvent += OnWin;
            minigameManager.OnLoseEvent += OnLose;
        }
    }

    public void OnWin()
    {
        context.StartCoroutine(Corout_OnWin());
    }

    private IEnumerator Corout_OnWin()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.UnloadSceneAsync(2);
        context.growthTarget.StartRegrowth();
        context.ChangeState(new PlayerGroundedState());
    }

    public void OnLose()
    {
        context.StartCoroutine(Corout_OnLose());        
    }

    private IEnumerator Corout_OnLose()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.UnloadSceneAsync(2);
        context.ChangeState(new PlayerGroundedState());
    }
}
