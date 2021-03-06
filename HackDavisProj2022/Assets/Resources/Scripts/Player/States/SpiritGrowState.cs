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
        context.StartCoroutine(SceneLoad());
        context.rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    IEnumerator SceneLoad()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
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
        SceneManager.UnloadSceneAsync(3);
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
        SceneManager.UnloadSceneAsync(3);
        context.ChangeState(new PlayerGroundedState());
    }
}
