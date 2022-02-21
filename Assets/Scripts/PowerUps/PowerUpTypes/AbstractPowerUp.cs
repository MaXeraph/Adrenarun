using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPowerUp
{
    public AbstractPowerUp(){}

    public abstract PowerUpType powerUpType {get; }

    public abstract void applyPowerUp(GameObject _player);
}

public class DamagePowerUp : AbstractPowerUp
{
    public DamagePowerUp()
    {

    }

    public override PowerUpType powerUpType => PowerUpType.DAMAGE;
    
    public override void applyPowerUp(GameObject _player)
    {
        Debug.Log("Damage Power Up");
    }
}

public class FireRatePowerUp : AbstractPowerUp
{
    public FireRatePowerUp()
    {

    }

    public override PowerUpType powerUpType => PowerUpType.FIRERATE;
    
    public override void applyPowerUp(GameObject _player)
    {
        Debug.Log("Fire Rate Power Up");

    }
}

public class ReloadSpdPowerUp : AbstractPowerUp
{
    public ReloadSpdPowerUp()
    {

    }

    public override PowerUpType powerUpType => PowerUpType.RELOADSPD;
    
    public override void applyPowerUp(GameObject _player)
    {
        Debug.Log("Reload Spd Power Up");

    }
}

public class ClipSizePowerUp : AbstractPowerUp
{
    public ClipSizePowerUp()
    {

    }

    public override PowerUpType powerUpType => PowerUpType.CLIPSIZE;
    
    public override void applyPowerUp(GameObject _player)
    {
        Debug.Log("Clip Size Power Up");

    }
}

public class AdrenalinPowerUp : AbstractPowerUp
{
    public AdrenalinPowerUp()
    {

    }

    public override PowerUpType powerUpType => PowerUpType.ADRENALIN;
    
    public override void applyPowerUp(GameObject _player)
    {

        Debug.Log("Adrenalin Power Up");
    }
}
