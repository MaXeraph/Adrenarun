using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Base class for AttackBehaviours, defines how the attack is determined to hit and the effects of the attack.
 */
public abstract class AbstractAttackBehaviour
{
    protected EntityType _owner;
    public float _damage;
    
    /**
     * Called to initiate an attack from position in direction.
     * Should call onHit().
     */
    public abstract void initiateAttack(Vector3 position, Vector3 direction);
    
    /**
     * Called on the game object that is hit by this attack.
     * Returns true if the attack should continue, false otherwise.
     */
    public abstract bool onHit(GameObject target);

    protected AbstractAttackBehaviour(EntityType owner, float damage)
    {
        _owner = owner;
        _damage = damage;
    }
}
