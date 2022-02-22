using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
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