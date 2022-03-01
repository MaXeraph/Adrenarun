using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public Button start;
    public Button cont;
    public Button quit;

    void Start()
    {
        start.onClick.AddListener(() => ButtonPress("start"));
        cont.onClick.AddListener(() => ButtonPress("continue"));
        quit.onClick.AddListener(() => ButtonPress("quit"));
    }

    void ButtonPress(string action)
    {
        switch (action)
        {
            case "start":
                SceneManager.LoadScene("LevelPrototypeScene");
                break;
            case "continue":
                //Possibly implement a save system...
                Debug.Log("Continue Game...");
                break;
            case "quit":
                Application.Quit();
                UnityEditor.EditorApplication.isPlaying = false;
                break;
        }
    }
}
