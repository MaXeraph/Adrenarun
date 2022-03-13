using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/**
 * Pie shaped melee attack.
 */
public class SweepAttackBehaviour : AbstractAttackBehaviour
{
	// Distance to the tip of the attack, radius of the sphere
	private float attackSize;

	private Vector3 halfExtents;
	// private float boxHeight = 0.5f;
	public SweepAttackBehaviour(EntityType owner, float damage = 50f, float size = 1f) 
        : base(owner, damage)
	{
		attackSize = size;
		float boxSideLen = Mathf.Sqrt(2 * size * size);
		halfExtents = new Vector3(boxSideLen/2, boxSideLen/4, boxSideLen/2);
	}

    public override void initiateAttack(Vector3 position, Vector3 direction)
    {
	    direction = direction.normalized;
	    Collider[] sphereCollisions = Physics.OverlapSphere(position, attackSize);
	    Collider[] boxCollisions = Physics.OverlapBox(position + direction*attackSize, halfExtents, Quaternion.Euler(0, 45, 0));
	    List<Collider> collisions = new List<Collider>();
	    foreach (Collider c in sphereCollisions)
	    {
		    if (boxCollisions.Contains(c)) collisions.Add(c);
	    }
	    foreach (Collider c in collisions)
	    {
		    Stats statsComponent = c.gameObject.GetComponent<Stats>();
		    if (statsComponent && statsComponent.owner != _owner)
		    {
			    statsComponent.currentHealth -= _damage;
		    }
	    }
    }
}
