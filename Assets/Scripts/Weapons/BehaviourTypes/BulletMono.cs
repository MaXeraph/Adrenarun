using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMono : MonoBehaviour
{
    float _damage = 10;

    float _bulletSpeed = 1f;

    float _bulletSpawnTime;

    public static void create(Vector3 position, Vector3 direction, Dictionary<string, float> statsModifiers)
    {
        GameObject newObject = Instantiate(Resources.Load("Bullet")) as GameObject;
        newObject.transform.position = position;
        newObject.transform.forward = direction;

        BulletMono bulletComponent = newObject.GetComponent<BulletMono>();
        bulletComponent._damage += statsModifiers["damage"];
        bulletComponent._bulletSpeed += statsModifiers["bulletSpeed"];
    }

    void Update()
    {
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime; 
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider c){
        switch (c.gameObject.tag)
        {
            case "Enemy":
                OnEnemyHit(c);
                break;
            case "Platform":
                Destroy(gameObject);
                break;
            case "Player":
                Debug.Log("Bullet hit player - Not self-destructing");
                break;
            default:
                break;
        }
    }

    void OnEnemyHit(Collider c)
    {
        Debug.Log("Bullet hit enemy dealing " + _damage + " damage");
        Destroy(gameObject);
        Destroy(c.gameObject);
    }
}
