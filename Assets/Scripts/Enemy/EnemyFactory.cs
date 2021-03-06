using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Define EnemyInfo for convenience.
public struct EnemyInfo
{
	public EnemyType enemyType;
	private Func<Action<GameObject, Vector3>> createNavAgentMove;
	public Action<GameObject, Vector3> navAgentMove
	{
		get { return createNavAgentMove();  }
	}
	public Func<Transform, Transform, Vector3> aim;
	public Action<GameObject> navAgentSetup;
	public AbstractAttackBehaviour attackBehaviour;
	public float fireRate;

	public EnemyInfo(EnemyType type, Action<GameObject, Vector3> navAgentMoveFunc, Func<Transform, Transform, Vector3> aimFunc,
		Action<GameObject> navAgentSetupFunc, AbstractAttackBehaviour attackBehaviour, float fireRate)
	{
		enemyType = type;
		createNavAgentMove = () => navAgentMoveFunc;
		aim = aimFunc;
		navAgentSetup = navAgentSetupFunc;
		this.attackBehaviour = attackBehaviour;
		this.fireRate = fireRate;
	}
	
	public EnemyInfo(EnemyType type, Func<Action<GameObject, Vector3>> createNavAgentMoveFunc, Func<Transform, Transform, Vector3> aimFunc,
		Action<GameObject> navAgentSetupFunc, AbstractAttackBehaviour attackBehaviour, float fireRate)
	{
		enemyType = type;
		createNavAgentMove = createNavAgentMoveFunc;
		aim = aimFunc;
		navAgentSetup = navAgentSetupFunc;
		this.attackBehaviour = attackBehaviour;
		this.fireRate = fireRate;
	}
}

/**
 * The goal of the EnemyFactory is to take a position, what kind of enemy we want, and spawn it there.
 * The enemy spawning mechanism will use this factory to spawn enemies.
 */
