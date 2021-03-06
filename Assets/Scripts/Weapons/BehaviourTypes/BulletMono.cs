using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour for Bullet game objects
public class BulletMono : MonoBehaviour
{
	public BulletAttackBehaviour _attackBehaviour;
	public HashSet<GameObject> hitObjects;

	public static GameObject Create(BulletAttackBehaviour attackBehaviour, Vector3 position, Vector3 direction, string bulletName)
	{
		GameObject newBullet = ObjectPool.Create(bulletName);

		newBullet.GetComponent<BulletMono>()._attackBehaviour = attackBehaviour;
		newBullet.transform.position = position;
		newBullet.transform.forward = direction;
		// if (attackBehaviour.Owner == EntityType.ENEMY) newBullet.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
		newBullet.GetComponent<BulletMono>().hitObjects = new HashSet<GameObject>();

		return newBullet;
	}

	public static void Destroy(GameObject bullet, string bulletName)
	{
		// Reset to default values
		bullet.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
		bullet.GetComponent<TrailRenderer>().Clear();
		bullet.GetComponent<BulletMono>().hitObjects.Clear();
		ObjectPool.Destroy(bulletName, bullet);
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
