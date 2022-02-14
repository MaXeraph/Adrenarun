using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour, IHitBehaviour
{
    public float damage { get { return 1f;} }

    public void startBehaviour()
    {
        Debug.Log("BulletPeashooter is created");
    }

    public void onHit()
    {
        Debug.Log("Peashooter Hit something");
    }
}
