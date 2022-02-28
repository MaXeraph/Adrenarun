using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairUI : MonoBehaviour
{

    public static Transform[] bulletIndicators;

    void Start()
    {
        bulletIndicators = new Transform[6];
    }

    public static void addIndicator(Collider c)
    {
        //GameObject indicator = Instantiate(Resources.Load("BulletIndicator")) as GameObject;
        //indicator.GetComponent<bulletIndicator>().bulletLocation = c.GetComponent<Transform>();
        //indicator.transform.SetParent(transform);
    }

    void removeIndicator()
    {
        //bulletIndicators.Shift();
    }

}
