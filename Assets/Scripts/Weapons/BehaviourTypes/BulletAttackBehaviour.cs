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

    public override bool onHit(GameObject target)
    {
        Stats statsComponent = target.GetComponent<Stats>();
        if (statsComponent)
        {
            if (statsComponent.owner != _owner)
            {
                statsComponent.takeDamage(_damage);
                return false; // destroy the bullet
            }
        }
        else if (target.GetComponent<BulletMono>() != null) return true; // do not collide with other bullets
        else return false;

        return true;
    }
}
