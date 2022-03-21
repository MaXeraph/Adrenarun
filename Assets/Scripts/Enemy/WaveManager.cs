using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	// private Vector3 _enemySpawn = Vector3.zero;
	private bool _cooldown = false;
	private float _cooldownDelay = SpeedManager.enemySpawnScaling;
	private const float platformRadius = 175 / 2;

	private bool canSpawn = false; // for within wave
	private bool startSpawn = false; // for each wave
	private int currentWaveNumber = 1;
	private static int currentLevelNumber = 1;
	private PowerUpManager pum;

	private int totalWaveNumber =  4;
	private int enemiesPerWave = 1;
	private const int spawnInterval = 0;
	private int enemiesSpawned = 0;
	private int currentNumEnemies = 0;
	private bool _timeout = false;
	private bool _levelComplete = false; 


	// Start is called before the first frame update
	void Start()
	{
		pum = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpManager>();
		if (!_timeout)
		{
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
			UIManager.enemiesLeft = enemiesPerWave - (enemiesSpawned - currentNumEnemies);

			if (currentNumEnemies == 0 && !_timeout )
			{


				HealingPill.DespawnPills();
				currentWaveNumber++;
				enemiesPerWave += enemiesPerWave;
				pum.presentPowerUps();

				if (currentWaveNumber < totalWaveNumber)
				{
					StartSpawningWave();
				} else
				{

					gameObject.SetActive(false);
				}
				
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
				EnemyType[] currentTypes = SpawnManager.GetEnemyTypes(currentLevelNumber, currentWaveNumber);
				int index = Random.Range(0, currentTypes.Length);
				SpawnEnemy(currentTypes[index]);
				_cooldown = true;
				currentNumEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
				enemiesSpawned += 1;
				UIManager.enemiesLeft = enemiesPerWave - (enemiesSpawned - currentNumEnemies);
				StartCoroutine(Cooldown());
			}
			else
			{
				if (enemiesSpawned >= enemiesPerWave)
				{
					StopSpawningWave();
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
		Vector3 spawnLocation = SpawnManager.enemySpawnBehaviour[enemy][Random.Range(0, SpawnManager.enemySpawnBehaviour[enemy].Length)](platformRadius);
		EnemyFactory.Instance.CreateEnemy(spawnLocation, enemy, variant);
	}


	IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(_cooldownDelay);
		_cooldown = false;
	}
}
