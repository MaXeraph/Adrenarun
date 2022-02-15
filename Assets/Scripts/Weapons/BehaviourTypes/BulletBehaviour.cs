using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : IHitBehaviour
{
    public void startBehaviour(Vector3 position, Vector3 direction, int damage, float bulletSpeed)
    {
        BulletMono.create(position, direction, damage, bulletSpeed); 
    }
}
