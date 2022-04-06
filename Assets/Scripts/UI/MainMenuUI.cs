using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public Button start;
    public Button tutorial;
    public Button quit;

    void Start()
    {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		start.onClick.AddListener(() => ButtonPress("start"));
        tutorial.onClick.AddListener(() => ButtonPress("tutorial"));
        quit.onClick.AddListener(() => ButtonPress("quit"));
    }

    void ButtonPress(string action)
    {
		AudioManager.PlayMenuSelectAudio();
        switch (action)
        {
            case "start":
				LevelTransition.init();
                break;
            case "tutorial":
                SceneManager.LoadScene(1);
                break;
            case "quit":
                Application.Quit();
                break;
        }
    }
}