public class EnemyFactory
{
	// Additional setup for an enemy.
	private static Dictionary<EnemyVariantType, Action<EnemyBehaviour, EnemyType>> _enemyPostSetups = new Dictionary<EnemyVariantType, Action<EnemyBehaviour, EnemyType>>();
	private static Dictionary<EnemyType, EnemyInfo> _enemyInfo = new Dictionary<EnemyType, EnemyInfo>();
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
		AddTurretToRoster();
		_enemyInfo.Add(EnemyType.GRENADIER, new EnemyInfo(EnemyType.GRENADIER, EnemyMovements.CreateGrenadierMovement, Globals.GrenadierTargeting, EnemyMovements.GrenadierSetup, new ArtilleryAttackBehaviour(EntityType.ENEMY, 5f), 3f));
		_enemyInfo.Add(EnemyType.RANGED, new EnemyInfo(EnemyType.RANGED, EnemyMovements.CreateRangedMovement, Globals.DirectTargeting, EnemyMovements.RangedSetup, new BulletAttackBehaviour(EntityType.ENEMY, 5f, Globals.enemyBulletSpeeds[EnemyType.RANGED]), 1f));
		_enemyInfo.Add(EnemyType.TANK, new EnemyInfo(EnemyType.TANK, EnemyMovements.TankMovement, Globals.ForwardTargeting, EnemyMovements.TankSetup, new SweepAttackBehaviour(EntityType.ENEMY, 30f, 5f), 1f));
		_enemyInfo.Add(EnemyType.HEALER, new EnemyInfo(EnemyType.HEALER, EnemyMovements.CreateHealerMovement, Globals.DirectTargeting, EnemyMovements.HealerSetup, new HealerAttackBehaviour(EntityType.ENEMY, -0.5f), 0.1f));
		_enemyInfo.Add(EnemyType.FLYING, new EnemyInfo(EnemyType.FLYING, EnemyMovements.CreateFlyingMovement, Globals.DirectTargeting, EnemyMovements.FlyingSetup, new BulletAttackBehaviour(EntityType.ENEMY, 5f, Globals.enemyBulletSpeeds[EnemyType.FLYING]), 3f));
		_enemyPostSetups.Add(EnemyVariantType.PREDICTIVE, CreatePredictiveVariant);
		_enemyPostSetups.Add(EnemyVariantType.SHIELD, CreateShieldVariant);
		_enemyPostSetups.Add(EnemyVariantType.AGGRESSOR, CreateAggressorVariant);
	}

	public GameObject CreateEnemy(Vector3 position, EnemyType enemyType, EnemyVariantType variantType = EnemyVariantType.NONE, float scaleFactor = 1f)
	{
		// Janky, find a better place for this. Unsure when the Player object is available.
		if (_defaultTarget == null) _defaultTarget = GameObject.FindGameObjectWithTag("Player");

		if (!Globals.enemyPrefabNames.ContainsKey(enemyType)) return null;

		string enemyName = Globals.enemyPrefabNames[enemyType];

		GameObject newEnemyObject = ObjectPool.Create(enemyName);
		Transform enemyTransform = newEnemyObject.GetComponent<Transform>();
		CompassUI.addEnemy(newEnemyObject);

		// TODO: change default vector to dynamically adjust height of enemy spawn so they don't spawn under the ground.
		NavMeshAgent navAgent = newEnemyObject.GetComponent<NavMeshAgent>();
		if (navAgent != null) {
			navAgent.Warp(position);
		}
		else {
			enemyTransform.position = position + new Vector3(0, 1, 0);
		}

		EnemyInfo enemyInfo = _enemyInfo[enemyType];

		Weapon enemyWeapon = newEnemyObject.AddComponent<Weapon>();
		enemyWeapon.Initialize(enemyInfo.attackBehaviour, enemyInfo.fireRate);

		EnemyBehaviour eb = EnemyBehaviour.AddToGameObject(
			newEnemyObject,
			_defaultTarget,
			enemyInfo,
			enemyWeapon);
		if (variantType != EnemyVariantType.NONE && _enemyPostSetups.ContainsKey(variantType)) _enemyPostSetups[variantType](eb, enemyType);

		// scale enemies by scaleFactor
		enemyWeapon._attackBehaviour._damage = enemyWeapon._attackBehaviour._baseDamage * scaleFactor;
		Stats enemyStats = newEnemyObject.GetComponent<Stats>();
		enemyStats.maxHealth = enemyStats.baseMaxHealth * scaleFactor;
		enemyStats.currentHealth = enemyStats.maxHealth;
		
		return newEnemyObject;
	}

	void AddTurretToRoster()
	{
		BulletAttackBehaviour bulletBehaviour = new BulletAttackBehaviour(EntityType.ENEMY, 30f, Globals.enemyBulletSpeeds[EnemyType.TURRET]);
		float fireRate = 5f;

		// Define enemyInfo for each type.
		_enemyInfo.Add(EnemyType.TURRET, new EnemyInfo(EnemyType.TURRET, EnemyMovements.TurretMovement, Globals.DirectTargeting, EnemyMovements.TurretSetup, bulletBehaviour, fireRate));

		_enemyPostSetups.Add(EnemyVariantType.SET, CreateSetVariant);
	}

	void CreateSetVariant(EnemyBehaviour eb, EnemyType et)
	{
		Weapon weapon = eb.GetComponent<Weapon>();
		weapon.firingMods.Add(new SetFiringPowerUp());
	}

	void CreatePredictiveVariant(EnemyBehaviour eb, EnemyType et)
	{
		eb.GetAimDirection = Globals.CreatePredictiveTargeting(GameObject.FindGameObjectWithTag("Player").transform, Globals.enemyBulletSpeeds[et]);
	}

	void CreateShieldVariant(EnemyBehaviour eb, EnemyType et)
	{
		eb.gameObject.transform.Find("Shield").gameObject.SetActive(true);
	}
	
	void CreateAggressorVariant(EnemyBehaviour eb, EnemyType et)
	{
		eb.gameObject.GetComponent<Stats>().moveSpeed = 14f;
		eb.gameObject.GetComponent<Stats>().maxHealth = 250f;
		eb.gameObject.GetComponent<Stats>().baseMaxHealth = 250f;
		eb.gameObject.GetComponent<Stats>().currentHealth = 250f;
	}
}
