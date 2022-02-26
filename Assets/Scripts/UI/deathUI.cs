using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

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
        inst.SetActive(true);
        PlayerCentral.paused = true;
        TimeBody.isRecording = false;
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
        //GetComponent<Image>().DOFade(0, 0.5f);
        //foreach (Image img in toFade){
        //   img.DOFade(0, 0.5f);
        //}
        gameObject.SetActive(false);
        TimeBody.StartRewind();
    }
}
