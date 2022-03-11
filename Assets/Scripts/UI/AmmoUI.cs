using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AmmoUI : MonoBehaviour
{

    public static int AmmoCapacity = 0;
    private static int CurrentAmmo;

    private static GameObject[] AmmoSlots;

    public static GameObject AmmoSlot;
    public static Transform Clip;
    public static Text AmmoCounter;
    public static GameObject AmmoPanel;

    private static Image AmmoPanelColor;
    private static Vector4 PanelColorNormal = new Vector4(0.7f, 0.6f, 0.6f, 0.8f);
    private static Vector4 PanelColorReload = new Vector4(0.9f, 0f, 0.1f, 0.5f);

    public static AmmoUI instance;

    void Awake()
    {
        instance = this;
        Clip = transform.GetChild(1);
        AmmoSlot = Clip.GetChild(0).gameObject;
        AmmoCounter = transform.GetChild(0).GetComponent<Text>();

        AmmoPanelColor = GetComponent<Image>();
        PanelColorNormal = AmmoPanelColor.color;
    }

    
  
    public static void UpdateAmmoCapacity(int capacity)
    {
  

        for (var i = 0; i < AmmoCapacity; i++)
        {
            if (i != 0) { Destroy(AmmoSlots[i]); }
        }

        AmmoCapacity = capacity;
        
        AmmoSlots = new GameObject[AmmoCapacity];

        for (var i = 0; i < AmmoCapacity; i++)
        {
            if (i == 0) { AmmoSlots[i] = AmmoSlot; }
            else
            {

                GameObject copy = Instantiate(AmmoSlot);
                copy.transform.SetParent(Clip);
                copy.transform.localScale = AmmoSlot.transform.localScale;
                AmmoSlots[i] = copy;
            }
        }
        UpdateCounter();
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
                AmmoSlots[current].GetComponent<ammoSlot>().used = false;
                AmmoPanelColor.color = PanelColorReload;
                if (current == AmmoCapacity-1) { AmmoPanelColor.color = PanelColorNormal; }
            }

            else if (current != AmmoCapacity && current <= CurrentAmmo)
            {
                AmmoSlots[current].GetComponent<ammoSlot>().used = true;
                AmmoPanelColor.color = PanelColorNormal; 
            }
        }
    }

}
