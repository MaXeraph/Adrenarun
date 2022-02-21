using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int numOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}

public class EnemySpawn : MonoBehaviour
{
    private Vector3 _enemySpawn = Vector3.zero;
    private bool _cooldown = false;
    private float _cooldownDelay = SpeedManager.enemySpawnScaling;
    private const float platformRadius = 175/2;

    public Wave[] waves;
    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!_cooldown)
        {
            _cooldownDelay = SpeedManager.enemySpawnScaling;
            SpawnEnemy();
            _cooldown = true;
            StartCoroutine(Cooldown());
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
            EnemyFactory.Instance.CreateEnemy("Enemy", targetSpawn);
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownDelay);
        _cooldown = false;
    }
}
