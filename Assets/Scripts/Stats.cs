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
			_currentHealth = value;
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
				_currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
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

	public Stats(float maxHealth = 100f)
	{
		this.maxHealth = maxHealth;
	}

	void Awake()
	{
		// TODO: refactor for more dynamic assignment
		if (gameObject.tag == "Player")
		{
			owner = EntityType.PLAYER;
			UIManager.MaxHealth = maxHealth;
		}
		else owner = EntityType.ENEMY;
	}

	// Assuming enable only happens due to object pooling
	// Better to use object pooling's subscriptions to clean this up
	void OnEnable()
	{
		currentHealth = maxHealth;
	}

}
