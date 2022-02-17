using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be attached to Bullet Objects
public class BulletMono : MonoBehaviour
{
    float _damage = 10f;

    float _bulletSpeed = 10f;

    float _bulletSpawnTime;

    bool _playerBullet;

    public static void create(Vector3 position, Vector3 direction, Dictionary<string, float> statsModifiers, bool playerBullet = true)
    {
        GameObject newObject = Instantiate(Resources.Load("Bullet")) as GameObject;
        newObject.transform.position = position;
        newObject.transform.forward = direction;

        BulletMono bulletComponent = newObject.GetComponent<BulletMono>();
        bulletComponent._damage += statsModifiers["damage"];
        bulletComponent._bulletSpeed += statsModifiers["bulletSpeed"];
        bulletComponent._playerBullet = playerBullet;
    }

    void Update()
    {
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime * SpeedManager.bulletSpeedScaling;
    }

    void OnTriggerEnter(Collider c){
        Stats statsComponent = c.gameObject.GetComponent<Stats>();
        if (statsComponent){
            if (statsComponent.isPlayer != _playerBullet){
                statsComponent.takeDamage(_damage);
                Destroy(gameObject); // bullet
            }
        } else {
            Destroy(gameObject);
        }
    }

    void OnEnemyHit(Collider c)
    {
        Debug.Log("Bullet hit enemy dealing " + _damage + " damage");
        Destroy(gameObject);
        Destroy(c.gameObject);
    }
}
