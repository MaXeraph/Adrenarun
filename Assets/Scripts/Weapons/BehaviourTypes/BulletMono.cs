using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMono : MonoBehaviour
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

    public static void create(Vector3 position, Vector3 direction, int damage, float bulletSpeed)
    {
        GameObject newObject = Instantiate(Resources.Load("Bullet")) as GameObject;
        newObject.transform.position = position;
        newObject.transform.forward = direction;

        BulletMono bulletComponent = newObject.GetComponent<BulletMono>();
        bulletComponent.Damage = damage;
        bulletComponent.BulletSpeed = bulletSpeed;
    }

    void Update()
    {
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime * SpeedManager.bulletSpeedScaling * 2; 
    }

    public void onHit()
    {
        Debug.Log("Peashooter Hit something");
    }
}
