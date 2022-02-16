using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lean.Gui;

public class UI_health : MonoBehaviour
{
   // public GameObject gameOver;
   // public Animation deathAnim;
    public Slider slider;
    public Image slider_fill;
    public Image slow_fill;
    private float current;
    public LeanShake shake;

    private Animation anim;
    private bool dead = false;

    void Start()
    {
        anim = GetComponent<Animation>();
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
            shake.Shake(5);
            DOTween.To(() => shake.Strength, x => shake.Strength = x, 0f, 0.25f);
        }

        current = health;
        slider.value = current;
        ColorChanger();
        slow_fill.DOFillAmount(current / slider.maxValue, 0.5f);
        if (current <= 0 && !dead)
        {
            dead = true;
            Cursor.lockState = CursorLockMode.None;
            //gameOver.SetActive(true);
            //deathAnim.Play();
        }
    }

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (slider.value / slider.maxValue));
        slider_fill.color = healthColor;
    }
}
