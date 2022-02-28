using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class AbstractFiringPowerUp
{
    public int sortOrder;
    public abstract List<(Vector3, float)> applyPowerUp(List<(Vector3, float)> direction);
}

public class ShotgunFiringPowerUp : AbstractFiringPowerUp
{
    // fire in a cone of 2 additional bullets per powerup obtained.
    // balancing wise i think this is broken but alpha
    // maybe limit to 2 powerups
    public ShotgunFiringPowerUp(){
        sortOrder = 0;
    }

    public override List<(Vector3, float)> applyPowerUp(List<(Vector3, float)> direction)
    {
        List<(Vector3, float)> shotDirections = new List<(Vector3, float)>();

        int num_directions = direction.Count;

        // add new leftmost shot
        shotDirections.Add((Quaternion.AngleAxis(-15, Vector3.up) * direction[0].Item1, direction[0].Item2));

        // add current shots in direction
        for (int i = 0; i < num_directions; i++)
        {
            shotDirections.Add(direction[i]);
        }

        // add new rightmost shot
        shotDirections.Add((Quaternion.AngleAxis(15, Vector3.up) * direction[num_directions - 1].Item1, direction[num_directions - 1].Item2));

        return shotDirections;
    }

}

public class RepeaterFiringPowerUp : AbstractFiringPowerUp
{
    public RepeaterFiringPowerUp(){
        sortOrder = 1;
    }
    // fire an additional bullet in the same direction.
    public override List<(Vector3, float)> applyPowerUp(List<(Vector3, float)> direction)
    {
        List<(Vector3, float)> shotDirections = new List<(Vector3, float)>();
        
        for (int i = 0; i < direction.Count; i++)
        {
            shotDirections.Add(direction[i]);
            shotDirections.Add((direction[i].Item1, direction[i].Item2 + 0.2f));
        }

        return shotDirections;
    }

}

public class AutomaticFiringPowerUp : AbstractFiringPowerUp
{
    public override List<(Vector3, float)> applyPowerUp(List<(Vector3, float)> direction)
    {
        return new List<(Vector3, float)>();
    }
    
}

public class ChargeShotFiringPowerUp : AbstractFiringPowerUp
{
    public override List<(Vector3, float)> applyPowerUp(List<(Vector3, float)> direction)
    {
        return new List<(Vector3, float)>();
    }
    
}
