using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    public Button start;
    public Button tutorial;
    public Button quit;
	public static RectTransform title;
	private static RectTransform _start;
	private static RectTransform _cont;
	private static RectTransform _quit;

	public static float scale_amount = 1.25f;

	public static Sequence anim;

	Transform background;

	void Start()
    {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		start.onClick.AddListener(() => ButtonPress("start"));
        tutorial.onClick.AddListener(() => ButtonPress("tutorial"));
        quit.onClick.AddListener(() => ButtonPress("quit"));

		title = transform.GetChild(4).GetComponent<RectTransform>();
		background = transform.GetChild(5);
		_start = start.transform.GetChild(0).GetComponent<RectTransform>();
		_cont = cont.transform.GetChild(0).GetComponent<RectTransform>();
		_quit = quit.transform.GetChild(0).GetComponent<RectTransform>();
		title.DOJumpAnchorPos(title.anchoredPosition, 10f, 1, 1.5f).SetLoops(-1).SetEase(Ease.InOutQuad);
	}

	void Update()
    {
		background.position -= new Vector3(0, 0, 0.5f);
    }

    void ButtonPress(string action)
    {
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

	public static void OnMouseOver(string item)
	{
		DOTween.SmoothRewind(anim);
		anim = DOTween.Sequence().SetEase(Ease.InOutQuad);
		switch (item)
		{
			case "title":
				anim.Insert(0, title.DOScale(scale_amount, 0.5f));
				break;
			case "start":
				anim.Insert(0, _start.DOScale(scale_amount, 0.5f));
				break;
			case "continue":
				anim.Insert(0, _cont.DOScale(scale_amount, 0.5f));
				break;
			case "quit":
				anim.Insert(0, _quit.DOScale(scale_amount, 0.5f));
				break;
		}
	}

	public static void OnMouseExit(string item)
	{
		DOTween.SmoothRewind(anim);
		anim = DOTween.Sequence().SetEase(Ease.InOutQuad);
		switch (item)
		{
			case "title":
				anim.Insert(0, title.DOScale(1, 0.5f));
				break;
			case "start":
				anim.Insert(0, _start.DOScale(1, 0.5f));
				break;
			case "continue":
				anim.Insert(0, _cont.DOScale(1, 0.5f));
				break;
			case "quit":
				anim.Insert(0, _quit.DOScale(1, 0.5f));
				break;
		}
	}
}
