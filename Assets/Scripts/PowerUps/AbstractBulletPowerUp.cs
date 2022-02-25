using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBulletPowerUp : AbstractPowerUp
{
}

public class TargetingBulletPowerUp : AbstractBulletPowerUp
{
    public override Vector3[] applyPowerUp(Vector3[] direction)
    {
        return new Vector3[0];
    }
    
}

public class OnHitBulletPowerUp : AbstractBulletPowerUp
{
    public override Vector3[] applyPowerUp(Vector3[] direction)
    {
        return new Vector3[0];
    }
    
}
