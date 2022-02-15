using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private Vector3 _enemySpawn = Vector3.zero;
    private Vector3 _platformSize = Vector3.zero;
    private Transform _platformTransform;
    private bool _cooldown = false;
    private float _cooldownDelay = SpeedManager.enemySpawnScaling;

    private const float _abovePlatformHeight = 0.6f; //TODO:  check this value
    private const float _platformOffset = 1f; //TODO:  check this value


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

    void SpawnEnemy()
    {
        _enemySpawn.x = _platformTransform.position[0] + Random.Range(-_platformSize.x, _platformSize.x);
        _enemySpawn.z = _platformTransform.position[2] + Random.Range(-_platformSize.z, _platformSize.z);
        _enemySpawn.y = _platformTransform.position[1] + _platformSize.y;

        GameObject.Instantiate(Resources.Load("Enemy"), _enemySpawn, Quaternion.identity);
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownDelay);
        _cooldown = false;
    }
}
