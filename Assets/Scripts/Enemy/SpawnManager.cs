using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;

public static class SpawnManager {


    // chance of enemy spawning on level i wave j is _[ENEMY]SpawnChances[i-1,j-1]
	// make sure the sum of SpawnChances[i,j] = 1
	// also assuming the lowest spawn chance will be 0.05f, everything else in increments of that

	// private static int[,] _tankSpawnChances = {{0, 0, 5, 0}, {5, 5, 10, 10}, {10, 10, 10, 10}};



	private static int[,] _grenadierSpawnChances = {{0, 15, 30, 30, 30, 30}};
	private static int[,] _flyingSpawnChances 	 = {{0, 10, 10, 10, 10, 10}};
	private static int[,] _rangedSpawnChances    = {{0, 35, 30, 30, 30, 30}};
	private static int[,] _tankSpawnChances 	 = {{100,  10,  5,  5, 10, 10}};
	private static int[,] _turretSpawnChances    = {{0, 25, 20, 20, 15, 15}};
	private static int[,] _healerSpawnChances    = {{0, 5, 5, 5, 5, 5 }};
	private static Dictionary<EnemyType, int[,]> enemySpawnChances = new Dictionary<EnemyType, int[,]>() {
		{EnemyType.GRENADIER, _grenadierSpawnChances},
		{EnemyType.RANGED, _rangedSpawnChances},
		{EnemyType.FLYING, _flyingSpawnChances},
		{EnemyType.TANK, _tankSpawnChances},
		{EnemyType.TURRET, _turretSpawnChances},
		{EnemyType.HEALER, _healerSpawnChances},
	};

	public static EnemyType[] GetEnemyTypes(int level, int wave) {
		EnemyType[] temp = new EnemyType[100];
		int temp_index = 0;
		foreach(var item in enemySpawnChances) {
			for (int i=0; i < item.Value[level-1,wave-1]; i++) {
				temp[temp_index + i] = item.Key;
			}
			temp_index += item.Value[level-1,wave-1];
		}

		return temp;
	}

	private static Func<float, GameObject, Vector3>[] _grenadierSpawnBehaviours = {SpawnGrenadierDefault};
	private static Func<float, GameObject, Vector3>[] _flyingSpawnBehaviours = {SpawnDefault};
	private static Func<float, GameObject, Vector3>[] _rangedSpawnBehaviours = {SpawnDefault};
	private static Func<float, GameObject, Vector3>[] _tankSpawnBehaviours = {SpawnDefault, SpawnTank};
	private static Func<float, GameObject, Vector3>[] _turretSpawnBehaviours = {SpawnTurretDefault};
	private static Func<float, GameObject, Vector3>[] _healerSpawnBehaviours = {SpawnDefault};
	public static Dictionary<EnemyType, Func<float, GameObject, Vector3>[]> enemySpawnBehaviour = new Dictionary<EnemyType, Func<float, GameObject, Vector3>[]>() {
		{EnemyType.GRENADIER, _grenadierSpawnBehaviours},
		{EnemyType.FLYING, _flyingSpawnBehaviours},
		{EnemyType.RANGED, _rangedSpawnBehaviours},
		{EnemyType.TANK, _tankSpawnBehaviours},
		{EnemyType.TURRET, _turretSpawnBehaviours},
		{EnemyType.HEALER, _healerSpawnBehaviours},
	};




    public static Vector3 SpawnDefault(float platformRadius, GameObject player) {
        Vector3 targetSpawn;
        if (RandomPoint(platformRadius, out targetSpawn))
        {
            return targetSpawn;
        }
        return new Vector3(0, 0, 0);
    }

    public static Vector3 SpawnTurretDefault(float platformRadius, GameObject player) {
		platformRadius *= 0.6f;
        Vector3 targetSpawn;
        if (RandomPoint(platformRadius, out targetSpawn))
		{
            return targetSpawn;
		}
        return new Vector3(0, 0, 0);
    }

	public static Vector3 SpawnGrenadierDefault(float platformRadius, GameObject player) {
		platformRadius *= 0.8f;
		Vector3 targetSpawn;
        if (RandomPointOnEdge(platformRadius, out targetSpawn))
		{
            return targetSpawn;
		}

		return new Vector3(0, 0, 0);
	}

    public static Vector3 SpawnTank(float platformRadius, GameObject player) {
        return player.transform.position + (player.transform.forward * 25);
    }


    private static bool RandomPoint(float radius, out Vector3 result)
	{
		Vector3 randomPoint = Random.insideUnitSphere * radius;
		NavMeshHit hit;
		if (NavMesh.SamplePosition(randomPoint, out hit, radius, 1))
		{
			result = hit.position;
			return true;
		}
		result = Vector3.zero;
		return false;
	}

	private static bool RandomPointOnEdge(float radius, out Vector3 result)
	{
		Vector2 tempPoint = Random.insideUnitCircle.normalized * radius;
        Vector3 randomPoint = new Vector3(tempPoint.x, 0, tempPoint.y);
		NavMeshHit hit;
		if (NavMesh.SamplePosition(randomPoint, out hit, radius, 1))
		{
			result = hit.position;
			return true;
		}
		result = Vector3.zero;
		return false;
	}

}