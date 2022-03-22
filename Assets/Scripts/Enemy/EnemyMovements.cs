using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class EnemyMovements
{
	private const float _grenadierBaseSpeed = 6f;
	private const float _rangedBaseSpeed = 8f;
	private const float _tankBaseSpeed = 12f;

	public static void GrenadierSetup(Vector3 transform)
	{

	}
	public static void GrenadierMovement(GameObject gameObject, Vector3 playerPosition)
	{
		Vector3 position = gameObject.transform.position;
		Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
		NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
		navAgent.speed = _grenadierBaseSpeed * SpeedManager.enemyMovementScaling;
		float playerDistance = Vector3.Distance(position, playerPosition);
		if (playerDistance > 40)
		{
			navAgent.SetDestination(playerPosition);
			anim.Play("grenad walk");
		}
		else anim.Stop();

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

	public static void RangedMovement(GameObject gameObject, Vector3 playerPosition)
	{
		Vector3 position = gameObject.transform.position;
		Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
		NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
		navAgent.speed = _rangedBaseSpeed * SpeedManager.enemyMovementScaling;
		float playerDistance = Vector3.Distance(position, playerPosition);
		if (playerDistance > 15)
		{
			navAgent.SetDestination(playerPosition);
		}
		if (playerDistance > 10) anim.Play("mech ");
		else anim.Stop();
	}

	public static void TankMovement(GameObject gameObject, Vector3 playerPosition)
	{
		Vector3 position = gameObject.transform.position;
		NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
		navAgent.speed = _tankBaseSpeed * SpeedManager.enemyMovementScaling;
		float playerDistance = Vector3.Distance(position, playerPosition);
		if (playerDistance > 0)
		{
			navAgent.SetDestination(playerPosition);
		}
	}
	public static void TankSetup(Vector3 transform)
	{

	}

	public static void FlyingMovement(GameObject gameObject, Vector3 playerPosition)
	{
		Vector3 position = gameObject.transform.position;
		Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
		NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
		navAgent.speed = _grenadierBaseSpeed * SpeedManager.enemyMovementScaling;
		float playerDistance = Vector3.Distance(position, playerPosition);
		if (playerDistance > 7)
		{
			navAgent.SetDestination(playerPosition);
			anim.Play("flying");
		}

	}
	
	public static void FlyingSetup(Vector3 transform) {

	}
}
