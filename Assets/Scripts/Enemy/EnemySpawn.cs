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
    private int currentWaveNumber = 0;
    private PowerUpManager pum;

    public int totalWaveNumber;
    public int enemiesPerWave;
    private const int spawnInterval = 0;
    private int currentNumEnemies = 0;

    public Wave[] waves;
    public Vector3[] spawnPoints; // not used for now, may need later

    // Start is called before the first frame update
    void Start()
    {
        pum = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpManager>();
        waves = new Wave[totalWaveNumber];
        EnemyType[] enemy = new EnemyType[2];
        enemy[0] = EnemyType.TURRET;
        enemy[1] = EnemyType.GRENADIER;

        for (int i = 0; i < totalWaveNumber; i++)
        {
            string name = "wave " + i.ToString();
            waves[i] = new Wave(name, enemiesPerWave, enemy, spawnInterval);
        }

        StartSpawningWave();
        currentWave = waves[currentWaveNumber];
    }

    void StartSpawningWave()
    {
        UIManager.enemiesTotal = 0;
        UIManager.enemiesLeft = 0;
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
        if (startSpawn)
        {
            SpawnWave();
        }
        else
        {
            currentNumEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            //Debug.Log(currentNumEnemies);

            if (currentNumEnemies == 0)
            {
                currentWaveNumber++;
                if (currentWaveNumber >= totalWaveNumber)
                {
                    //Debug.Log("Game Over!");
                    // TODO : GAME OVER UI
                }
                else
                {
                    // still have more waves to spawn

                    currentWave = waves[currentWaveNumber];
                    // grant power up here as well
                    // TODO : POWER UP UI
                    pum.presentPowerUps();

                    StartSpawningWave();
                }

            }

        }


    }

    void SpawnWave()
    {
        if (!_cooldown)
        {
            _cooldownDelay = SpeedManager.enemySpawnScaling;
            if (canSpawn && currentNumEnemies < enemiesPerWave)
            {

                // TODO: need to fix the way this is implemented
                // Random on array
                EnemyType[] currentTypes = currentWave.typeOfEnemies;
                int index = Random.Range(0, currentTypes.Length);
                SpawnEnemy(currentTypes[index]);
                _cooldown = true;
                currentNumEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
                //Debug.Log(currentNumEnemies);
                StartCoroutine(Cooldown());
            }
            else
            {
                if (currentNumEnemies >= enemiesPerWave)
                {
                    StopSpawningWave();
                    //Debug.Log("stopped spawning the current wave");
                }
            }
        }

    }
    bool RandomPoint(float radius, out Vector3 result)
    {
        Vector3 randomPoint = Random.insideUnitSphere * radius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }


    void SpawnEnemy(EnemyType enemy)
    {

        // need to include variant type as a parameter
        // default is None for a type from factory 

        Vector3 targetSpawn;
        EnemyVariantType variant = (EnemyVariantType)Random.Range(0, 2);
        if (RandomPoint(platformRadius, out targetSpawn))
        {
            Debug.DrawRay(targetSpawn, Vector3.up, Color.blue, 1.0f);
            EnemyFactory.Instance.CreateEnemy(targetSpawn, enemy, variant);
            UIManager.enemiesTotal += 1;
            UIManager.enemiesLeft += 1;
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownDelay);
        _cooldown = false;
    }
}
