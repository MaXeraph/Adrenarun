using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IHitBehaviour
{
    protected EntityType _owner;
    public abstract void startBehaviour(Vector3 position, Vector3 direction);
}
