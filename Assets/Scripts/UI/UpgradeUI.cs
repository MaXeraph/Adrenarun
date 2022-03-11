using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;

public class UpgradeUI : MonoBehaviour
{

    static TMP_Text info;
    static Button[] choiceButtons;
    static Image[] choiceImages;
    static TMP_Text[] choiceTexts;

    public static PowerUpType[] powerUpSelectionList;
    public static UpgradeUI instance;

    public static RectTransform[] items;
    public static float offscreenDistance = 500;

    static bool valid = false;

    void Awake()
    {
        instance = this;
        info = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

        items = new RectTransform[4];
        foreach (RectTransform child in instance.transform)
        {
            child.localPosition += new Vector3(0, offscreenDistance, 0);
            items[child.GetSiblingIndex()] = child;
        }
        transform.localPosition += new Vector3(0, offscreenDistance, 0);



        Transform Choices = transform.GetChild(1);
        choiceButtons = Choices.GetComponentsInChildren<Button>();
        choiceImages = Choices.GetComponentsInChildren<Image>();
        choiceTexts = Choices.GetComponentsInChildren<TMP_Text>();

        for (var i = 0; i < 3; i++)
        {
            setButton(choiceButtons[i], i);
            choiceButtons[i].GetComponent<UpgradeChoice>().choiceNum = i;
        }

        gameObject.SetActive(false);
    }

    public static void init()
    {
        instance.gameObject.SetActive(true);
        valid = false;
        for (var i = 0; i < 3; i++)
        {
            choiceTexts[i].text = powerUpSelectionList[i].ToString();
            choiceImages[i].sprite = Globals.PowerUpIconDictionary[powerUpSelectionList[i]];
        }

        Sequence openSequence = DOTween.Sequence();
        openSequence.PrependInterval(2);
        openSequence.SetUpdate(UpdateType.Late, true);
        openSequence.OnComplete(Valid);
        foreach (RectTransform child in items)
        {
            openSequence.Insert(0, child.DOLocalMoveY(child.localPosition.y-offscreenDistance, 2 + (child.GetSiblingIndex()*0.5f)).SetEase(Ease.InOutElastic));
        }
        openSequence.Insert(0, instance.transform.DOLocalMoveY(instance.transform.localPosition.y - offscreenDistance, 2).SetEase(Ease.InOutElastic));
        openSequence.Play();
    }

    void setButton(Button targetButton, int choice)
    {
        targetButton.onClick.AddListener(() => choose(choice));
    }

    void choose(int num)
    { 
        if (num < 0 || num > 2 || !valid) return;
        UIManager.powerSelection = num;
        exit();
    }

    void exit()
    {
        Sequence openSequence = DOTween.Sequence();
        openSequence.PrependInterval(2);
        //openSequence.SetUpdate(UpdateType.Late, true);
        foreach (RectTransform child in items)
        {
            openSequence.Insert(0, child.DOLocalMoveY(child.localPosition.y + offscreenDistance, 2 + (child.GetSiblingIndex() * 0.5f)).SetEase(Ease.InOutElastic));
        }
        openSequence.Insert(0, instance.transform.DOLocalMoveY(instance.transform.localPosition.y + offscreenDistance, 2).SetEase(Ease.InOutElastic));
        openSequence.Play();

        Cursor.lockState = CursorLockMode.Locked;
        UIManager.UpdateWeapon();
        UIManager.Reloading = true;
        //gameObject.SetActive(false);
    }

    static void Valid()
    {
        valid = true;
    }
    

    public static void updateInfo(PowerUpType type)
    {
        info.text = Globals.PowerUpInfoDictionary[type];
    }

    

}
