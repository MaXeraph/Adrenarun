using System.Collections;
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
		PauseMenu.blurPanel.SetFloat("_Intensity", 0);
		PauseMenu.blurPanel.SetColor("_Color", new Color(1f, 1f, 1f, 0));
		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SpeedManager.updateSpeeds(1f);
        PlayerCentral.paused = false;
		//CompassUI.instance.gameObject.SetActive(false);
		//CompassUI.instance.gameObject.SetActive(true);
		UIManager.dead = false;
		CompassUI.enemies.Clear();
		CompassUI.enemyMarkers.Clear();

		StopAllCoroutines();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		gameObject.SetActive(false);
	}
}
