using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue3 : MonoBehaviour
{
    public static Dialogue3 instance;

    public TextMeshProUGUI textComponent;
    public string[] lines;
    public Color[] colors;
    public int nextScene;
    public float textSpeed;
    private int index;
    public float textDelay;

    public int autoPlayCount;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        textComponent.text = string.Empty;
        frameGuard = true;
        Invoke("StartDialogue", 5f);
    }

    public void StartDialogue() {
        index = 0;
        frameGuard = false;
        currentLineShowTHingIwhgot = StartCoroutine(TypeLine());
    }

    private Coroutine currentLineShowTHingIwhgot;
    bool endTransition = false;
    IEnumerator TypeLine() {
        frameGuard = false;
        int queueIndex = index;
        endTransition = queueIndex == lines.Length - 1;
        textComponent.text = "";
        textComponent.color = colors[queueIndex];
        foreach (char c in lines[index].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        if (autoPlayCount > 0)
        {
            autoPlayCount--;
            yield return new WaitForSeconds(textDelay);
            NextLine();
        }
        if(queueIndex == lines.Length - 1)
        {
            yield return new WaitForSeconds(4f);
            BlackFadeThingScriptLOL.instance.FadeTransition(false);
            yield return new WaitForSeconds(4.5f);
            SceneManager.LoadScene(nextScene);
        }
    }

    private bool frameGuard = false;

    public void NextLine() {
        if (index < lines.Length - 1 && !endTransition && !frameGuard) {
            frameGuard = true;
            index++;
            textComponent.text = string.Empty;
            if (currentLineShowTHingIwhgot != null)
                StopCoroutine(currentLineShowTHingIwhgot);
            currentLineShowTHingIwhgot = StartCoroutine(TypeLine());
        }
    }
}
