using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class deathUI : MonoBehaviour
{

    private Image[] toFade;
    Button retry;
    Button leave;
    public static GameObject instance;

	static CanvasGroup items;

   void Awake()
    {
		instance = this.gameObject;
		items = transform.GetChild(0).GetComponent<CanvasGroup>();
		toFade = GetComponentsInChildren<Image>();
        retry = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>();
        leave = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
        retry.onClick.AddListener(rewind);
        leave.onClick.AddListener(quit);
		GetComponent<Canvas>().enabled = true;
		instance.SetActive(false);
    }

    public static void reveal(GameObject inst)
    {
		instance.SetActive(true);
		DOTween.To(() => PauseMenu.blurPanel.GetColor("_Color"), x => PauseMenu.blurPanel.SetColor("_Color", x), new Color(0.98f, 0.6f, 0.6f, 1), 1f);
		UIManager.dead = true;
		DOTween.To(() => PauseMenu.blurPanel.GetFloat("_Intensity"), x => PauseMenu.blurPanel.SetFloat("_Intensity", x), 1, 1.5f).OnComplete(show_content);
        PlayerCentral.paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	}

	private static void show_content()
    {
		items.DOFade(1, 1f);
	}

    void quit()
    {
        Application.Quit();
    }

   public void rewind()
    {
		Time.timeScale = 1f;

		Globals.TransitionPowerUpDictionary = new Dictionary<PowerUpType, int>()
	{
		{PowerUpType.DAMAGE, 0},
		{PowerUpType.FIRERATE, 0 },
		{PowerUpType.RELOADSPD, 0 },
		{PowerUpType.CLIPSIZE, 0 },
		{PowerUpType.ADRENALIN, 0 },
		{PowerUpType.SHOTGUN, 0 },
		{PowerUpType.REPEATER, 0 }
	};

		CompassUI.reset();
		PauseMenu.blurPanel.SetFloat("_Intensity", 0);
		PauseMenu.blurPanel.SetColor("_Color", new Color(1f, 1f, 1f, 0));
		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SpeedManager.coreSpeed = 1;
        PlayerCentral.paused = false;

		    UIManager.dead = false;
		      StopAllCoroutines();

        gameObject.SetActive(false);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

}
