using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableUI : MonoBehaviour
{
	public static GameObject pill1;
	public static GameObject pill2;

	void Awake()
    {
		pill1 = transform.GetChild(0).gameObject;
		pill2 = transform.GetChild(1).gameObject;
    }

    public static void update_pill_amount(int amount)
    {
		if(amount > 0)
        {
			pill1.SetActive(true);
			if (amount > 1) pill2.SetActive(true);
			else pill2.SetActive(false);

		}
        else { pill1.SetActive(false); pill2.SetActive(false); }
    }
}
