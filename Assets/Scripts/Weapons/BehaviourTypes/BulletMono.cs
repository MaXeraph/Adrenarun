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

    float _bulletSpawnTime;

    public static void create(Vector3 position, Vector3 direction, Dictionary<string, float> statsModifiers)
    {
        GameObject newObject = Instantiate(Resources.Load("Bullet")) as GameObject;
        newObject.transform.position = position;
        newObject.transform.forward = direction;

        BulletMono bulletComponent = newObject.GetComponent<BulletMono>();
        bulletComponent.Damage = statsModifiers["damage"];
        bulletComponent.BulletSpeed = statsModifiers["bulletSpeed"];
    }

    void Start()
    {
        _bulletSpawnTime = Time.time;
    }

    void Update()
    {
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime; 
        if (Time.time - _bulletSpawnTime > 10f)
        {
            Destroy(gameObject);
        }
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
