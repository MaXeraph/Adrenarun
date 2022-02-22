using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define EnemyInfo for convenience.
public struct EnemyInfo
{
    public Func<Transform, Transform, Vector3> pathfind;
    public Func<Transform, Transform, Vector3> aim;
    public Action<Transform, Vector3> move;

    public EnemyInfo(Func<Transform, Transform, Vector3> pathfindFunc, Func<Transform, Transform, Vector3> aimFunc,
        Action<Transform, Vector3> moveFunc)
    {
        pathfind = pathfindFunc;
        aim = aimFunc;
        move = moveFunc;
    }
}

/**
 * The goal of the EnemyFactory is to take a position, what kind of enemy we want, and spawn it there.
 * The enemy spawning mechanism will use this factory to spawn enemies.
 */
public class EnemyFactory
{

    // Additional setup for an enemy.
    private static Dictionary<EnemyType, Action<EnemyBehaviour>> _enemyPostSetups = new Dictionary<EnemyType, Action<EnemyBehaviour>>();
    private static Dictionary<EnemyType, EnemyInfo> _enemyInfo = new Dictionary<EnemyType, EnemyInfo>();
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
        // Define enemyInfo for each type.
        _enemyInfo.Add( EnemyType.TURRET, new EnemyInfo(_defaultFunc, Globals.DirectTargeting, _defaultMove));
        
        // Set up the dictionary with methods.
        _enemyPostSetups.Add(EnemyType.TURRET, CreateTurret);
    }
    
    public GameObject CreateEnemy(EnemyType enemyType, Vector3 position)
    {
        // Janky, find a better place for this. Unsure when the Player object is available.
        if (_defaultTarget == null) _defaultTarget = GameObject.FindGameObjectWithTag("Player");

        if (!Globals.enemyPrefabNames.ContainsKey(enemyType)) return null;
        
        string enemyName = Globals.enemyPrefabNames[enemyType];
        
        GameObject newEnemyObject = GameObject.Instantiate(Resources.Load(enemyName)) as GameObject;
        Transform enemyTransform = newEnemyObject.GetComponent<Transform>();
        
        // TODO: change default vector to dynamically adjust height of enemy spawn so they don't spawn under the ground.
        enemyTransform.position = position + new Vector3(0, 1, 0);
        
        Weapon enemyWeapon = newEnemyObject.AddComponent<Weapon>();
        enemyWeapon.Initialize(new BulletAttackBehaviour(EntityType.ENEMY), 1f);

        EnemyInfo enemyInfo = _enemyInfo[enemyType];
        
        EnemyBehaviour eb = EnemyBehaviour.AddToGameObject(
            newEnemyObject, 
            _defaultTarget, 
            enemyInfo.pathfind, 
            enemyInfo.aim, 
            enemyInfo.move, 
            enemyWeapon);
        if (_enemyPostSetups.ContainsKey(enemyType)) _enemyPostSetups[enemyType](eb);

        return newEnemyObject;
    }
    
    private void CreateTurret(EnemyBehaviour eb)
    {
        
    }
}
