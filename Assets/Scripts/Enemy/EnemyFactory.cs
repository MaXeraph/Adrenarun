using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    To test a spawn, put this in PlayerControls.cs, then hit 'O'.
    private bool test = false;
    ...
    if (Input.GetKey(KeyCode.O) && !test)
        {
            test = true;
            EnemyFactory.Instance.CreateEnemy("Enemy", new Vector3(0, 0, 8));
        }
*/
 
/**
 * The goal of the EnemyFactory is to take a position, what kind of enemy we want, and spawn it there.
 * The enemy spawning mechanism will use this factory to spawn enemies.
 */
public class EnemyFactory
{
    // Additional setup for an enemy.
    private static Dictionary<string, Action<EnemyBehaviour>> _enemySetups = new Dictionary<string, Action<EnemyBehaviour>>();
    private static Func<Transform, Transform, Vector3> _defaultFunc = (Transform t1, Transform t2) => new Vector3(0, 0, 0);
    private static Action<Transform, Vector3> _defaultMove = (Transform t, Vector3 v) => t.position += v * Time.deltaTime;
    private GameObject _defaultTarget = null;

    private static EnemyFactory _instance;

    public static EnemyFactory Instance
    {
        get
        {
            if (_instance == null) _instance = new EnemyFactory();
            return _instance;
        }
    }

    public EnemyFactory()
    {
        // Set up the dictionary with methods.
        _enemySetups.Add("Turret", CreateTurret);
    }
    
    // TODO: Determine how we want our targeting, moving, and pathfinding to be passed in or set up.
    public GameObject CreateEnemy(string enemyName, Vector3 position)
    {
        // Janky, find a better place for this. Unsure when the Player object is available.
        if (_defaultTarget == null) _defaultTarget = GameObject.FindGameObjectWithTag("Player");
        
        // We expect enemyName to be the name of the resource.
        GameObject newEnemyObject = GameObject.Instantiate(Resources.Load(enemyName)) as GameObject;
        Transform enemyTransform = newEnemyObject.GetComponent<Transform>();
        
        // TODO: change default vector to dynamically adjust height of enemy spawn so they don't spawn under the ground.
        enemyTransform.position = position + new Vector3(0, 1, 0);
        
        // TODO: Determine whether we're including EnemyBehvaviour in the prefab, or adding it as component.
        // I'll just go with adding it for now.
        EnemyBehaviour eb = EnemyBehaviour.AddToGameObject(
            newEnemyObject, 
            _defaultTarget, 
            _defaultFunc, 
            Globals.DirectTargeting, 
            _defaultMove, 
            new Weapon(new BulletBehaviour(EntityType.ENEMY), 1f));
        if (_enemySetups.ContainsKey(enemyName)) _enemySetups[enemyName](eb);

        return newEnemyObject;
    }
    
    private void CreateTurret(EnemyBehaviour eb)
    {
        
    }
}
