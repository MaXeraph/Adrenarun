using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour for Bullet game objects
public class BulletMono : MonoBehaviour
{
    private BulletAttackBehaviour _attackBehaviour;

    public static void create(BulletAttackBehaviour attackBehaviour, Vector3 position, Vector3 direction)
    {
        GameObject newBullet = Instantiate(Resources.Load("Bullet")) as GameObject;

        newBullet.GetComponent<BulletMono>()._attackBehaviour = attackBehaviour;
        
        newBullet.transform.position = position;
        newBullet.transform.forward = direction;
    }

    void Update()
    {
        transform.position += transform.forward * _attackBehaviour._bulletSpeed * Time.deltaTime * SpeedManager.bulletSpeedScaling;
    }

    void OnTriggerEnter(Collider c)
    {
        if (!_attackBehaviour.onHit(c.gameObject))
        {
            Destroy(gameObject);
        }
    }
}
