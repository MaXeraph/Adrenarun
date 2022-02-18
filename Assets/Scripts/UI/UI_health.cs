using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_health : MonoBehaviour
{
    public static Slider slider;
    public static Image slider_fill;
    public static Image slow_fill;
    private static float current;
    private static float max;
    private static Vector3 start_pos;

    private static bool dead = false;  

    public static UI_health instance;

    void Awake()
    {
        instance = this;
        slider = GetComponent<Slider>();
        slider_fill = GameObject.Find("Fill").GetComponent<Image>();
        slow_fill = transform.Find("Slow").GetComponent<Image>();
    }

    void Start()
    {
        start_pos = transform.position;
        ColorChanger();
    }

    public static void setMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        current = health;
        max = health;
    }

    public static void setHealth(float health)
    {
        if (health < current)
        {
                impact_fill(health);
        }

        else { slow_fill.fillAmount = health / max; }

        current = health;
        slider.value = current;
        ColorChanger();

        if (current <= 0 && !dead)
        {
            dead = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public static void impact_fill(float fill)
    {
        Sequence damaged = DOTween.Sequence();
        damaged.Append(instance.transform.DOShakePosition(0.075f, new Vector3(2f, 2f, 2f), 10, 15f).SetLoops(3));
        damaged.Insert(0, instance.transform.DOShakeRotation(0.075f, new Vector3(2, 2, 2), 10, 15f).SetLoops(3));
        damaged.Append(slow_fill.DOFillAmount(fill / max, 0.4f));
        damaged.Insert(1, instance.transform.DOMove(start_pos, 0.1f));
        damaged.Insert(1, instance.transform.DORotate(new Vector3(0, 0, 0), 0.1f));
    }

    public static void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (current / max));
        slider_fill.color = healthColor;
    }
}
