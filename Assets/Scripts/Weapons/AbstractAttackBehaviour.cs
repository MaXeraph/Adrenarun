using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAttackBehaviour
{
    protected EntityType _owner;
    public abstract void initiateAttack(Vector3 position, Vector3 direction);
}
