using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
	public static Dictionary<PowerUpType, AbstractStatPowerUp> StatPowerUpDictionary = new Dictionary<PowerUpType, AbstractStatPowerUp>
	{{PowerUpType.DAMAGE, new DamagePowerUp()},
	{PowerUpType.FIRERATE, new FireRatePowerUp()},
	{PowerUpType.RELOADSPD, new ReloadSpeedPowerUp()},
	{PowerUpType.CLIPSIZE, new ClipSizePowerUp()},
	{PowerUpType.ADRENALIN, new AdrenalinPowerUp()}
	};
	
	public static Dictionary<PowerUpType, AbstractFiringPowerUp> FiringPowerUpDictionary = new Dictionary<PowerUpType, AbstractFiringPowerUp>
	{{PowerUpType.SHOTGUN, new ShotgunFiringPowerUp()},
	{PowerUpType.REPEATER, new RepeaterFiringPowerUp()}
	};

	public static Dictionary<PowerUpType, AbstractBulletPowerUp> BulletPowerUpDictionary = new Dictionary<PowerUpType, AbstractBulletPowerUp>
	{
	};

	public static Dictionary<PowerUpType, PowerUpClass> PowerUpClassDictionary = new Dictionary<PowerUpType, PowerUpClass>
	{{PowerUpType.DAMAGE, PowerUpClass.STAT},
	{PowerUpType.FIRERATE, PowerUpClass.STAT},
	{PowerUpType.RELOADSPD, PowerUpClass.STAT},
	{PowerUpType.CLIPSIZE, PowerUpClass.STAT},
	{PowerUpType.ADRENALIN, PowerUpClass.STAT},
	{PowerUpType.SHOTGUN, PowerUpClass.FIRING},
	{PowerUpType.REPEATER, PowerUpClass.FIRING}
	};

    public static Vector3 DirectTargeting(Transform from, Transform to)
    {
        Vector3 direction = (to.position - from.position).normalized;
        return direction;
    }

    public static Dictionary<EnemyType, string> enemyPrefabNames = new Dictionary<EnemyType, string>()
    {
        { EnemyType.TURRET, "Turret" },
        { EnemyType.GRENADIER, "Grenadier"},
        { EnemyType.RANGED, "Ranged"}
    };
}

public enum EnemyType
{
	TURRET,
	GRENADIER,
	RANGED,
}

public enum EnemyVariantType
{
    NONE,
    HEALER
}

public enum EntityType
{
    PLAYER,
    ENEMY
}

public enum PowerUpType{
	NONE,
	DAMAGE,
	FIRERATE,
	RELOADSPD,
	CLIPSIZE,
	ADRENALIN,
	SHOTGUN,
	REPEATER
}

public enum PowerUpClass{
	STAT,
	FIRING,
	BULLET
}
