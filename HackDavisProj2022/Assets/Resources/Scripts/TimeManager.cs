using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    private Coroutine corout;

    private float fixedDTime;

    private void Awake()
    {
        instance = this;
        fixedDTime = Time.fixedDeltaTime;
    }

    public void SlowTime(float slow, float duration)
    {
        if(corout != null)
            StopCoroutine(corout);
        corout = StartCoroutine(Corout_SlowTime(slow, duration));
    }

    private IEnumerator Corout_SlowTime(float slow, float duration)
    {
        if (slow == 0)
            slow = 0.01f;
        Time.timeScale = slow;
        Time.fixedDeltaTime = fixedDTime / slow;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedDTime;
    }
}
