using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class waveUI : MonoBehaviour
{
    static Text total;
    static Text left;

    public static int _total;
    public static int _left;

    void Awake()
    {
        left = transform.GetChild(0).GetComponent<Text>();
        total = transform.GetChild(1).GetComponent<Text>();
    }

    public static void setTotal(int num)
    {
        string pre = "";
        if (num < 10) pre = " ";
        total.text = pre + num.ToString();
        _total = num;
    }

    public static void setLeft(int num)
    {
        string pre = "";
        if (num < 10) pre = " ";
        left.text = pre + num.ToString();
        _left = num;
        left.transform.DOPunchScale(new Vector3(0.1f, 1.3f, 1.3f), 0.5f, 1, 0.1f);
    }

}
