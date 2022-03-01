using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class deathUI : MonoBehaviour
{

    private Image[] toFade;
    Button retry;
    Button leave;
    public static GameObject instance;





   void Awake()
    {
        instance = this.gameObject;
        toFade = GetComponentsInChildren<Image>();
        retry = transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
        leave = transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
        retry.onClick.AddListener(rewind);
        leave.onClick.AddListener(quit);
        instance.SetActive(false);
    }

    public static void reveal(GameObject inst)
    {
        instance.SetActive(true);
        PlayerCentral.paused = true;
        //TimeBody.isRecording = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void quit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

   public void rewind()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        SpeedManager.updateSpeeds(1f);
        PlayerCentral.paused = false;
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        //GetComponent<Image>().DOFade(0, 0.5f);
        //foreach (Image img in toFade){
        //   img.DOFade(0, 0.5f);
        //}
        
        //TimeBody.StartRewind();
    }
}
