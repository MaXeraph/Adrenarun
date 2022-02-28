using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractStatPowerUp
{
    protected float _modifier;

    public abstract void applyPowerUp(Weapon weapon);
}

public class DamagePowerUp : AbstractStatPowerUp
{
    public DamagePowerUp(){
        _modifier = 5f;
    }

    public override void applyPowerUp(Weapon weapon){
        weapon._attackBehaviour._damage += _modifier;
    }
}

public class FireRatePowerUp : AbstractStatPowerUp
{
    public FireRatePowerUp(){
        _modifier = -0.04f;
    }

    public override void applyPowerUp(Weapon weapon){
        weapon._fireRate += _modifier;
    }
}

public class ReloadSpeedPowerUp : AbstractStatPowerUp
{
    public ReloadSpeedPowerUp(){
        _modifier = 1f;
    }

    public override void applyPowerUp(Weapon weapon){
        weapon._reloadSpeed += _modifier;
    }
}

public class ClipSizePowerUp : AbstractStatPowerUp
{
    public ClipSizePowerUp(){
        _modifier = 2f;
    }

    public override void applyPowerUp(Weapon weapon){
        weapon._magazineSize += (int)_modifier;
    }
}

public class AdrenalinPowerUp : AbstractStatPowerUp
{
    public AdrenalinPowerUp(){
        _modifier = 0.15f;
    }

    public override void applyPowerUp(Weapon weapon){
        SpeedManager.adrenalinModifier += _modifier;
    }
}
