using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ConsumableUI : MonoBehaviour
{
	public static Transform pill1;
	public static Transform pill2;

	void Awake()
    {
		Transform parent = transform;
		pill1 = parent.GetChild(2);
		pill2 = parent.GetChild(3);
		change_pill(pill1, false); change_pill(pill2, false);

	}

    public static void update_pill_amount(int amount)
    {
		if(amount > 0)
        {
			change_pill(pill1, true);
			if (amount > 1) change_pill(pill2, true);
			else change_pill(pill2, false);

		}
        else { change_pill(pill1,false); change_pill(pill2, false); }
    }

	public static void change_pill(Transform pill,bool state)
    {
		float Target = 0;
		if (state) Target = 1;
		pill.DOScale(Target, 0.2f);
    }
}
