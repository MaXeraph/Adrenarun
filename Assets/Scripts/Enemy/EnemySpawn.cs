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

    private bool canSpawn = false;
    private bool startSpawn = false;
    private float nextSpawnTime;
    private Wave currentWave;
    private int currentWaveNumber = 0;

    private const int totalWaveNumber = 3;
    private const int enemiesPerWave = 10;
    private const int spawnInterval = 0;
    private int currentNumEnemies = 0;

    public Wave[] waves;
    public Vector3[] spawnPoints;

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
    }

    void StartSpawningWave()
    {
        UIManager.enemiesTotal = 0;
        canSpawn = true;
        startSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {

        currentWave = waves[currentWaveNumber];
        SpawnWave();


        if (!_cooldown)
        {
            _cooldownDelay = SpeedManager.enemySpawnScaling;
            if (canSpawn && currentNumEnemies < enemiesPerWave)
            {
                SpawnEnemy();
                _cooldown = true;
                currentNumEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
                StartCoroutine(Cooldown());
            }
            else
            {
                if(currentNumEnemies >= enemiesPerWave)
                {
                    canSpawn = false;
                    // Debug.Log(GameObject.FindGameObjectsWithTag("Enemy").Length);
                }
            }
        }
    }

    void SpawnWave()
    {

        // only do this if the currentWave enemy finishes
        // utilize the randomly generated vector 3 values and call on spawnEnemy

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
        UIManager.enemiesTotal += 1;
        UIManager.enemiesLeft += 1;

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
