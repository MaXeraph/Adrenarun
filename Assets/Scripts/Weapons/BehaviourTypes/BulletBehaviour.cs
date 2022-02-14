using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : IHitBehaviour
{
    float _damage;
    public float Damage 
    { 
        get => _damage; 
        set => _damage = value; 
    }
    float _bulletSpeed = 1f;
    public float BulletSpeed {
        get => _bulletSpeed;
        set => _bulletSpeed = value;
    }

    public void startBehaviour(Vector3 position, Vector3 direction, int damage, float bulletSpeed)
    {
        BulletMono.create(position, direction, damage, bulletSpeed); 
    }

    public void onHit()
    {
        Debug.Log("Peashooter Hit something");
    }
}
