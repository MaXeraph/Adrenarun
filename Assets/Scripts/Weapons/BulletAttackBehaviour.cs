using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Bullet-type attack. Spawns a bullet to determine hits.
 */
public class BulletAttackBehaviour : AbstractAttackBehaviour
{
    public float _bulletSpeed;
    public Dictionary<string, int> _hitTypeModifiers;
    public Dictionary<string, float> _hitStatsModifiers;

    public BulletAttackBehaviour(EntityType owner, float damage = 10f, float bulletSpeed = 20f)
        : base(owner, damage)
    {
        _bulletSpeed = bulletSpeed;
        _hitTypeModifiers = new Dictionary<string, int>()
        {
            { "exploding", 0 },
            { "pierceThrough", 0 }
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
            if (statsComponent.owner != _owner)
            {
                statsComponent.currentHealth -= _damage;
                BulletMono.Destroy(bm.gameObject);
                if (statsComponent.owner == EntityType.ENEMY)
                {
                    AudioManager.PlayImpactAudio();
                    UIManager.DamageText(bm.gameObject.transform.position + bm.gameObject.transform.up * 0.15f, -_damage);
                }
            }
        }
        else if (target.GetComponent<BulletMono>() == null) // if not another bullet...
        {
            BulletMono.Destroy(bm.gameObject);
        }
    }
}
