using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttackBehaviour : AbstractAttackBehaviour
{
    public Dictionary<string, int> _hitTypeModifiers;
    public Dictionary<string, float> _hitStatsModifiers;

    public BulletAttackBehaviour(EntityType owner)
    {
        _owner = owner;
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
    public BulletAttackBehaviour(Dictionary<string, int> typeModifiers, Dictionary<string, float> statsModifiers)
    {
        _hitTypeModifiers = typeModifiers;
        _hitStatsModifiers = statsModifiers;
    }
    public override void initiateAttack(Vector3 position, Vector3 direction)
    {
        BulletMono.create(position, direction, _hitStatsModifiers, _owner);
    }
}
