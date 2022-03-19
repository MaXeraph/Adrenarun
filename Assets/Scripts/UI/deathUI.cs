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

	static Krivodeling.UI.Effects.UIBlur blurPanel;
	static CanvasGroup items;

   void Awake()
    {
        instance = this.gameObject;
		blurPanel = GetComponent<Krivodeling.UI.Effects.UIBlur>();
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
        UIManager.dead = true;
		DOTween.To(() => blurPanel.Intensity, x => blurPanel.Intensity = x, 1, 2).OnComplete(show_content);
		instance.SetActive(true);
        PlayerCentral.paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

	private static void show_content()
    {
		items.DOFade(1, 2);
	}

    void quit()
    {
        Application.Quit();
    }

   public void rewind()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SpeedManager.updateSpeeds(1f);
        PlayerCentral.paused = false;
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
