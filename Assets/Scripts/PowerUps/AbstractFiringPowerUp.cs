using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFiringPowerUp : AbstractPowerUp
{
    public abstract Vector3[] applyPowerUp(Vector3[] direction);

}

public class ShotgunFiringPowerUp : AbstractFiringPowerUp
{
    public override Vector3[] applyPowerUp(Vector3[] direction)
    {
        return new Vector3[0];
    }

}

public class RepeaterFiringPowerUp : AbstractFiringPowerUp
{
    public override Vector3[] applyPowerUp(Vector3[] direction)
    {
        return new Vector3[0];
    }
}

public class AutomaticFiringPowerUp : AbstractFiringPowerUp
{
    public override Vector3[] applyPowerUp(Vector3[] direction)
    {
        return new Vector3[0];
    }
}

public class ChargeShotFiringPowerUp : AbstractFiringPowerUp
{
    public override Vector3[] applyPowerUp(Vector3[] direction)
    {
        return new Vector3[0];
    }
}
