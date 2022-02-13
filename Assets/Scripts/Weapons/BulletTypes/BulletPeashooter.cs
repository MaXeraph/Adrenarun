using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPeashooter : MonoBehaviour, Bullet
{   
    public int clipSize
    {
        get
        {
            return 100;
        }
    }

    public float fireRate
    {
        get
        {
            return 1f;
        }
    }

    public float damage
    {
        get
        {
            return 1f;
        }
    }
    public void onShoot()
    {
        Debug.Log("Peashooter Shooting");
    }

    public void onHit()
    {
        Debug.Log("Peashooter Hit something");
    }

    public void onDestroy()
    {
        Debug.Log("Peashooter Destroyed");
    }
}
