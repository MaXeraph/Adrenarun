using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPowerUp
{
    public float modifier;
    public abstract Vector3[] applyPowerUp(Vector3[] direction);

}
