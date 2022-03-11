using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public static void open(GameObject menu = null)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void close()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void quit()
    {
        Application.Quit();
    }

    public static void restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SpeedManager.updateSpeeds(1f);
        PlayerCentral.paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public static void pause()
    {
        SpeedManager.updateSpeeds(0);
        PlayerCentral.paused = true;
    }

    public static void resume()
    {
        Time.timeScale = 1f;
        SpeedManager.updateSpeeds(UIManager.Health/UIManager.MaxHealth);
        PlayerCentral.paused = false;
    }


}
