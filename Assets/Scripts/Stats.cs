using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: refactor away from MonoBehaviour
public class Stats : MonoBehaviour
{
	private float _currentHealth;
	
	public float currentHealth
	{
		get => _currentHealth;
		set
		{
			// if (value - _currentHealth < 0) Debug.Log((value - _currentHealth) + " | actual: " + (value - _currentHealth) * _damageTakenMultiplier);
			if (owner == EntityType.PLAYER) Debug.Log(value);
			if (value - _currentHealth < 0) _currentHealth = _currentHealth + (value - _currentHealth) * _damageTakenMultiplier;
			else _currentHealth = value;
			if (value <= 0 && owner == EntityType.ENEMY)
			{
				EnemyBehaviour.Destroy(gameObject);
				int rand = Random.Range(0, 20);
				if (rand == 0)
				{
					HealingPill.SpawnPill(gameObject.transform.position);
				}
			}
			else if (owner == EntityType.PLAYER)
			{
				_currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth*2);
				SpeedManager.updateSpeeds(_currentHealth / maxHealth);
				UIManager.Health = _currentHealth;
				if (_currentHealth == 0)
				{
					AudioManager.UpdateMusicAudio(1);
				}
				else
				{
					AudioManager.UpdateMusicAudio(_currentHealth / maxHealth);

				}
				AudioManager.PlayInjuryAudio();
			}
		}
	}
	public float maxHealth;
	public EntityType owner;
	bool dead = false;
	public Dictionary<object, float> damageTakenMultipliers;

	private float _damageTakenMultiplier
	{
		get {
			float total = 1f;
			Dictionary<object, float>.ValueCollection multipliers = damageTakenMultipliers.Values;
			foreach (float mult in multipliers) {
				total *= mult;
			}
			return total;
		}
	}

	public Stats(float maxHealth = 100f)
	{
		this.maxHealth = maxHealth;
	}

	void Awake()
	{
		damageTakenMultipliers = new Dictionary<object, float>();
	}
	
	void Start()
	{
		// TODO: refactor for more dynamic assignment
		if (gameObject.tag == "Player")
		{
			owner = EntityType.PLAYER;
			UIManager.MaxHealth = maxHealth;
		}
		else owner = EntityType.ENEMY;

		currentHealth = maxHealth;
	}

	// Assuming enable only happens due to object pooling
	// Better to use object pooling's subscriptions to clean this up
	void OnEnable()
	{
		_currentHealth = maxHealth;
	}
}
