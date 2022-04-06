using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class EnemyMovements
{
	private const float _grenadierBaseSpeed = 6f;
	private const float _rangedBaseSpeed = 8f;
	private const float _tankBaseSpeed = 8f;

	public static void GrenadierSetup(Vector3 transform)
	{

	}
	
	public static Action<GameObject, Vector3> CreateGrenadierMovement()
	{
		int angle = UnityEngine.Random.Range(-90, 90);
		Vector3 vectorToEnemy = Vector3.zero;
		return delegate(GameObject gameObject, Vector3 playerPosition)
		{
			Vector3 position = gameObject.transform.position;
			if (vectorToEnemy == Vector3.zero) vectorToEnemy = (position - playerPosition).normalized;
			Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
			NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
			navAgent.speed = _grenadierBaseSpeed * SpeedManager.enemyMovementScaling;
			float playerDistance = Vector3.Distance(position, playerPosition);
			if (playerDistance > 0)
			{
				float y = vectorToEnemy.y;
				vectorToEnemy.y = 0;
				Vector3 newPosition = (Quaternion.AngleAxis(angle, Vector3.up) * vectorToEnemy) * 40 + playerPosition;
				newPosition.y = y;
				navAgent.SetDestination(newPosition);
			}

			if (playerDistance > 0) anim.Play("grenad walk");
			else anim.Stop();
		};
	}

	public static void TurretMovement(GameObject gameObject, Vector3 playerPosition)
	{

	}
	public static void TurretSetup(Vector3 transform)
	{

	}

	public static void RangedSetup(Vector3 transform)
	{

	}

	public static Action<GameObject, Vector3> CreateRangedMovement()
	{
		int angle = UnityEngine.Random.Range(-90, 90);
		return delegate(GameObject gameObject, Vector3 playerPosition)
		{
			Vector3 position = gameObject.transform.position;
			Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
			NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
			navAgent.speed = _rangedBaseSpeed * SpeedManager.enemyMovementScaling;
			float playerDistance = Vector3.Distance(position, playerPosition);

			Vector3 vectorToEnemy = position - playerPosition;
			float y = vectorToEnemy.y;
			vectorToEnemy.y = 0;
			Vector3 newPosition = (Quaternion.AngleAxis(angle, Vector3.up) * vectorToEnemy).normalized * 30 + playerPosition;
			navAgent.SetDestination(newPosition);

			if (playerDistance > 0) anim.Play("mech ");
			else anim.Stop();
		};
	}

	public static void TankMovement(GameObject gameObject, Vector3 playerPosition)
	{
		Vector3 position = gameObject.transform.position;
		Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
		NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
		navAgent.speed = _tankBaseSpeed * SpeedManager.enemyMovementScaling;
		float playerDistance = Vector3.Distance(position, playerPosition);
		if (playerDistance > 0)
		{
			navAgent.SetDestination(playerPosition);
			anim.Play("Tank_game_rig|shield");
		}
	}
	public static void TankSetup(Vector3 transform)
	{

	}
	
	public static void HealerMovement(GameObject gameObject, Vector3 playerPosition)
	{
		Vector3 position = gameObject.transform.position;
		NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
		navAgent.speed = _tankBaseSpeed * SpeedManager.enemyMovementScaling;
		float playerDistance = Vector3.Distance(position, playerPosition);
		if (playerDistance > 15)
		{
			navAgent.SetDestination(playerPosition);
		}
	}

	private static bool CheckLOS(Vector3 posA, Vector3 posB)
	{
		RaycastHit[] hits = Physics.RaycastAll(posA, posB - posA);
		Array.Sort(hits,
			(RaycastHit a, RaycastHit b) => ((a.transform.position - posA).magnitude -
			                                 (b.transform.position - posA).magnitude) > 0 ? 1 : -1);
		for (int i = 0; i < hits.Length; i++)
		{
			if (Vector3.Distance(posA, hits[i].transform.position) == Vector3.Distance(posA, posB)) return true;
			if (hits[i].transform.root.gameObject.tag == "PlatformObjects") return false;
		}

		return true;
	}
	
	public static Action<GameObject, Vector3> CreateHealerMovement()
	{
		int angle = UnityEngine.Random.Range(-90, 90);
		Vector3 vectorToEnemy = Vector3.zero;
		return delegate(GameObject gameObject, Vector3 playerPosition)
		{
			Vector3 position = gameObject.transform.position;
			if (vectorToEnemy == Vector3.zero) vectorToEnemy = (position - playerPosition).normalized;
			// Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
			NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
			navAgent.speed = _grenadierBaseSpeed * SpeedManager.enemyMovementScaling;

			vectorToEnemy.y = 0;
			Vector3 newPosition = (Quaternion.AngleAxis(angle, Vector3.up) * vectorToEnemy) * 40 + playerPosition;
			for (int i = 0; i < 5 && !CheckLOS(playerPosition, newPosition); i++)
			{
				angle = UnityEngine.Random.Range(-90, 90);
				newPosition = (Quaternion.AngleAxis(angle, Vector3.up) * vectorToEnemy) * 40 + playerPosition;
			}
			
			navAgent.SetDestination(newPosition);
		};
	}

	public static void HealerSetup(Vector3 transform)
	{
	}

	public static Action<GameObject, Vector3> CreateFlyingMovement()
	{
		int angle = UnityEngine.Random.Range(60, 90);
		return delegate(GameObject gameObject, Vector3 playerPosition)
		{
			Vector3 position = gameObject.transform.position;
			Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
			NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
			navAgent.speed = _grenadierBaseSpeed * SpeedManager.enemyMovementScaling;
			float playerDistance = Vector3.Distance(position, playerPosition);

			Vector3 vectorToEnemy = (position - playerPosition);
			float y = vectorToEnemy.y;
			vectorToEnemy.y = 0;
			Vector3 newPosition = (Quaternion.AngleAxis(angle, Vector3.up) * vectorToEnemy).normalized * 30 + playerPosition;
			navAgent.SetDestination(newPosition);

			anim.Play("flying");
		};
	}

	public static void FlyingSetup(Vector3 transform) 
	{
	}
}
