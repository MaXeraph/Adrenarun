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
	private int pierceCount = 0;

	public BulletAttackBehaviour(EntityType owner, float damage = 10f, float bulletSpeed = 20f)
		: base(owner, damage)
	{
		_bulletSpeed = bulletSpeed;
		_hitTypeModifiers = new Dictionary<string, int>()
		{
			{ "explode", 0 },
			{ "pierce", 0 }
		};
		_hitStatsModifiers = new Dictionary<string, float>()
		{
			{ "damage", 0 },
			{ "bulletSpeed", 0 }
		};
	}

	public override void initiateAttack(Vector3 position, Vector3 direction)
	{
		BulletMono.Create(this, position, direction);
	}

	public virtual void onHit(BulletMono bm, GameObject target)
	{
		Stats statsComponent = target.GetComponent<Stats>();
		if (statsComponent)
		{
			if (statsComponent.owner != _owner && !bm.piercedObjects.Contains(target))
			{
				statsComponent.currentHealth -= _damage;
				bm.piercedObjects.Add(target);
				if (statsComponent.owner == EntityType.ENEMY)
				{
					AudioManager.PlayImpactAudio();
					UIManager.DamageText(bm.gameObject.transform.position + bm.gameObject.transform.up * 0.15f, -_damage);
				}
				if (bm.piercedObjects.Count > _hitTypeModifiers["pierce"])
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

	// private void applyOnHitEffects(BulletMono bm, GameObject target)
	// {
	// 	// destroy if pierce count is met
	// 	pierceCount += 1;
	// 	Debug.Log(pierceCount);
	// 	Debug.Log(_hitTypeModifiers["pierce"]);
	// 	if (pierceCount > _hitTypeModifiers["pierce"])
	// 	{
	// 		BulletMono.Destroy(bm.gameObject);
	// 		pierceCount = 0;
	// 	}

	// 	// explode with radius dependent on amount of explode powerups
	// }
}
