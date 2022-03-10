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

	public static Dictionary<PowerUpType, string> PowerUpInfoDictionary = new Dictionary<PowerUpType, string>
	{{PowerUpType.NONE, ""},
	{PowerUpType.DAMAGE, "Increase bullet damage"},
	{PowerUpType.FIRERATE, "Increase bullet fire rate"},
	{PowerUpType.RELOADSPD, "Increase gun reload speed"},
	{PowerUpType.CLIPSIZE, "Increase gun clip size"},
	{PowerUpType.ADRENALIN, "Reduces effect of health-based speed scaling"},
	{PowerUpType.SHOTGUN, "Shoot more bullets with increased spread"},
	{PowerUpType.REPEATER, "Shoot an additional bullet with each shot"}
	};

	public static Dictionary<PowerUpType, Sprite> PowerUpIconDictionary = new Dictionary<PowerUpType, Sprite>
	{{PowerUpType.DAMAGE, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_side_spritesheet_1")[6]},
	{PowerUpType.FIRERATE, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_front_spritesheet_1")[1]},
	{PowerUpType.RELOADSPD, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_front_spritesheet_1")[8]},
	{PowerUpType.CLIPSIZE, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_front_spritesheet_1")[6]},
	{PowerUpType.ADRENALIN, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_front_spritesheet_1")[4]},
	{PowerUpType.SHOTGUN, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_side_spritesheet_1")[1]},
	{PowerUpType.REPEATER, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_side_spritesheet_1")[7]}
	};

	public static Vector3 DirectTargeting(Transform from, Transform to)
    {
        Vector3 direction = (to.position - from.position).normalized;
        return direction;
    }

	public static Vector3 GrenadierTargeting(Transform from, Transform to) {
		return to.position;
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
    HEALER,
	SET
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
