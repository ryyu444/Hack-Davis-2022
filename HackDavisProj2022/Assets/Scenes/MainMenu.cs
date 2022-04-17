using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        BlackFadeThingScriptLOL.instance.FadeTransition(false);
        Invoke("PlayGameButCooler", 4f);
    }
    public void PlayGameButCooler() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Application.Quit();
    }

}
