using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractBulletPowerUp : AbstractPowerUp
{
    public override Vector3[] applyPowerUp(Vector3[] direction){
        return new Vector3[0];
    }
    
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
