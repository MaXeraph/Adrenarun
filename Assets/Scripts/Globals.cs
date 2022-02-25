using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
	public static Dictionary<PowerUpType, StatPowerUp> StatPowerUpDictionary = new Dictionary<PowerUpType, StatPowerUp>
	{{PowerUpType.DAMAGE, new StatPowerUp(5f)},
	{PowerUpType.FIRERATE, new StatPowerUp(-0.04f)},
	{PowerUpType.RELOADSPD, new StatPowerUp(1f)},
	{PowerUpType.CLIPSIZE, new StatPowerUp(2f)},
	{PowerUpType.ADRENALIN, new StatPowerUp(0.15f)}
	};

	public static Vector3 DirectTargeting(Transform from, Transform to) {
		Vector3 direction = (to.position - from.position).normalized;
		return direction;
	}

	public static Dictionary<EnemyType, string> enemyPrefabNames = new Dictionary<EnemyType, string>()
	{
		{ EnemyType.TURRET, "Turret" }
	};
}

public enum EnemyType
{
	TURRET
}

public enum EntityType {
	PLAYER,
	ENEMY
}

public enum PowerUpType{
	NONE,
	DAMAGE,
	FIRERATE,
	RELOADSPD,
	CLIPSIZE,
	ADRENALIN
}
