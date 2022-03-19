using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Wave
{
	public string waveName;
	public int numOfEnemies;
	public EnemyType[] typeOfEnemies;
	public float spawnInterval;

	public Wave(string name, int num, EnemyType[] types, float interval)
	{
		waveName = name;
		numOfEnemies = num;
		typeOfEnemies = types;
		spawnInterval = interval;
	}
}

public class EnemySpawn : MonoBehaviour
{
	private Vector3 _enemySpawn = Vector3.zero;
	private bool _cooldown = false;
	private float _cooldownDelay = SpeedManager.enemySpawnScaling;
	private const float platformRadius = 175 / 2;

	private bool canSpawn = false; // for within wave
	private bool startSpawn = false; // for each wave
	private float nextSpawnTime; // not used for now
	private Wave currentWave;
	private int currentWaveNumber = 1;
	private static int currentLevelNumber = 1;
	private PowerUpManager pum;

	private int totalWaveNumber;
	private int enemiesPerWave = 15;
	private const int spawnInterval = 0;
	private int enemiesSpawned = 0;
	private int currentNumEnemies = 0;
	private bool _timeout = false;
	private EnemyType[] enemy;

	public Wave[] waves;
	public Vector3[] spawnPoints; // not used for now, may need later

	// Start is called before the first frame update
	void Start()
	{
		pum = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpManager>();
		// waves = new Wave[totalWaveNumber];
		enemy = new EnemyType[4];
		enemy[0] = EnemyType.TURRET;
		enemy[1] = EnemyType.GRENADIER;
		enemy[2] = EnemyType.RANGED;
		enemy[3] = EnemyType.TANK;

		// /*for (int i = 0; i < totalWaveNumber; i++)
		// {
		//     string name = "wave " + i.ToString();
		//     waves[i] = new Wave(name, enemiesPerWave, enemy, spawnInterval);
		// }*/

		if (!_timeout)
		{
			// currentWave = waves[currentWaveNumber];
			currentWave = new Wave(name, enemiesPerWave, enemy, spawnInterval);
			StartSpawningWave();
			_timeout = true;

			StartCoroutine(TimeOut());
		}


	}

	IEnumerator TimeOut()
	{
		yield return new WaitForSeconds(2);
		_timeout = false;
	}
	void StartSpawningWave()
	{
		UIManager.enemiesTotal = enemiesPerWave;
		UIManager.enemiesLeft = enemiesPerWave;
		enemiesSpawned = 0;
		canSpawn = true;
		startSpawn = true;
	}

	void StopSpawningWave()
	{
		canSpawn = false;
		startSpawn = false;

	}
	// Update is called once per frame
	void Update()
	{
		if (startSpawn && !_timeout)
		{
			SpawnWave();
		}
		else
		{
			currentNumEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
			UIManager.enemiesLeft = enemiesPerWave - (enemiesSpawned - currentNumEnemies); ;
			//Debug.Log(currentNumEnemies);
			//Debug.Log(enemiesSpawned);

			if (currentNumEnemies == 0 && !_timeout)
			{
				HealingPill.DespawnPills();
				currentWaveNumber++;
				enemiesPerWave += enemiesPerWave;

				currentWave = new Wave(name, enemiesPerWave, enemy, spawnInterval);
				pum.presentPowerUps();
				StartSpawningWave();

				// /*
				// if (currentWaveNumber >= totalWaveNumber)
				// {
				//     //Debug.Log("Game Over!");
				//     // TODO : GAME OVER UI
				// }
				// else
				// {
				//     // still have more waves to spawn

				//     currentWave = waves[currentWaveNumber];
				//     enemiesPerWave += 5;
				//     // grant power up here as well
				//     // TODO : POWER UP UI
				//     pum.presentPowerUps();

				//     StartSpawningWave();
				// }
				// */

			}

		}


	}

	void SpawnWave()
	{
		if (!_cooldown)
		{
			_cooldownDelay = SpeedManager.enemySpawnScaling;
			if (canSpawn && enemiesSpawned < enemiesPerWave)
			{

				// TODO: need to fix the way this is implemented
				// Random on array
				EnemyType[] currentTypes = SpawnBehaviour.GetEnemyTypes(currentLevelNumber, currentWaveNumber);
				int index = Random.Range(0, currentTypes.Length);
				SpawnEnemy(currentTypes[index]);
				_cooldown = true;
				currentNumEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
				enemiesSpawned += 1;
				UIManager.enemiesLeft = enemiesPerWave - (enemiesSpawned - currentNumEnemies);
				//Debug.Log(enemiesSpawned);
				StartCoroutine(Cooldown());
			}
			else
			{
				if (enemiesSpawned >= enemiesPerWave)
				{
					StopSpawningWave();
					//Debug.Log("stopped spawning the current wave");
				}
			}
		}

	}
	


	void SpawnEnemy(EnemyType enemy)
	{
		EnemyVariantType variant = EnemyVariantType.NONE;
		// 50% to be predictive if turret or ranged.
		if ((enemy == EnemyType.TURRET || enemy == EnemyType.RANGED) && Random.Range(0, 2) == 0)
		{
			variant = EnemyVariantType.PREDICTIVE;
		}
		else if (enemy == EnemyType.TURRET && Random.Range(0, 2) == 0)
		{
			variant = EnemyVariantType.SET;
		}
		else if (enemy == EnemyType.TANK)
		{
			variant = EnemyVariantType.SHIELD;
		}
		Vector3 spawnLocation = SpawnBehaviour.enemySpawnBehaviour[enemy][Random.Range(0, SpawnBehaviour.enemySpawnBehaviour[enemy].Length)](platformRadius);
		EnemyFactory.Instance.CreateEnemy(spawnLocation, enemy, variant);
	}

	IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(_cooldownDelay);
		_cooldown = false;
	}
}
