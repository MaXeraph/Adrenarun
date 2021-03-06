using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	// private Vector3 _enemySpawn = Vector3.zero;
	private bool _cooldown = false;
	private float _cooldownDelay = SpeedManager.enemySpawnScaling;
	private float platformRadius = 175 / 2;

	private bool canSpawn = false; // for within wave
	private bool startSpawn = false; // for each wave
	private int currentWaveNumber = 1;
	public static int currentLevelNumber = 1;
	public static int maxLevelNumber = 5;
	private PowerUpManager pum;
	private Stats _player;
	private float waveStartTime;
	private float waveEndTime;

	private int totalWaveNumber = 4;
	private int enemiesPerWave = 10;
	private const int spawnInterval = 0;
	private int enemiesSpawned = 0;
	private int currentNumEnemies = 0;
	private bool _timeout = false;
	private bool _infinite = false;
	private LevelTransition transition;
	private GameObject player;


	void Start()
	{
		if (LevelTransition.currentLevel == maxLevelNumber) _infinite = true;
		enemiesPerWave += (LevelTransition.progression - 1) * 5;
		transition = transform.GetChild(0).GetComponent<LevelTransition>();
		player = GameObject.FindGameObjectWithTag("Player");
		pum = player.GetComponent<PowerUpManager>();
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
		if (!_timeout)
		{
			StartSpawningWave();
			_timeout = true;

			StartCoroutine(TimeOut());
		}
		GameObject platform = GameObject.FindGameObjectWithTag("Platform");
		Mesh mesh = platform.GetComponent<MeshFilter>().mesh;
		if (LevelTransition.currentLevel == 3)
		{
			platformRadius = platform.transform.localScale.x * 3; // * 4 / 2 because is pro builder platform
		}
		else if (LevelTransition.currentLevel == 2 || LevelTransition.currentLevel == 4) {
			platformRadius = platform.transform.localScale.x * 5; // * 10 / 2 because is terrain not game object
		}
		else {
			platformRadius = platform.transform.localScale.x / 2;
		}
		platformRadius *= 0.9f;
	}


	IEnumerator TimeOut()
	{
		yield return new WaitForSeconds(2);
		_timeout = false;
	}


	void StartSpawningWave()
	{
		if (_infinite) totalWaveNumber += 1;
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
			waveStartTime = Time.time;
			SpawnWave();
		}
		else
		{
			currentNumEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
			UIManager.enemiesLeft = enemiesPerWave - (enemiesSpawned - currentNumEnemies);

			if (currentNumEnemies == 0 && !_timeout )
			{

				waveEndTime = Time.time;
				HealingPill.DespawnPills();
				currentWaveNumber++;
				enemiesPerWave += enemiesPerWave;
				_player.currentHealth += 10;
				pum.presentPowerUps(waveEndTime - waveStartTime, currentWaveNumber);

				if (currentWaveNumber < totalWaveNumber) StartSpawningWave();
				else transition.LevelComplete(this);
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
		if ((enemy == EnemyType.TURRET || enemy == EnemyType.RANGED || enemy == EnemyType.FLYING) && Random.Range(0, 2) == 0)
		{
			variant = EnemyVariantType.PREDICTIVE;
		}
		else if (enemy == EnemyType.TURRET && Random.Range(0, 2) == 0)
		{
			variant = EnemyVariantType.SET;
		}
		else if (enemy == EnemyType.TANK)
		{
			if (Random.Range(0, 2) == 0) {
				variant = EnemyVariantType.SHIELD;
			}
			else {
				variant = EnemyVariantType.AGGRESSOR;
			}
		}
		Vector3 spawnLocation = SpawnManager.enemySpawnBehaviour[enemy][Random.Range(0, SpawnManager.enemySpawnBehaviour[enemy].Length)](platformRadius, player);
		EnemyFactory.Instance.CreateEnemy(spawnLocation, enemy, variant, (currentLevelNumber + 1) / 2f);
	}


	IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(_cooldownDelay);
		_cooldown = false;
	}
}
