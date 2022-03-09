using System;
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

	//For Upgrade Icons
	//public static Dictionary<PowerUpType, Texture> PowerUpIconDictionary = new Dictionary<PowerUpType, Texture>
	//{{PowerUpType.DAMAGE, tex},
	//{PowerUpType.FIRERATE, tex},
	//{PowerUpType.RELOADSPD, tex},
	//{PowerUpType.CLIPSIZE, tex},
	//{PowerUpType.ADRENALIN, tex},
	//{PowerUpType.SHOTGUN, tex},
	//{PowerUpType.REPEATER, tex}
	//};

    public static Vector3 DirectTargeting(Transform from, Transform to)
    {
        Vector3 direction = (to.position - from.position).normalized;
        return direction;
    }

	public static Func<Transform, Transform, Vector3> CreatePredictiveTargeting(Transform target, float bSpeed) {
		float bulletSpeed = bSpeed;
		Vector3 prevPosition = new Vector3(target.position.x, target.position.y, target.position.z);
		float prevTime = Time.time;
		Func<Transform, Transform, Vector3> predictive = delegate(Transform from, Transform to) {
			float modifiedBulletSpeed = bulletSpeed*SpeedManager.bulletSpeedScaling;
			Vector3 enemyDir = (to.position - prevPosition).normalized;
			Vector3 plane = MathModule.determinePlaneNormal(from.position, to.position, to.position + enemyDir);
			float now = Time.time;
			float enemySpeed = (to.position - prevPosition).magnitude/(now - prevTime);
			// Use law of sines
			float angleBeta = Vector3.Angle(from.position - to.position, enemyDir);
			// Angles towards or away cause the plane defined to not have a major y-component... in these cases, just shoot at them
			if (angleBeta >= 170f || angleBeta <= 10f) return (to.position - from.position).normalized;
			float angleAlpha = Mathf.Abs(Mathf.Rad2Deg*Mathf.Asin(enemySpeed*Mathf.Sin(angleBeta*Mathf.Deg2Rad)/modifiedBulletSpeed));
			Vector3 firingDirection = Quaternion.AngleAxis(angleAlpha, plane) * (to.position - from.position);
			prevPosition.x = to.position.x;
			prevPosition.y = to.position.y;
			prevPosition.z = to.position.z;
			prevTime = now;
			return firingDirection.normalized;
		};
		return predictive;
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
