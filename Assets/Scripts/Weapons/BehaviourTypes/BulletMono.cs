using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour for Bullet game objects
public class BulletMono : MonoBehaviour
{
    private BulletAttackBehaviour _attackBehaviour;

    public static GameObject Create(BulletAttackBehaviour attackBehaviour, Vector3 position, Vector3 direction)
    {
        GameObject newBullet = ObjectPool.Create("Bullet");

        newBullet.GetComponent<BulletMono>()._attackBehaviour = attackBehaviour;

        newBullet.transform.position = position;
        newBullet.transform.forward = direction;
        if (attackBehaviour.Owner == EntityType.ENEMY) newBullet.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);

        return newBullet;
    }

    public static void Destroy(GameObject bullet)
    {
        // Reset to default values
        bullet.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        bullet.GetComponent<TrailRenderer>().Clear();
        ObjectPool.Destroy("Bullet", bullet);
    }

    void Update()
    {
        transform.position += transform.forward * _attackBehaviour._bulletSpeed * Time.deltaTime * SpeedManager.bulletSpeedScaling;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Detector") { if (_attackBehaviour.Owner == EntityType.ENEMY) { CrosshairUI.addIndicator(c.transform.position); } return; }

        _attackBehaviour.onHit(this, c.gameObject);
    }
}
