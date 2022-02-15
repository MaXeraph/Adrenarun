using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : IHitBehaviour
{
    public void startBehaviour(Vector3 position, Vector3 direction, Dictionary<string, float> statsModifiers)
    {
        BulletMono.create(position, direction, statsModifiers); 
    }
}
