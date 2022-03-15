using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class waveUI : MonoBehaviour
{
	static Text total;
	static Text left;

	public static int _total;
	public static int _left;

	void Awake()
	{
		left = transform.GetChild(0).GetComponent<Text>();
		total = transform.GetChild(1).GetComponent<Text>();
	}

	public static void setTotal(int num)
	{
		_total = num;
	}

	public static void setLeft(int num)
	{
		left.text = num.ToString() + " / " + _total.ToString();
	}

}
