using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class PowerUpProcessor
{
    private static AbstractPowerUp apu;

    public static void applyPowerUp(PowerUpType powerUp, GameObject target){
        switch (powerUp){
            case PowerUpType.DAMAGE:
                apu = new DamagePowerUp();
                break;
            case PowerUpType.FIRERATE:
                apu = new FireRatePowerUp();
                break;
            case PowerUpType.RELOADSPD:
                apu = new ReloadSpdPowerUp();
                break;
            case PowerUpType.CLIPSIZE:
                apu = new ClipSizePowerUp();
                break;
            case PowerUpType.ADRENALIN:
                apu = new AdrenalinPowerUp();
                break;
            default:
                break;
        }
        apu.applyPowerUp(target);
    }
}
