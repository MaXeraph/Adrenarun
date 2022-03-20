using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{

	public static Material blurPanel;
	Image blur;
	CanvasGroup items;
	static Slider MouseSensitivity;

	static float previousChange = 1;

	int _intensityId;
	int _colorId;
	int _flipXId;
	int _flipYId;
	int _multiplierId;


	public static GameObject instance;

	bool revealed = false;

	void Awake()
    {
		DOTween.defaultTimeScaleIndependent = true;	
		instance = this.gameObject;
		blur = transform.GetChild(0).GetComponent<Image>();
		items = transform.GetChild(1).GetComponent<CanvasGroup>();
		MouseSensitivity = transform.GetChild(1).GetChild(2).GetComponent<Slider>();
		MouseSensitivity.onValueChanged.AddListener(delegate { change_sensitivity(); });
		blurPanel = blur.material;
		blurPanel.SetFloat("_Intensity", 0);
		blurPanel.SetFloat("_Multiplier", 0.5f);
		blurPanel.SetColor("_Color", new Color(1f, 1f, 1f, 0));
		MouseSensitivity.value = previousChange;
		change_sensitivity();
	}

	public void reveal()
	{
		blurPanel.SetFloat("_Intensity", 0);
		blurPanel.SetColor("_Color", new Color(1f, 1f, 1f, 0));
		DOTween.To(() => blurPanel.GetColor("_Color"), x => blurPanel.SetColor("_Color", x), new Color(0.6f, 0.6f, 0.6f, 1), 0.4f);
		blur.DOFade(1, 0.4f);
		DOTween.To(() => blurPanel.GetFloat("_Intensity"), x => blurPanel.SetFloat("_Intensity", x), 1, 0.5f).OnComplete(show_content);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		PlayerCentral.paused = true;
		Time.timeScale = 0;
		instance.SetActive(true);
	}

	private void show_content()
	{
		items.DOFade(1, 0.5f);
	}

	private void hide_content()
	{
		DOTween.To(() => blurPanel.GetColor("_Color"), x => blurPanel.SetColor("_Color", x), new Color(1, 1, 1, 0), 0.4f);
		DOTween.To(() => blurPanel.GetFloat("_Intensity"), x => blurPanel.SetFloat("_Intensity", x), 0, 0.5f);
	}

	public void exit()
    {
		Time.timeScale = 1;
		items.DOFade(0, 0.5f).OnComplete(hide_content);
		PlayerCentral.paused = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

	}

	public static void change_sensitivity()
    {
		float change = MouseSensitivity.value;
		Movement.mouseSensitivity /= previousChange;
		Movement.mouseSensitivity *= change;
		previousChange = change;
	}

	void Update()
    {
		if (Input.GetKeyDown(KeyCode.P) && !UIManager.dead)
		{
			if (!revealed) { reveal(); revealed = true; }
			else { exit(); revealed = false; }
		}
    }
}
