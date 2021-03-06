using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
	public static Dictionary<PowerUpType, int> TransitionPowerUpDictionary = newTransitionPowerUpDictionary();

	public static Dictionary<PowerUpType, int> newTransitionPowerUpDictionary(){
		return new Dictionary<PowerUpType, int>()
		{
			{PowerUpType.DAMAGE, 0},
			{PowerUpType.FIRERATE, 0 },
			{PowerUpType.RELOADSPD, 0 },
			{PowerUpType.CLIPSIZE, 0 },
			{PowerUpType.ADRENALIN, 0 },
			{PowerUpType.SHOTGUN, 0 },
			{PowerUpType.REPEATER, 0 },
			{PowerUpType.PIERCING, 0 },
			{PowerUpType.EXPLODING, 0 },
			{PowerUpType.DASHCD, 0}
		};
	}

	public static Dictionary<PowerUpType, AbstractStatPowerUp> StatPowerUpDictionary = new Dictionary<PowerUpType, AbstractStatPowerUp>
	{{PowerUpType.DAMAGE, new DamagePowerUp()},
	{PowerUpType.FIRERATE, new FireRatePowerUp()},
	{PowerUpType.RELOADSPD, new ReloadSpeedPowerUp()},
	{PowerUpType.CLIPSIZE, new ClipSizePowerUp()},
	{PowerUpType.ADRENALIN, new AdrenalinPowerUp()},
	{PowerUpType.DASHCD, new DashCDPowerUp()}
	};

	public static Dictionary<PowerUpType, AbstractFiringPowerUp> FiringPowerUpDictionary = new Dictionary<PowerUpType, AbstractFiringPowerUp>
	{{PowerUpType.SHOTGUN, new ShotgunFiringPowerUp()},
	{PowerUpType.REPEATER, new RepeaterFiringPowerUp()}
	};

	public static Dictionary<PowerUpType, AbstractBulletPowerUp> BulletPowerUpDictionary = new Dictionary<PowerUpType, AbstractBulletPowerUp>
	{{PowerUpType.PIERCING, new PiercingBulletPowerUp()},
	{PowerUpType.EXPLODING, new ExplodingBulletPowerUp()}
	};

	public static Dictionary<PowerUpType, PowerUpClass> PowerUpClassDictionary = new Dictionary<PowerUpType, PowerUpClass>
	{{PowerUpType.DAMAGE, PowerUpClass.STAT},
	{PowerUpType.FIRERATE, PowerUpClass.STAT},
	{PowerUpType.RELOADSPD, PowerUpClass.STAT},
	{PowerUpType.CLIPSIZE, PowerUpClass.STAT},
	{PowerUpType.ADRENALIN, PowerUpClass.STAT},
	{PowerUpType.DASHCD, PowerUpClass.STAT},
	{PowerUpType.SHOTGUN, PowerUpClass.FIRING},
	{PowerUpType.REPEATER, PowerUpClass.FIRING},
	{PowerUpType.PIERCING, PowerUpClass.BULLET},
	{PowerUpType.EXPLODING, PowerUpClass.BULLET}
	};

	public static Dictionary<PowerUpType, string> PowerUpInfoDictionary = new Dictionary<PowerUpType, string>
	{{PowerUpType.NONE, ""},
	{PowerUpType.DAMAGE, "Increase bullet damage"},
	{PowerUpType.FIRERATE, "Increase bullet fire rate"},
	{PowerUpType.RELOADSPD, "Increase gun reload speed"},
	{PowerUpType.CLIPSIZE, "Increase gun clip size"},
	{PowerUpType.ADRENALIN, "Reduces effect of health-based speed scaling"},
	{PowerUpType.SHOTGUN, "Shoot more bullets with increased spread"},
	{PowerUpType.REPEATER, "Shoot an additional bullet with each shot"},
	{PowerUpType.PIERCING, "Pierce through an additional enemy with each bullet"},
	{PowerUpType.EXPLODING, "Bullets explode on contact"},
	{PowerUpType.DASHCD, "Reduce dash cooldown by 20%"}
	};

	public static Dictionary<PowerUpType, Sprite> PowerUpIconDictionary = new Dictionary<PowerUpType, Sprite>
	{{PowerUpType.DAMAGE, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_side_spritesheet_1")[6]},
	{PowerUpType.FIRERATE, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_front_spritesheet_1")[1]},
	{PowerUpType.RELOADSPD, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_front_spritesheet_1")[8]},
	{PowerUpType.CLIPSIZE, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_front_spritesheet_1")[6]},
	{PowerUpType.ADRENALIN, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_front_spritesheet_1")[4]},
	{PowerUpType.DASHCD, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_front_spritesheet_1")[4]},
	{PowerUpType.SHOTGUN, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_side_spritesheet_1")[1]},
	{PowerUpType.REPEATER, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_side_spritesheet_1")[7]},
	{PowerUpType.PIERCING, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_side_spritesheet_1")[7]},
	{PowerUpType.EXPLODING, Resources.LoadAll<Sprite>("Textures/toon_muzzleflash_side_spritesheet_1")[4]}
	};

	public static Vector3 DirectTargeting(Transform from, Transform to)
	{
		Vector3 direction = (to.position - from.position).normalized;
		return direction;
	}

	public static Vector3 ForwardTargeting(Transform from, Transform to)
	{
		return from.forward;
	}

	public static Func<Transform, Transform, Vector3> CreatePredictiveTargeting(Transform target, float bSpeed)
	{
		float bulletSpeed = bSpeed;
		Vector3 prevPosition = new Vector3(target.position.x, target.position.y, target.position.z);
		float prevTime = Time.time;
		Func<Transform, Transform, Vector3> predictive = delegate (Transform from, Transform to)
		{
			float modifiedBulletSpeed = bulletSpeed * SpeedManager.bulletSpeedScaling;
			Vector3 enemyDir = (to.position - prevPosition).normalized;
			Vector3 plane = MathModule.determinePlaneNormal(from.position, to.position, to.position + enemyDir);
			float now = Time.time;
			float enemySpeed = (to.position - prevPosition).magnitude / (now - prevTime);
			// Use law of sines
			float angleBeta = Vector3.Angle(from.position - to.position, enemyDir);
			// Angles towards or away cause the plane defined to not have a major y-component... in these cases, just shoot at them
			if (angleBeta >= 170f || angleBeta <= 10f) return (to.position - from.position).normalized;
			float angleAlpha = Mathf.Abs(Mathf.Rad2Deg * Mathf.Asin(enemySpeed * Mathf.Sin(angleBeta * Mathf.Deg2Rad) / modifiedBulletSpeed));
			Vector3 firingDirection = Quaternion.AngleAxis(angleAlpha, plane) * (to.position - from.position);
			prevPosition.x = to.position.x;
			prevPosition.y = to.position.y;
			prevPosition.z = to.position.z;
			prevTime = now;
			return firingDirection.normalized;
		};
		return predictive;
	}

	public static Vector3 GrenadierTargeting(Transform from, Transform to)
	{
		return to.position;
	}

	public static Dictionary<EnemyType, string> enemyPrefabNames = new Dictionary<EnemyType, string>()
	{
		{ EnemyType.TURRET, "Turret" },
		{ EnemyType.GRENADIER, "Grenadier"},
		{ EnemyType.RANGED, "Ranged"},
		{ EnemyType.HEALER, "Healer" },
		{ EnemyType.TANK, "Tank" },
		{ EnemyType.FLYING, "Flying"}
	};

	public static Dictionary<EnemyType, float> enemyBulletSpeeds = new Dictionary<EnemyType, float>() {
		{ EnemyType.TURRET, 20f },
		{ EnemyType.RANGED, 20f },
		{ EnemyType.FLYING, 20f }
	};

	public static float TransitionHealth = 100f;
	public static int TransitionPills = 0;
}

public enum EnemyType
{
	GRENADIER,
	FLYING,
	RANGED,
	TANK,
	HEALER,
	TURRET,
}

public enum EnemyVariantType
{
	NONE,
	SET,
	PREDICTIVE,
	SHIELD,
	AGGRESSOR
}

public enum EntityType
{
	ENEMY,
	PLAYER,
}

public enum PowerUpType
{
	NONE,
	DAMAGE,
	FIRERATE,
	RELOADSPD,
	CLIPSIZE,
	ADRENALIN,
	SHOTGUN,
	REPEATER,
	PIERCING,
	EXPLODING,
	DASHCD
}

public enum PowerUpClass
{
	STAT,
	FIRING,
	BULLET
}

public enum PowerUpTier{
	HIGH,
	MID,
	LOW
}
