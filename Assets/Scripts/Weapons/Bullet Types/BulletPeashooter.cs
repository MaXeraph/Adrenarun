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

    public int fireRate
    {
        get
        {
            return 1;
        }
    }

    public int damage
    {
        get
        {
            return 1;
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
