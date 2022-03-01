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

    public BulletAttackBehaviour(EntityType owner, float damage = 10f, float bulletSpeed = 10f) 
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
        BulletMono.create(this, position, direction);
    }

    public virtual void onHit(BulletMono bm, GameObject target)
    {
        Stats statsComponent = target.GetComponent<Stats>();
        if (statsComponent)
        {
            if (statsComponent.owner != _owner)
            {
                statsComponent.currentHealth -= _damage;
                GameObject.Destroy(bm.gameObject);
                if (statsComponent.owner == EntityType.ENEMY) {
                    AudioManager.PlayImpactAudio();
                }
            }
        }
        else if (target.GetComponent<BulletMono>() == null) // if not another bullet...
        {
            GameObject.Destroy(bm.gameObject);
            if (Owner.ToString() == "PLAYER") {
                AudioManager.PlayImpactAudio();
            }
        }
    }
}
