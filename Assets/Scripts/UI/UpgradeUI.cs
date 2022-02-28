using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UpgradeUI : MonoBehaviour
{

    static TMP_Text info;

    static Button[] choiceButtons;

    static Image[] choiceImages;

    static TMP_Text[] choiceTexts;



    public static PowerUpType[] powerUpSelectionList;
    public static UpgradeUI instance;




    void Awake()
    {
        instance = this;
        info = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

        Transform Choices = transform.GetChild(1);
        choiceButtons = Choices.GetComponentsInChildren<Button>();
        choiceImages = Choices.GetComponentsInChildren<Image>();
        choiceTexts = Choices.GetComponentsInChildren<TMP_Text>();

        for (var i = 0; i < 3; i++)
        {
            choiceButtons[i].onClick.AddListener(() => choose(i));
        }

        gameObject.SetActive(false);
    }

    public static void init()
    {
        for (var i = 0; i < 3; i++)
        {
            choiceTexts[i].text = powerUpSelectionList[i].ToString();
        }
    }

    void choose(int num)
    {
        if (num < 0 || num > 2) return;
        UIManager.powerSelection = num;
        gameObject.SetActive(false);
    }

    void exit()
    {
        gameObject.SetActive(false);
    }

    

    public static void upateInfo(string _info)
    {
        info.text = _info;
    }

    

}
