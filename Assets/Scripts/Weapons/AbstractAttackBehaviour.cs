using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Base class for AttackBehaviours, defines how the attack is determined to hit and the effects of the attack.
 */
public abstract class AbstractAttackBehaviour
{
    public EntityType Owner
    {
        get { return _owner; }
        private set { _owner = value; }
    }
    protected EntityType _owner;
    public float _damage;
    
    /**
     * Called to initiate an attack from position in direction.
     */
    public abstract void initiateAttack(Vector3 position, Vector3 direction);

    protected AbstractAttackBehaviour(EntityType owner, float damage)
    {
        _owner = owner;
        _damage = damage;
    }
}
