using System.Collections;
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
            setButton(choiceButtons[i], i);
            choiceButtons[i].GetComponent<UpgradeChoice>().choiceNum = i;
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

    void setButton(Button targetButton, int choice)
    {
        targetButton.onClick.AddListener(() => choose(choice));
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
        Cursor.lockState = CursorLockMode.Locked;
    }
    

    public static void updateInfo(PowerUpType type)
    {
        info.text = Globals.PowerUpInfoDictionary[type];
    }

    

}
