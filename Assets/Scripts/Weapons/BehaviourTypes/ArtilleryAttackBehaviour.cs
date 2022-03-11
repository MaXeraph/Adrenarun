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
    
    public override void initiateAttack(Vector3 position, Vector3 direction)
    {
        ArtilleryMono.create(this, position, direction);
    }

    public override void onHit(BulletMono bm, GameObject target)
    {
        if (target.tag != "Enemy" && target.tag != "Detector" && (target.tag == "Player" || target.GetComponent<BulletMono>() == null)) // if not another bullet...
        {
            Vector3 position = bm.gameObject.GetComponent<Transform>().position;
            Collider[] explosionHits = Physics.OverlapSphere(position, 4f); // 4f = scale of ThermitePool/2
            for (int i = 0; i < explosionHits.Length; i++)
            {
                Collider collider = explosionHits[i];
                if (collider.gameObject.tag == "Player")
                {
                    Stats statsComponent = collider.gameObject.GetComponent<Stats>();
                    statsComponent.currentHealth -= _damage/2; // Deal half damage on explosion.
                    GameObject.Destroy(bm.gameObject);
                    break;
                }
            }
            
            RaycastHit[] hitInfo = Physics.RaycastAll(position + new Vector3(0, 5, 0), new Vector3(0, -1, 0));
            for (int i = 0; i < hitInfo.Length; i++)
            {
                if (hitInfo[i].collider.gameObject.tag == "Platform")
                {
                    GameObject thermitePool = GameObject.Instantiate(Resources.Load("ThermitePool")) as GameObject;
                    // Spawn the pool on the ground.
                    thermitePool.GetComponent<Transform>().position = hitInfo[i].point;
                    thermitePool.GetComponent<ThermitePoolMono>().Initialize(this);
                    GameObject.Destroy(bm.gameObject);
                    return;
                }
            }
        }
    }
}