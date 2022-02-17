using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_health : MonoBehaviour
{
    public Slider slider;
    public Image slider_fill;
    public Image slow_fill;
    private float current;

    private Animation anim;
    private bool dead = false;  
    

    void Start()
    {
        ColorChanger();
    }

    public void setMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        current = health;
    }

    public void setHealth(float health)
    {
        if (health < current)
        {
                impact_fill(health);
        }

        else { slow_fill.fillAmount = health / slider.maxValue; }

        current = health;
        slider.value = current;
        ColorChanger();

        if (current <= 0 && !dead)
        {
            dead = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void impact_fill(float fill)
    {
        Sequence damaged = DOTween.Sequence();
        damaged.Append(transform.DOShakePosition(0.075f, new Vector3(2f, 2f, 2f), 10, 15f).SetLoops(3));
        damaged.Insert(0,transform.DOShakeRotation(0.075f, new Vector3(2, 2, 2), 10, 15f).SetLoops(3));
        damaged.Append(slow_fill.DOFillAmount(fill / slider.maxValue, 0.4f));
    }

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (slider.value / slider.maxValue));
        slider_fill.color = healthColor;
    }
}
