using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeChoice : MonoBehaviour
{
    public int choiceNum;

    void Start()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnterDelegate(); });
        eventTrigger.triggers.Add(entry);
    }

    public void OnPointerEnterDelegate()
    {
        PowerUpType type = UpgradeUI.powerUpSelectionList[choiceNum];
        UpgradeUI.updateInfo(type); 
    }
}
