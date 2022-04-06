using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Generic EnemyBehaviour. Should be initialized after creation via Initialize(), otherwise is not usable.
 */
public class EnemyBehaviour : MonoBehaviour
{
	private bool _initialized = false;
	public EnemyType enemyType;
	// Transform of the enemy's target.
	private Transform _targetTransform;
	private Quaternion _lookRotation;
	private Vector3 _direction;
	private Weapon _weapon;

	// Method to pathfind from Transform to Transform, returns Vector3, the direction to move in next.
	private Action<GameObject, Vector3> NavAgentMove;
	// Method to determine aiming direction from Transform to Transform, returns Vector3, the direction to fire in.
	public Func<Transform, Transform, Vector3> GetAimDirection;

	public static EnemyBehaviour AddToGameObject(
		GameObject gameObject,
		GameObject target,
		EnemyInfo info,
		Weapon weapon)
	{
		EnemyBehaviour eb = gameObject.AddComponent<EnemyBehaviour>();
		eb.Initialize(target, info, weapon);

		return eb;
	}

	// Initialize must be run for the Update loop to run.
	// This is because the mechanisms (Pathfind, Aim, etc.) aren't set until Initialize is called.
	public bool Initialize(GameObject target,
		EnemyInfo info,
		Weapon weapon)
	{
		if (_initialized) return false;
		_initialized = true;

		enemyType = info.enemyType;
		_weapon = weapon;
		_targetTransform = target.GetComponent<Transform>();
		NavAgentMove = info.navAgentMove;
		GetAimDirection = info.aim;
		info.navAgentSetup(new Vector3(0, 0, 0));

		lastPosition = transform.position;
		
		return true;
	}

	public static void Destroy(GameObject enemy)
	{
		EnemyBehaviour eb = enemy.GetComponent<EnemyBehaviour>();
		EnemyType type = eb.enemyType;
		if (type == EnemyType.TANK) {
			AudioManager.PlayEnemyTankDeathAudio();
		} else {
			AudioManager.PlayEnemyDeathAudio();
		}
		Destroy(eb);
		ObjectPool.Destroy(Globals.enemyPrefabNames[type], enemy);
	}

	private Vector3 lastPosition;

	// Update is called once per frame
	void Update()
	{
		if (!_initialized) return;

		Animation anim = gameObject.transform.childCount > 0 ? gameObject.transform.GetChild(0).GetComponent<Animation>() : null;
		
		// Look. Determine look direction. Only do this if we haven't moved to not interfere with NavAgent.
		if (transform.position == lastPosition)
		{
			_direction = (_targetTransform.position - transform.position).normalized;
			_direction.y = 0;
			// To change maximum rotation speed, lower 360f.
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, _direction, Mathf.Deg2Rad*360f*Time.deltaTime, 100f);
			_lookRotation = Quaternion.LookRotation(newDirection);
			transform.rotation = _lookRotation;
		}

		lastPosition = transform.position;
		
		NavAgentMove(this.gameObject, _targetTransform.position);

		// Aim. Determine firing direction.
		Vector3 aimDirection = GetAimDirection(transform, _targetTransform);

		// TODO: spawn bullet outside of model
		if (_weapon.Attack(transform.position, aimDirection, EntityType.ENEMY) && anim)
		{
			foreach (AnimationState state in anim)
			{
				if (state.name == "shoot") anim.Play("shoot");
			}
		}
	}
}
