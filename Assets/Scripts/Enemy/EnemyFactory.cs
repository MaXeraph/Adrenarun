using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define EnemyInfo for convenience.
public struct EnemyInfo
{
	public EnemyType enemyType;
	public Action<GameObject, Vector3> navAgentMove;
	public Func<Transform, Transform, Vector3> aim;
	public Action<Vector3> navAgentSetup;
	public AbstractAttackBehaviour attackBehaviour;
	public float fireRate;

	public EnemyInfo(EnemyType type, Action<GameObject, Vector3> navAgentMoveFunc, Func<Transform, Transform, Vector3> aimFunc,
		Action<Vector3> navAgentSetupFunc, AbstractAttackBehaviour attackBehaviour, float fireRate)
	{
		enemyType = type;
		navAgentMove = navAgentMoveFunc;
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
		_enemyInfo.Add(EnemyType.GRENADIER, new EnemyInfo(EnemyType.GRENADIER, EnemyMovements.GrenadierMovement, Globals.GrenadierTargeting, EnemyMovements.GrenadierSetup, new ArtilleryAttackBehaviour(EntityType.ENEMY, 5f), 3f));
		_enemyInfo.Add(EnemyType.RANGED, new EnemyInfo(EnemyType.RANGED, EnemyMovements.RangedMovement, Globals.DirectTargeting, EnemyMovements.RangedSetup, new BulletAttackBehaviour(EntityType.ENEMY, 5f, Globals.enemyBulletSpeeds[EnemyType.RANGED]), 1f));
		_enemyInfo.Add(EnemyType.TANK, new EnemyInfo(EnemyType.TANK, EnemyMovements.TankMovement, Globals.ForwardTargeting, EnemyMovements.TankSetup, new SweepAttackBehaviour(EntityType.ENEMY, 10f, 10f), 0.1f));
		AddHealerVariantToRoster();
		_enemyPostSetups.Add(EnemyVariantType.PREDICTIVE, CreatePredictiveVariant);
		// _enemyPostSetups.Add(EnemyVariantType.SHIELD, CreateShieldVariant);
	}

	public GameObject CreateEnemy(Vector3 position, EnemyType enemyType, EnemyVariantType variantType = EnemyVariantType.NONE)
	{
		// Janky, find a better place for this. Unsure when the Player object is available.
		if (_defaultTarget == null) _defaultTarget = GameObject.FindGameObjectWithTag("Player");

		if (!Globals.enemyPrefabNames.ContainsKey(enemyType)) return null;

		string enemyName = Globals.enemyPrefabNames[enemyType];

		GameObject newEnemyObject = ObjectPool.Create(enemyName);
		Transform enemyTransform = newEnemyObject.GetComponent<Transform>();
		CompassUI.addEnemy(newEnemyObject);

		// TODO: change default vector to dynamically adjust height of enemy spawn so they don't spawn under the ground.
		enemyTransform.position = position + new Vector3(0, 1, 0);

		EnemyInfo enemyInfo = _enemyInfo[enemyType];

		Weapon enemyWeapon = newEnemyObject.AddComponent<Weapon>();
		enemyWeapon.Initialize(enemyInfo.attackBehaviour, enemyInfo.fireRate);

		EnemyBehaviour eb = EnemyBehaviour.AddToGameObject(
			newEnemyObject,
			_defaultTarget,
			enemyInfo,
			enemyWeapon);
		if (variantType != EnemyVariantType.NONE && _enemyPostSetups.ContainsKey(variantType)) _enemyPostSetups[variantType](eb, enemyType);

		return newEnemyObject;
	}

	void AddHealerVariantToRoster()
	{
		// Set up the dictionary with methods.
		_enemyPostSetups.Add(EnemyVariantType.HEALER, CreateHealerVariant);
	}

	void CreateHealerVariant(EnemyBehaviour eb, EnemyType et)
	{
		AbstractAttackBehaviour enemyAttackBehaviour = eb.GetComponent<Weapon>()._attackBehaviour;
		if (enemyAttackBehaviour._damage > 0) enemyAttackBehaviour._damage *= -1;
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

	// void CreateShieldVariant(EnemyBehaviour eb, EnemyType et)
	// {
	// 	eb.gameObject.transform.Find("Shield").gameObject.SetActive(true);
	// }
}
