using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int numOfEnemies;
    public string[] typeOfEnemies;
    public float spawnInterval;

    public Wave(string name, int num, string[] types, float interval)
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
    private const float platformRadius = 175/2;

    private bool canSpawn = false; // for within wave
    private bool startSpawn = false; // for each wave 
    private float nextSpawnTime; // not used for now 
    private Wave currentWave;
    private int currentWaveNumber = 0;

    public int totalWaveNumber;
    public int enemiesPerWave;
    private const int spawnInterval = 0;
    private int currentNumEnemies = 0;

    public Wave[] waves;
    public Vector3[] spawnPoints; // not used for now, may need later 

    // Start is called before the first frame update
    void Start()
    {
        waves = new Wave[totalWaveNumber];
        string[] enemy = new string[1];
        enemy[0] = "Enemy";

        for (int i = 0; i < totalWaveNumber; i++)
        {
            string name = "wave " + i.ToString();
            waves[i] = new Wave(name, enemiesPerWave, enemy, spawnInterval);
        }

        // need to generate the spawnpoints location here
        // already have spawnpoints array but need to populate it here
        // we dont need spawnpoints yet because spawnEnemy spawn randon

        StartSpawningWave();
        currentWave = waves[currentWaveNumber];
    }

    void StartSpawningWave()
    {
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
                }
                else
                {
                    // still have more waves to spawn

                    currentWave = waves[currentWaveNumber];
                    // grant power up here as well 


                    StartSpawningWave();
                }
                
            }
            // startSpawn only turns to false when we make all the enemies in the same wave
            // check if the number of enemies has reached 0
            // check if we are on the last wave
            // implement powerup here
        }
       
        
    }

    void SpawnWave()
    {
        if (!_cooldown)
        {
            _cooldownDelay = SpeedManager.enemySpawnScaling;
            if (canSpawn && currentNumEnemies < enemiesPerWave)
            {
                SpawnEnemy();
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


    void SpawnEnemy()
    {
        Vector3 targetSpawn;
        if (RandomPoint(platformRadius, out targetSpawn)) {
            Debug.DrawRay(targetSpawn, Vector3.up, Color.blue, 1.0f);
            EnemyFactory.Instance.CreateEnemy(EnemyType.TURRET, targetSpawn);
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownDelay);
        _cooldown = false;
    }
}
