using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : IHitBehaviour
{
    public Dictionary<string, int> _hitTypeModifiers;
    public Dictionary<string, float> _hitStatsModifiers;

    public BulletBehaviour()
    {
        _hitTypeModifiers = new Dictionary<string, int>()
    {
        { "exploding", 0 },
        { "pierceThrough", 0 }
    };
        _hitStatsModifiers = new Dictionary<string, float>()
    {
        { "damage", 10f },
        { "bulletSpeed", 20f }
    };
    }
    public BulletBehaviour(Dictionary<string, int> typeModifiers, Dictionary<string, float> statsModifiers)
    {
        _hitTypeModifiers = typeModifiers;
        _hitStatsModifiers = statsModifiers;
    }
    public void startBehaviour(Vector3 position, Vector3 direction)
    {
        BulletMono.create(position, direction, _hitStatsModifiers); 
    }
}
