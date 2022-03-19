using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimatePill : MonoBehaviour
{
	RectTransform body;
	public float delay = 0;

	void Start()
    {
		body = GetComponent<RectTransform>();
		Animate(delay);
    }

	void Animate(float delay)
    {
		body.DOJumpAnchorPos(body.anchoredPosition, 3f, 1, 1.5f).SetLoops(-1).SetEase(Ease.InQuad).SetDelay(delay);
    }


}
