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
    public int nextScene;
    public float textSpeed;
    private int index;
    public float textDelay;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    public void StartDialogue() {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        foreach (char c in lines[index].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        if(index == lines.Length - 1)
        {
            yield return new WaitForSeconds(4f);
            BlackFadeThingScriptLOL.instance.FadeTransition(false);
            yield return new WaitForSeconds(4.5f);
            SceneManager.LoadSceneAsync(nextScene);
        }
    }
    public void NextLine() {
        if (index < lines.Length - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else {
            gameObject.SetActive(false);
        }
    }
}
