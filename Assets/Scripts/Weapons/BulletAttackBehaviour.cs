using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Bullet-type attack. Spawns a bullet to determine hits.
 */
public class BulletAttackBehaviour : AbstractAttackBehaviour
{
	public float _bulletSpeed;
	public Dictionary<string, float> _hitStatsModifiers;

	public BulletAttackBehaviour(EntityType owner, float damage = 10f, float bulletSpeed = 20f)
		: base(owner, damage)
	{
		_bulletSpeed = bulletSpeed;
		_hitTypeModifiers = new Dictionary<string, int>()
		{
			{ "exploding", 0 },
			{ "piercing", 0 }
		};
		_hitStatsModifiers = new Dictionary<string, float>()
		{
			{ "damage", 0 },
			{ "bulletSpeed", 0 }
		};
	}

	public override void initiateAttack(Vector3 position, Vector3 direction, EntityType entityType)
	{
		string bulletName;
		if (entityType == EntityType.PLAYER) bulletName = "Bullet";
		else bulletName = "EnemyBullet";
		BulletMono.Create(this, position, direction, bulletName);
	}

	public virtual void onHit(BulletMono bm, GameObject target)
	{
		Stats statsComponent = target.GetComponent<Stats>();
		if (statsComponent)
		{
			if (statsComponent.owner != _owner && !bm.hitObjects.Contains(target))
			{
				statsComponent.currentHealth -= _damage;
				bm.hitObjects.Add(target);
				applyOnHitEffects(bm, target);
				if (statsComponent.owner == EntityType.ENEMY)
				{
					AudioManager.PlayImpactAudio();
					UIManager.DamageText(bm.gameObject.transform.position + bm.gameObject.transform.up * 0.15f, -_damage);
				}
				if (bm.hitObjects.Count > _hitTypeModifiers["piercing"])
				{
					BulletMono.Destroy(bm.gameObject);
				}
			}
		}
		else if (target.tag != "Projectile" && target.tag != "Detector" && target.tag != "Portal")
		{
			BulletMono.Destroy(bm.gameObject);
		}
	}

	private void applyOnHitEffects(BulletMono bm, GameObject target)
	{
		// explode with radius dependent on amount of explode powerups
		HashSet<GameObject> hitObject = new HashSet<GameObject>();
		hitObject.Add(target);
		Vector3 position = bm.gameObject.GetComponent<Transform>().position;
		Collider[] explosionHits = Physics.OverlapSphere(position, 3f * _hitTypeModifiers["exploding"]); // 3f = slightly smaller than ThermitePool
		for (int i = 0; i < explosionHits.Length; i++)
		{
			Collider collider = explosionHits[i];
			Stats statsComponent = collider.gameObject.GetComponent<Stats>();
			if (statsComponent && !hitObject.Contains(collider.gameObject) && statsComponent.owner != _owner)
			{
				statsComponent.currentHealth -= _damage / 2; // Deal half damage on explosion.
				hitObject.Add(collider.gameObject);
				AudioManager.PlayImpactAudio();
				UIManager.DamageText(collider.gameObject.transform.position + collider.gameObject.transform.up * 0.15f, -_damage / 2);
			}
		}
	}
}
