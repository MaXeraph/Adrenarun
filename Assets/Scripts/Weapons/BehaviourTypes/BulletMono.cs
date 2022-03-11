using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour for Bullet game objects
public class BulletMono : MonoBehaviour
{
    private BulletAttackBehaviour _attackBehaviour;

    public static GameObject create(BulletAttackBehaviour attackBehaviour, Vector3 position, Vector3 direction)
    {
        GameObject newBullet = Instantiate(Resources.Load("Bullet")) as GameObject;

        newBullet.GetComponent<BulletMono>()._attackBehaviour = attackBehaviour;
        
        newBullet.transform.position = position;
        newBullet.transform.forward = direction;
        if (attackBehaviour.Owner == EntityType.ENEMY)newBullet.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);

        return newBullet;
    }

    void Update()
    {
        transform.position += transform.forward * _attackBehaviour._bulletSpeed * Time.deltaTime * SpeedManager.bulletSpeedScaling;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Detector") { if (_attackBehaviour.Owner == EntityType.ENEMY) { CrosshairUI.addIndicator(c.transform.position); } return; }

        _attackBehaviour.onHit(this, c.gameObject);
        if (c.gameObject.tag == "Enemy" && _attackBehaviour.Owner != EntityType.ENEMY) UIManager.DamageText(transform.position + transform.up * 0.15f, -_attackBehaviour._damage);
    }
}
