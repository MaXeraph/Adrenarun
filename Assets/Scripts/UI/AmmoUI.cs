using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AmmoUI : MonoBehaviour
{
    public int Capacity
    {
        get { return AmmoCapacity; }
        set { UpdateAmmoCapacity(value); }
    }

    public int AmmoCapacity = 0;
    private  int CurrentAmmo;

    private  GameObject[] AmmoSlots;
    [SerializeField] private GameObject AmmoSlot;
    [SerializeField] private GameObject Clip;
    [SerializeField] private GameObject AmmoCounter;
    [SerializeField] private GameObject AmmoPanel;
    private Image AmmoPanelColor;
    private Vector4 PanelColorNormal = new Vector4(0.7f, 0.6f, 0.6f, 0.8f);
    private static Vector4 PanelColorReload = new Vector4(0.9f, 0f, 0.1f, 0.5f);

    void Awake()
    {
        AmmoPanelColor = AmmoPanel.GetComponent<Image>();
        PanelColorNormal = AmmoPanelColor.color;
    }
  
    public void UpdateAmmoCapacity(int capacity)
    {
        AmmoPanelColor = AmmoPanel.GetComponent<Image>();
        PanelColorNormal = AmmoPanelColor.color;

        for (var i = 0; i < AmmoCapacity; i++)
        {
            if (i != 0) { Destroy(AmmoSlots[i]); }
        }

        AmmoCapacity = capacity;
        
        AmmoSlots = new GameObject[AmmoCapacity];

        for (var i = 0; i < AmmoCapacity; i++)
        {
            if (i == 0) {AmmoSlots[i] = AmmoSlot;}
            else
            {
                
                GameObject copy = Instantiate(AmmoSlot);
                copy.transform.SetParent(Clip.transform);
                copy.transform.localScale = AmmoSlot.transform.localScale;
                AmmoSlots[i] = copy;
            }
        }
        UpdateCounter();
    }

    public void UpdateCounter()
    {
        if (CurrentAmmo >= 10)
            {
            AmmoCounter.GetComponent<Text>().text = CurrentAmmo.ToString();
        }

        else
        {
            AmmoCounter.GetComponent<Text>().text = "0" + CurrentAmmo.ToString();
        }

    }

    public void UpdateAmmo(int current, bool reloading = false)
    {
        if (current != CurrentAmmo)
        {
            CurrentAmmo = current;

            UpdateCounter();
          
            if (reloading && current != AmmoCapacity)
            {
                AmmoSlots[current].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
                AmmoPanelColor.color = PanelColorReload;
                if (current == AmmoCapacity-1) { AmmoPanelColor.color = PanelColorNormal; }
            }

            else if (current != AmmoCapacity && current <= CurrentAmmo){ AmmoSlots[current].transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f); AmmoPanelColor.color = PanelColorNormal; }
        }
    }

}
