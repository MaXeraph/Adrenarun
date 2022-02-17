using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    private Vector3 _enemySpawn = Vector3.zero;
    private Vector3 _platformSize = Vector3.zero;
    private Transform _platformTransform;
    private bool _cooldown = false;
    private float _cooldownDelay = SpeedManager.enemySpawnScaling;

    private const float _abovePlatformHeight = 0.6f; //TODO:  check this value
    private const float _platformOffset = 1f; //TODO:  check this value

    private const float platformRadius = 175/2;


    // public UpdatePlayerInfo UpdatePlayerInfo;
    // Start is called before the first frame update
    void Start()
    {
        _platformTransform = GameObject.FindGameObjectWithTag("Platform").transform;

        _platformSize.x = _platformTransform.localScale.x / 2 - _platformOffset;
        _platformSize.z = _platformTransform.localScale.z / 2 - _platformOffset;
        _platformSize.y = _platformTransform.localScale.y + _abovePlatformHeight;
        InvokeRepeating("SpawnEnemy", 0, 5);
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
            Debug.Log(result);
            return true;
        }
        result = Vector3.zero;
        Debug.Log(result);
        return false;
    }


    void SpawnEnemy()
    {
        Debug.Log("spawning");
        // _enemySpawn.x = _platformTransform.position[0] + Random.Range(-_platformSize.x, _platformSize.x);
        // _enemySpawn.z = _platformTransform.position[2] + Random.Range(-_platformSize.z, _platformSize.z);
        // _enemySpawn.y = _platformTransform.position[1] + _platformSize.y;

        Vector3 targetSpawn;
        if (RandomPoint(platformRadius, out targetSpawn)) {
            Debug.DrawRay(targetSpawn, Vector3.up, Color.blue, 1.0f);
            GameObject.Instantiate(Resources.Load("Enemy"), targetSpawn, Quaternion.identity);
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownDelay);
        _cooldown = false;
    }
}
