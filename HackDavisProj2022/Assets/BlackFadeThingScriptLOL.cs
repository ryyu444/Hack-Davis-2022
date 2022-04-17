using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlackFadeThingScriptLOL : MonoBehaviour
{
    public static BlackFadeThingScriptLOL instance;

    public Image ui;

    private void Awake()
    {
        instance = this;
        FadeTransition(true);
    }

    public void FadeTransition(bool fadeIn)
    {
        ui.raycastTarget = false;
        StartCoroutine(Corout_Transition(fadeIn));
    }

    IEnumerator Corout_Transition(bool fadeIn)
    {
        
        yield return new WaitForSeconds(1.5f);
        Color c = ui.color;
        float time = 2.5f;
        float timer = 0f;

        while(timer < time)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0,1,timer / time);
            c.a = fadeIn ? 1 - t:t;
            ui.color = c;
            yield return new WaitForEndOfFrame();
        }
        c.a = fadeIn ? 0 : 1;
        ui.color = c;
        ui.raycastTarget = !fadeIn;
    }
}
