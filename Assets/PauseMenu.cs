using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{

	static UI.Effects.UIBlur blurPanel;
	static CanvasGroup items;
	static Slider MouseSensitivity;

	static float previousChange = 1;
	float previousCore;


	static GameObject instance;

	bool revealed = false;

	void Awake()
    {
		instance = this.gameObject;
		blurPanel = transform.GetChild(0).GetComponent<UI.Effects.UIBlur>();
		items = transform.GetChild(1).GetComponent<CanvasGroup>();
		MouseSensitivity = transform.GetChild(1).GetChild(2).GetComponent<Slider>();
		MouseSensitivity.onValueChanged.AddListener(delegate { change_sensitivity(); });
	}

	public void reveal()
	{
		DOTween.To(() => blurPanel.Intensity, x => blurPanel.Intensity = x, 1, 0.5f).OnComplete(show_content);
		previousCore = SpeedManager.coreSpeed;
		SpeedManager.coreSpeed = 0;
		SpeedManager.updateGameObjectSpeed();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		PlayerCentral.paused = true;
	}

	private void show_content()
	{
		items.DOFade(1, 0.5f);
	}

	private void hide_content()
	{
		DOTween.To(() => blurPanel.Intensity, x => blurPanel.Intensity = x, 0, 0.5f);
	}

	public void exit()
    {
		items.DOFade(0, 0.5f).OnComplete(hide_content);
		SpeedManager.coreSpeed = previousCore;
		SpeedManager.updateGameObjectSpeed();
		instance.SetActive(true);
		Cursor.lockState = CursorLockMode.Locked;
		PlayerCentral.paused = false;
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
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (!revealed) { reveal(); revealed = true; }
			else { exit(); revealed = false; }
		}
    }
}
