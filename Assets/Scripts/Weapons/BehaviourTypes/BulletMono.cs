using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be attached to Bullet Objects
public class BulletMono : MonoBehaviour
{
    float _damage = 10f;

    float _bulletSpeed = 10f;

    float _bulletSpawnTime;

    EntityType _owner;

    public static void create(Vector3 position, Vector3 direction, Dictionary<string, float> statsModifiers, EntityType owner)
    {
        GameObject newObject = Instantiate(Resources.Load("Bullet")) as GameObject;
        newObject.transform.position = position;
        newObject.transform.forward = direction;

        BulletMono bulletComponent = newObject.GetComponent<BulletMono>();
        bulletComponent._damage += statsModifiers["damage"];
        bulletComponent._bulletSpeed += statsModifiers["bulletSpeed"];
        bulletComponent._owner = owner;
    }

    void Update()
    {
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime; 
    }

    void OnTriggerEnter(Collider c){
        Stats statsComponent = c.gameObject.GetComponent<Stats>();
        if (statsComponent){
            if (statsComponent.owner != _owner){
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
