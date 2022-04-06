using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AmmoUI : MonoBehaviour
{

    public static int AmmoCapacity = 1;
    private static int CurrentAmmo;

    public static ammoSlot[] AmmoSlots;

    public static ammoSlot AmmoSlot;
    public static Transform Clip;
    public static Text AmmoCounter;
    public static GameObject AmmoPanel;

    private static Image AmmoPanelColor;
    private static Vector4 PanelColorNormal = new Vector4(0.7f, 0.6f, 0.6f, 0.8f);
    private static Vector4 PanelColorReload = new Vector4(0.9f, 0f, 0.1f, 0.5f);

	public static bool changingClip = false;

    public static AmmoUI instance;

	public static bool has_updated = false;

    void Awake()
    {
		AmmoSlots = new ammoSlot[0];
		AmmoCapacity = 1;
		instance = this;
		Clip = transform.GetChild(1);
		AmmoSlot = Clip.GetChild(0).GetComponent<ammoSlot>();
		AmmoCounter = transform.GetChild(0).GetComponent<Text>();
		AmmoPanelColor = GetComponent<Image>();
		PanelColorNormal = AmmoPanelColor.color;
	}

    public static void resetSlots()
    {
		if (AmmoSlots != null && AmmoSlots.Length > 1) { foreach (ammoSlot child in AmmoSlots) child.used = false; }
	}
  
    public static void UpdateAmmoCapacity(int capacity)
    {
		//instance.StopAllCoroutines();
		//resetSlots();
		changingClip = true;
		for (var i = AmmoCapacity; i < capacity; i++)
		{

			ammoSlot copy = Instantiate(AmmoSlot);
			copy.transform.SetParent(Clip);
			copy.transform.localScale = AmmoSlot.transform.localScale;
			if (i == capacity - 1)
			{
				UIManager.Ammo = capacity;
				AmmoCapacity = capacity;
				//Reset array
				AmmoSlots = new ammoSlot[capacity];
				//Add back to array
				AmmoSlots = Clip.GetComponentsInChildren<ammoSlot>();
				CurrentAmmo = AmmoSlots.Length;
				changingClip = false;
				AmmoCounter.text = capacity.ToString();
				//resetSlots();
			}
		}

    }



    public static void UpdateCounter()
    {
        if (CurrentAmmo >= 10)
            {
            AmmoCounter.text = CurrentAmmo.ToString();
        }

        else
        {
            AmmoCounter.text = "0" + CurrentAmmo.ToString();
        }

    }



    public static void UpdateAmmo(int current, bool reloading = false)
    {
        if (current != CurrentAmmo)
        {
            CurrentAmmo = current;

            UpdateCounter();
          
            if (reloading && current != AmmoCapacity)
            {
				AmmoSlots[current].used = false;
				AmmoPanelColor.color = PanelColorReload;
                if (current == AmmoCapacity-1) { AmmoPanelColor.color = PanelColorNormal; }
            }

            else if (current != AmmoCapacity && current <= CurrentAmmo){
				AmmoSlots[current].used = true;
				AmmoPanelColor.color = PanelColorNormal; 
			}
        }
    }

}
