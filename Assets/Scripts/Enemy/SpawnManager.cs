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



	private static int[,] _grenadierSpawnChances = {{20, 15, 30, 30, 30, 30}};
	private static int[,] _flyingSpawnChances 	 = {{10, 10, 10, 10, 10, 10}};
	private static int[,] _rangedSpawnChances    = {{40, 35, 30, 30, 30, 30}};
	private static int[,] _tankSpawnChances 	 = {{0,  10,  5,  5, 10, 10}};
	private static int[,] _turretSpawnChances    = {{30, 30, 25, 25, 20, 20}};
	private static Dictionary<EnemyType, int[,]> enemySpawnChances = new Dictionary<EnemyType, int[,]>() {
		{EnemyType.GRENADIER, _grenadierSpawnChances},
		{EnemyType.RANGED, _rangedSpawnChances},
		{EnemyType.FLYING, _flyingSpawnChances},
		{EnemyType.TANK, _tankSpawnChances},
		{EnemyType.TURRET, _turretSpawnChances}
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

	private static Func<float, Vector3>[] _grenadierSpawnBehaviours = {SpawnDefault};
	private static Func<float, Vector3>[] _flyingSpawnBehaviours = {SpawnDefault};
	private static Func<float, Vector3>[] _rangedSpawnBehaviours = {SpawnDefault};
	private static Func<float, Vector3>[] _tankSpawnBehaviours = {SpawnDefault};
	private static Func<float, Vector3>[] _turretSpawnBehaviours = {SpawnTurretDefault};
	public static Dictionary<EnemyType, Func<float, Vector3>[]> enemySpawnBehaviour = new Dictionary<EnemyType, Func<float, Vector3>[]>() {
		{EnemyType.GRENADIER, _grenadierSpawnBehaviours},
		{EnemyType.FLYING, _flyingSpawnBehaviours},
		{EnemyType.RANGED, _rangedSpawnBehaviours},
		{EnemyType.TANK, _tankSpawnBehaviours},
		{EnemyType.TURRET, _turretSpawnBehaviours}
	};




    public static Vector3 SpawnDefault(float platformRadius) {
        Vector3 targetSpawn;
        if (RandomPoint(platformRadius, out targetSpawn))
		{
            return targetSpawn;
		}
        return new Vector3(0, 0, 0);
    }

    public static Vector3 SpawnTurretDefault(float platformRadius) {
		platformRadius /= 1.5f;
        Vector3 targetSpawn;
        if (RandomPoint(platformRadius, out targetSpawn))
		{
            return targetSpawn;
		}
        return new Vector3(0, 0, 0);
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


}