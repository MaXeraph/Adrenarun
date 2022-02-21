using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
	public static Vector3 DirectTargeting(Transform from, Transform to) {
		Vector3 direction = (to.position - from.position).normalized;
		return direction;
	}
}

public enum EntityType {
	PLAYER,
	ENEMY
}

public enum PowerUpType {
	NONE,
	DAMAGE,
	FIRERATE,
	RELOADSPD,
	CLIPSIZE,
	ADRENALIN
}
