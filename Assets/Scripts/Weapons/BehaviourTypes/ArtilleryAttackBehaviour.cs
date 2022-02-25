using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Attack for the Grenadier.
 */
public class ArtilleryAttackBehaviour : BulletAttackBehaviour
{
    public int thermiteDurability;
    public float thermiteDamageCooldown;
    

    public ArtilleryAttackBehaviour(EntityType owner, float damage = 10f, float bulletSpeed = 10f, int durability = 10, float damageCooldown = 1f)
        : base(owner, damage, bulletSpeed)
    {
        thermiteDurability = durability;
        thermiteDamageCooldown = damageCooldown;
    }

    public override void onHit(BulletMono bm, GameObject target)
    {
        if (target.GetComponent<Stats>() == null && target.GetComponent<BulletMono>() == null) // if not another bullet...
        {
            GameObject.Destroy(bm.gameObject);
            GameObject thermitePool = GameObject.Instantiate(Resources.Load("ThermitePool")) as GameObject;
            // TODO: project down to surface
            thermitePool.GetComponent<Transform>().position = bm.gameObject.GetComponent<Transform>().position;
            thermitePool.GetComponent<ThermitePoolMono>().Initialize(this);
        }
    }
}