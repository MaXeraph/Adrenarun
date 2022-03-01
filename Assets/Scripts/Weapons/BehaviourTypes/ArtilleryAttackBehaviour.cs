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
        if (target.tag == "Player" || target.GetComponent<BulletMono>() == null) // if not another bullet...
        {
            Vector3 position = bm.gameObject.GetComponent<Transform>().position;
            RaycastHit hitInfo;
            Physics.Raycast(position, new Vector3(0, -1, 0), out hitInfo);
            GameObject.Destroy(bm.gameObject);
            GameObject thermitePool = GameObject.Instantiate(Resources.Load("ThermitePool")) as GameObject;
            // Spawn the pool on the ground.
            thermitePool.GetComponent<Transform>().position = hitInfo.point;
            thermitePool.GetComponent<ThermitePoolMono>().Initialize(this);
        }
    }
}